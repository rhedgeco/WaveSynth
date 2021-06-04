using UnityEngine;

namespace WaveSynth.Generators
{
    public abstract class FunctionGenerator : AudioOutput
    {
        [SerializeField] private MidiTrigger trigger;

        private long _timeIndex;

        protected override void ProcessBuffer(ref float[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = FunctionSample(_timeIndex + i);
            }

            if (!trigger) return;
            if (trigger.Frequencies == null) return;
            if (trigger.Frequencies.Count == 0) _timeIndex = 0;
            else _timeIndex += buffer.Length;
        }

        private float FunctionSample(long timeIndex)
        {
            if (!trigger) return 0;
            if (trigger.Frequencies == null) return 0;

            int midiCount = trigger.Frequencies.Count;
            float amplitude = 1f / midiCount;
            
            float sample = 0, loop;
            foreach (float frequency in trigger.Frequencies)
            {
                loop = timeIndex % (GlobalAudioController.SampleRate / frequency);
                sample += WaveFunction(loop * frequency / GlobalAudioController.SampleRate) * amplitude;
            }
            
            return sample;
        }

        protected abstract float WaveFunction(float value);
    }
}