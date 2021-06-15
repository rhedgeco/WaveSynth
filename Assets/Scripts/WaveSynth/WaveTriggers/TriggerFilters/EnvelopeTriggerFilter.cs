using UnityEngine;

namespace WaveSynth.WaveTriggers.TriggerFilters
{
    public class EnvelopeTriggerFilter : TriggerFilter
    {
        [SerializeField] private float attackMs = 2f;
        [SerializeField] private float releaseMs = 15f;

        private WaveTriggerIndexer _cache = new WaveTriggerIndexer();

        public override WaveTriggerIndexer FilterTriggers(WaveTriggerIndexer indexer)
        {
            // apply attack
            for (int i = 0; i < indexer.Count; i++)
            {
                int rawIndex = indexer.GetRawIndex(i);
                Trigger self = _cache.GetTrigger(rawIndex);
                Trigger other = indexer.GetTrigger(rawIndex);
                
                if (self.UniqueId != other.UniqueId)
                {
                    _cache.AddTrigger(other.UniqueId, other.Frequency, 0, rawIndex);
                }
                else // trigger is active
                {
                    if (self.Amplitude < other.Amplitude)
                    {
                        float increase = 1f / (attackMs * (WaveSettings.SampleRate / 1000f));
                        self.Amplitude = Mathf.Clamp(self.Amplitude + increase, 0, other.Amplitude);
                    }
                }
            }

            // apply release
            for (int i = 0; i < _cache.Count; i++)
            {
                int rawIndex = _cache.GetRawIndex(i);
                Trigger self = _cache.GetTrigger(rawIndex);
                Trigger other = indexer.GetTrigger(rawIndex);
            
                if (other.UniqueId != self.UniqueId)
                {
                    float decrease = 1f / (releaseMs * (WaveSettings.SampleRate / 1000f));
                    self.Amplitude -= decrease;
                    if (self.Amplitude <= 0) _cache.RemoveTrigger(rawIndex);
                }
            }

            return _cache;
        }
    }
}