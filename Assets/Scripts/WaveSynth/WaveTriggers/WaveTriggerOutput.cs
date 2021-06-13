using UnityEngine;

namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTriggerOutput : MonoBehaviour
    {
        private const int MaxTriggerCount = 512;
        
        public abstract TriggerRange[] GetTriggers();
        
        public class TriggerRange
        {
            public int ActiveLength = 0;
            public Trigger[] Triggers { get; } = new Trigger[MaxTriggerCount];
        }

        public struct Trigger
        {
            public int SampleProgress;
            public float Frequency;
            public float Amplitude;
        }
    }
}