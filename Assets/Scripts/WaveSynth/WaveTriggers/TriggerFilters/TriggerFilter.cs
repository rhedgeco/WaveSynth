using UnityEngine;

namespace WaveSynth.WaveTriggers.TriggerFilters
{
    public abstract class TriggerFilter : WaveTriggerOutput
    {
        [SerializeField] private WaveTriggerOutput triggerOutput;

        public override WaveTriggerIndexer GetSampleTriggers()
        {
            WaveTriggerIndexer indexer = triggerOutput.GetSampleTriggers();
            return FilterTriggers(indexer);
        }

        public abstract WaveTriggerIndexer FilterTriggers(WaveTriggerIndexer indexer);
    }
}