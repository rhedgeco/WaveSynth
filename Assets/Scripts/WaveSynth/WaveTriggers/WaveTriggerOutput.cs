using System;
using UnityEngine;

namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTriggerOutput : MonoBehaviour
    {
        public abstract WaveTriggerIndexer GetSampleTriggers();

        public class Trigger
        {
            public string UniqueId;
            public float Frequency;
            public float Amplitude;

            public Trigger(float frequency, float amplitude)
            {
                UniqueId = Guid.NewGuid().ToString();
                Frequency = frequency;
                Amplitude = amplitude;
            }

            public Trigger(string uniqueId, float frequency, float amplitude)
            {
                UniqueId = uniqueId;
                Frequency = frequency;
                Amplitude = amplitude;
            }

            public void Set(float frequency, float amplitude)
            {
                Frequency = frequency;
                Amplitude = amplitude;
            }
        }
    }
}