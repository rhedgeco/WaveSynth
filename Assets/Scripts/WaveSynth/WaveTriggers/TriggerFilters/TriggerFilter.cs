using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.WaveTriggers.TriggerFilters
{
    public abstract class TriggerFilter : WaveTrigger
    {
        [SerializeField] private WaveTrigger trigger;
        
        public override HashSet<Trigger> GetTriggers(int bufferLength)
        {
            HashSet<Trigger> triggers = trigger.GetTriggers(bufferLength);
            triggers = ProcessTriggers(triggers, bufferLength);
            return triggers;
        }

        protected abstract HashSet<Trigger> ProcessTriggers(HashSet<Trigger> triggers, int bufferLength);
    }
}