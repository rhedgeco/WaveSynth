using UnityEngine;
using WaveSynth.WaveTriggers;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public abstract class FunctionGenerator : WaveRoot
    {
        [SerializeField] private WaveTriggerOutput triggerOutput;
        [SerializeField] private bool output = true;
        [SerializeField] [Range(0, 1)] private float amplitude = 0.8f;

        private TriggerHolder[] _triggers = new TriggerHolder[WaveSettings.MaxTriggerCount];

        private void Awake()
        {
            for (int i = 0; i < _triggers.Length; i++)
                _triggers[i] = new TriggerHolder();
        }

        protected override void ProcessWaveBuffer(ref float[] buffer)
        {
            for (int sampleIndex = 0; sampleIndex < buffer.Length; sampleIndex += 2)
            {
                WaveTriggerIndexer indexer = triggerOutput.GetSampleTriggers();
                if (indexer.Count == 0 || !output)
                {
                    buffer[sampleIndex] = buffer[sampleIndex + 1] = 0;
                    continue;
                }
                
                float sample = 0;
                for (int i = 0; i < indexer.Count; i++)
                {
                    int rawIndex = indexer.GetRawIndex(i);
                    TriggerHolder holder = _triggers[rawIndex];
                    WaveTriggerOutput.Trigger trigger = indexer.GetTrigger(rawIndex);
                    float triggerFrequency = trigger.Frequency;
                    float triggerAmplitude = trigger.Amplitude;

                    if (holder.Trigger == null || holder.Trigger.UniqueId != trigger.UniqueId)
                    {
                        holder.Trigger = trigger;
                        holder.Phase = 0;
                    }
                    else
                    {
                        holder.Phase += triggerFrequency / WaveSettings.SampleRate;
                    }

                    sample += SampleFunction(holder.Phase) * triggerAmplitude * amplitude;
                }
                
                buffer[sampleIndex] = buffer[sampleIndex + 1] = sample;
            }
        }

        protected abstract float SampleFunction(double phase);

        private class TriggerHolder
        {
            public WaveTriggerOutput.Trigger Trigger;
            public double Phase;

            public void Set(WaveTriggerOutput.Trigger trigger, double phase)
            {
                Trigger = trigger;
                Phase = phase;
            }
        }
    }
}