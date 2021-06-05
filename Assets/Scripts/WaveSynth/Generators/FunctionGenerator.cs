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
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = FunctionSample(_timeIndex + i);
            }

            if (!trigger) return;
            List<float> frequencies = trigger.GetActiveFrequencies();
            if (frequencies.Count == 0) _timeIndex = 0;
            else _timeIndex += GlobalAudioController.BufferSize;
        }

        private float FunctionSample(long timeIndex)
        {
            if (!trigger) return 0;
            List<float> frequencies = trigger.GetActiveFrequencies();

            int midiCount = frequencies.Count;
            float amplitude = 1f / midiCount;
            
            float sample = 0, loop, frequency;
            for(int i = 0; i < midiCount; i++)
            {
                frequency = frequencies[i];
                loop = timeIndex % (GlobalAudioController.SampleRate / frequency);
                sample += WaveFunction(loop * frequency / GlobalAudioController.SampleRate) * amplitude;
            }
            
            return sample;
        }

        protected abstract float WaveFunction(float value);
    }
}