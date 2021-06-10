using System.Collections.Generic;
using UnityEngine;
using WaveSynth.WaveTriggers;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public abstract class FunctionGenerator : WaveOutput
    {
        [SerializeField] private WaveTrigger trigger;
        [SerializeField] [Range(0, 1)] private float amplitude = 0.8f;
        
        private Dictionary<WaveTrigger.Trigger, int> _bufferProgress =
            new Dictionary<WaveTrigger.Trigger, int>();

        protected override void ProcessWaveBuffer(ref float[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
                HashSet<WaveTrigger.Trigger> triggers = trigger.GetTriggers(1);
                foreach (WaveTrigger.Trigger frequencyTrigger in triggers)
                {
                    if (!_bufferProgress.ContainsKey(frequencyTrigger)) 
                        _bufferProgress.Add(frequencyTrigger, 0);
                    int progress = _bufferProgress[frequencyTrigger]++;
                    float phase = progress % (WaveSettings.SampleRate / frequencyTrigger.Frequency);
                    phase *= frequencyTrigger.Frequency / WaveSettings.SampleRate;
                    buffer[i] += SampleFunction(phase % 1) * amplitude * frequencyTrigger.Amplitude;
                }
            }
        }

        protected abstract float SampleFunction(float phase);
    }
}