using UnityEngine;
using UnityEngine.Profiling;
using WaveSynth.WaveTriggers;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public abstract class FunctionGenerator : WaveRoot
    {
        [SerializeField] private WaveTriggerOutput triggerOutput;
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
                float sample = 0;
                WaveTriggerIndexer indexer = triggerOutput.GetSampleTriggers();
                if (indexer.Count == 0)
                {
                    buffer[sampleIndex] = buffer[sampleIndex + 1] = sample;
                    continue;
                }
                
                for (int i = 0; i < indexer.Count; i++)
                {
                    int rawIndex = indexer.GetRawIndex(i);
                    TriggerHolder holder = _triggers[rawIndex];
                    WaveTriggerOutput.Trigger trigger = indexer.GetTrigger(rawIndex);
                    float triggerFrequency = trigger.Frequency;
                    float triggerAmplitude = trigger.Amplitude;
                    
                    if (holder.Trigger == trigger)
                    {
                        holder.Phase += triggerFrequency / WaveSettings.SampleRate;
                    }
                    else
                    {
                        holder.Trigger = trigger;
                        holder.Phase = 0;
                    }
                    
                    sample += SampleFunction(holder.Phase) * triggerAmplitude * amplitude;
                }
                
                buffer[sampleIndex] = buffer[sampleIndex + 1] = sample;
            }
        }

        protected abstract float SampleFunction(float phase);

        private class TriggerHolder
        {
            public WaveTriggerOutput.Trigger Trigger;
            public float Phase;

            public void Set(WaveTriggerOutput.Trigger trigger, float phase)
            {
                Trigger = trigger;
                Phase = phase;
            }
        }
    }
}