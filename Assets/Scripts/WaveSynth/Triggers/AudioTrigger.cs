using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.Triggers
{
    public abstract class AudioTrigger : MonoBehaviour
    {
        public abstract List<float> GetActiveFrequencies();
    }
}