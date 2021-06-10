using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTrigger : MonoBehaviour
    {
        public abstract HashSet<Trigger> GetTriggers(int bufferLength);
        
        public class Trigger
        {
            public float Frequency { get; set; }
            public float Amplitude { get; set; }

            public Trigger(float frequency, float amplitude)
            {
                Frequency = frequency;
                Amplitude = amplitude;
            }
        }
    }
}