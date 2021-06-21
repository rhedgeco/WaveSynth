using UnityEngine;

namespace WaveSynth.WaveMidi.MidiFilters
{
    public class EnvelopeMidiFilter : WaveMidiCache
    {
        [SerializeField] private WaveMidiOutput midiSource;
        [SerializeField] private float attackMs = 2f;
        [SerializeField] private float releaseMs = 15f;

        private readonly float[] _envelopes = new float[MidiState.MidiKeyCount];

        protected override void ProcessMidiBuffer(ref MidiState[] buffer)
        {
            MidiState[] outStates = midiSource.GetMidiBuffer();

            for (int stateIndex = 0; stateIndex < buffer.Length; stateIndex++)
            {
                MidiState.Key[] outStateKeys = outStates[stateIndex].Keys;
                for (int keyIndex = 0; keyIndex < outStateKeys.Length; keyIndex++)
                {
                    bool active = outStateKeys[keyIndex].Active;
                    int sampleTime = outStateKeys[keyIndex].SampleTime;
                    float amplitude = outStateKeys[keyIndex].Amplitude;
                    
                    buffer[stateIndex].Keys[keyIndex].SampleTime = sampleTime;
                    if (sampleTime == 0) _envelopes[keyIndex] = 0;

                    // attack
                    if (active && _envelopes[keyIndex] < 1)
                    {
                        float increase = 1f / (attackMs * (WaveSettings.SampleRate / 1000f));
                        _envelopes[keyIndex] = Mathf.Min(_envelopes[keyIndex] + increase, 1);
                    }

                    // release
                    if (!active && _envelopes[keyIndex] > 0)
                    {
                        float decrease = 1f / (releaseMs * (WaveSettings.SampleRate / 1000f));
                        _envelopes[keyIndex] = Mathf.Max(_envelopes[keyIndex] - decrease, 0);
                    }

                    buffer[stateIndex].Keys[keyIndex].Active = _envelopes[keyIndex] > 0.0001;
                    buffer[stateIndex].Keys[keyIndex].Amplitude = amplitude * _envelopes[keyIndex];
                }
            }
        }
    }
}