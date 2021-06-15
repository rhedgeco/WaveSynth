using UnityEngine;

namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTriggerOutput : MonoBehaviour
    {
        public abstract WaveTriggerIndexer GetSampleTriggers();

        public class Trigger
        {
            public float Frequency;
            public float Amplitude;

            public Trigger(float frequency, float amplitude)
            {
                Frequency = frequency;
                Amplitude = amplitude;
            }
        }
    }
}