using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.Triggers.Filters
{
    public abstract class TriggerFilter : AudioTrigger
    {
        [SerializeField] private AudioTrigger triggerInput;
        
        protected override List<TriggerFrequency> ProcessFrequencies(int bufferLength)
        {
            return ProcessTriggerFilter(triggerInput.CalculateActiveFrequencies(bufferLength));
        }

        protected abstract List<TriggerFrequency> ProcessTriggerFilter(List<TriggerFrequency> frequencies);
    }
}