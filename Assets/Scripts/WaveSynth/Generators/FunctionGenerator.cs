using System.Collections.Generic;
using UnityEngine;
using WaveSynth.Triggers;

namespace WaveSynth.Generators
{
    public abstract class FunctionGenerator : AudioOutput
    {
        [SerializeField] private AudioTrigger trigger;

        private long _timeIndex;

        protected override void ProcessBuffer(ref float[] buffer)
        {
            List<AudioTrigger.TriggerFrequency> frequencies = trigger.CalculateActiveFrequencies();
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = FunctionSample(_timeIndex + i, frequencies);
            }

            if (!trigger) return;
            if (trigger.ActiveFrequencyCount == 0) _timeIndex = 0;
            else _timeIndex += buffer.Length;
        }

        private float FunctionSample(long timeIndex, List<AudioTrigger.TriggerFrequency> frequencies)
        {
            if (!trigger) return 0;

            int midiCount = frequencies.Count;
            float amplitude = 1f / midiCount;

            float sample = 0, loop, frequency;
            for (int i = 0; i < midiCount; i++)
            {
                frequency = frequencies[i].Frequency;
                loop = timeIndex % (GlobalAudioController.SampleRate / frequency);
                sample += WaveFunction(loop * frequency / GlobalAudioController.SampleRate) * amplitude;
            }

            return sample;
        }

        protected abstract float WaveFunction(float value);
    }
}