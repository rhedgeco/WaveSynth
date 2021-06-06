using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.Triggers
{
    public abstract class AudioTrigger : MonoBehaviour
    {
        private int _lastAccessID = -1;
        private List<TriggerFrequency> _frequencyCache = new List<TriggerFrequency>(512);

        public List<TriggerFrequency> CalculateActiveFrequencies(int bufferLength)
        {
            if (GlobalAudioController.AccessID == _lastAccessID) 
                return _frequencyCache;
            _lastAccessID = GlobalAudioController.AccessID;
            
            List<TriggerFrequency> frequencies = ProcessFrequencies(bufferLength);
            _frequencyCache.Clear();
            _frequencyCache.AddRange(frequencies);
            return _frequencyCache;
        }

        protected abstract List<TriggerFrequency> ProcessFrequencies(int bufferLength);
    }
}