using UnityEngine;
using WaveSynth.WaveTriggers;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public abstract class FunctionGenerator : WaveRoot
    {
        [SerializeField] private WaveTriggerOutput triggerRoot;
        [SerializeField] [Range(0, 1)] private float amplitude = 0.8f;

        protected override void ProcessWaveBuffer(ref float[] buffer)
        {
            WaveTriggerOutput.TriggerRange[] sampleTriggers = triggerRoot.GetTriggers();
            for (int sampleIndex = 0; sampleIndex < buffer.Length; sampleIndex += 2)
            {
                float sample = 0;
                WaveTriggerOutput.TriggerRange triggers = sampleTriggers[sampleIndex / 2];
                if (triggers.ActiveLength == 0)
                {
                    buffer[sampleIndex] = buffer[sampleIndex + 1] = sample;
                    continue;
                }

                WaveTriggerOutput.Trigger trigger;
                for (int triggerIndex = 0; triggerIndex < triggers.ActiveLength; triggerIndex++)
                {
                    trigger = triggers.Triggers[triggerIndex];
                    int progress = triggers.Triggers[triggerIndex].SampleProgress;
                    float phase = progress % (WaveSettings.SampleRate / trigger.Frequency);
                    phase *= trigger.Frequency / WaveSettings.SampleRate;
                    sample += SampleFunction(phase % 1) * trigger.Amplitude * amplitude;
                }

                buffer[sampleIndex] = buffer[sampleIndex + 1] = sample;
            }
        }

        protected abstract float SampleFunction(float phase);
    }
}