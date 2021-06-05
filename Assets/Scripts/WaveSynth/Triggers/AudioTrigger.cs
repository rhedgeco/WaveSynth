using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.Triggers
{
    public abstract class AudioTrigger : MonoBehaviour
    {
        private int _lastAccessID = -1;
        private List<float> _frequencyCache = new List<float>();

        public List<float> GetActiveFrequencies()
        {
            if (GlobalAudioController.AccessID == _lastAccessID) 
                return _frequencyCache;
            _lastAccessID = GlobalAudioController.AccessID;

            _frequencyCache = ProcessFrequencies();
            return _frequencyCache;
        }

        protected abstract List<float> ProcessFrequencies();
    }
}