using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.Triggers
{
    public abstract class AudioTrigger : MonoBehaviour
    {
        private int _lastAccessID = -1;
        private List<TriggerFrequency> _frequencyCache = new List<TriggerFrequency>(256);

        public int ActiveFrequencyCount => _frequencyCache.Count;

        public List<TriggerFrequency> CalculateActiveFrequencies()
        {
            if (GlobalAudioController.AccessID == _lastAccessID) 
                return _frequencyCache;
            _lastAccessID = GlobalAudioController.AccessID;
            
            _frequencyCache = ProcessFrequencies();
            return _frequencyCache;
        }

        protected abstract List<TriggerFrequency> ProcessFrequencies();
        
        public struct TriggerFrequency
        {
            public float Frequency { get; }
            public float Amplitude { get; }

            public TriggerFrequency(float frequency, float amplitude)
            {
                Frequency = frequency;
                Amplitude = amplitude;
            }
        }
    }
}