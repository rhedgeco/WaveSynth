using Unity.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using WaveSynth.FrequencyHandlers;
using WaveSynth.WaveMidi;

namespace WaveSynth.WaveOutputs.WaveGenerators {
    public abstract class FunctionGenerator : WaveCache {
        public const int MaxVoices = 16;

        [SerializeField] private WaveMidiOutput midiSource;
        [SerializeField] [Range(0, 1)] private float amplitude = 0.8f;

        [Header("Phase Control")]
        [SerializeField] [Range(0, 1)] private float phaseOffset = 0.5f;
        [SerializeField] [Range(0, 0.5f)] private float randomPhase = 0.5f;

        [Header("Voices")]
        [SerializeField] [Range(1, MaxVoices)] private int voices = 1;
        [SerializeField] [Range(0, 1)] private float detune = 1;
        [SerializeField] private float maxDetune = 12f;

        private System.Random _random = new System.Random();
        private WaveState[] _waveStates = new WaveState[MidiState.MidiKeyCount];

        protected override void ProcessWaveBuffer(ref NativeArray<float> buffer) {
            Profiler.BeginSample("ProcessWaveBuffer");
            Profiler.BeginSample("FunctionGetMidi");
            MidiState[] states = midiSource.GetMidiBuffer();
            Profiler.EndSample();
            for (int sampleIndex = 0; sampleIndex < buffer.Length; sampleIndex += 2) {
                MidiState state = states[sampleIndex / 2];
                MidiState.Key[] keys = state.Keys;

                float sampleL = 0;
                float sampleR = 0;
                for (int i = 0; i < keys.Length; i++) {
                    if (!keys[i].Active) continue;
                    if (_waveStates[i].VoicePhases == null)
                        _waveStates[i].VoicePhases = new double[MaxVoices];

                    int halfStep = i % 12;
                    uint octave = (uint) (i / 12);
                    Profiler.BeginSample("GetFrequency");
                    float keyFrequency = FrequencyTable.GetEqualTemperedFrequency(halfStep, octave);
                    Profiler.EndSample();
                    float keyAmplitude = keys[i].Amplitude;

                    if (keys[i].SampleTime == 0) {
                        _waveStates[i].PhaseOffset = CreatePhase();
                        _waveStates[i].SampleRef = 0;
                    }

                    int sampleProgress = keys[i].SampleTime - _waveStates[i].SampleRef;
                    double phase = (keyFrequency / WaveSettings.SampleRate) * sampleProgress;
                    Profiler.BeginSample("SampleFunction");
                    float sample = SampleFunction((phase + _waveStates[i].PhaseOffset) % 1) * keyAmplitude * amplitude;
                    Profiler.EndSample();
                    sampleL += sample;
                    sampleR += sample;
                }

                buffer[sampleIndex + 0] = sampleL;
                buffer[sampleIndex + 1] = sampleR;
            }

            Profiler.EndSample();
        }

        private double CreatePhase() {
            double phase = phaseOffset + _random.NextDouble() * randomPhase;
            return phase % 1;
        }

        protected abstract float SampleFunction(double phase);

        private struct WaveState {
            public int SampleRef;
            public double PhaseOffset;
            public double[] VoicePhases;
        }
    }
}