using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WaveSynth.Triggers;
using Random = System.Random;

namespace WaveSynth.Generators
{
    public abstract class FunctionGenerator : AudioOutput
    {
        // @formatter:off
        [Header("Generator")]
        [SerializeField] [Range(0, 1)] private float amplitude = 0.3f;
        [SerializeField] [Range(0, 1)] private float phase = 0;
        [SerializeField] [Range(0, 1)] private float randomPhase = 1;
        [SerializeField] private AudioTrigger trigger;
        
        [Header("Envelope")]
        [SerializeField] private float attackMS = 2f;
        [SerializeField] private float releaseMS = 15f;
        // @formatter:on

        private float _attackSpeed = 1;
        private float _releaseSpeed = 1;
        private List<EnvelopeData> _envelopes = new List<EnvelopeData>();
        private Random _random = new Random();

        protected override void ProcessBuffer(ref float[] buffer)
        {
            if (!trigger) return;
            _attackSpeed = (GlobalAudioController.SampleRate / 1000) * attackMS;
            _releaseSpeed = (GlobalAudioController.SampleRate / 1000) * releaseMS;
            List<TriggerFrequency> newFrequencies = trigger.CalculateActiveFrequencies(buffer.Length);

            // prune envelope map
            for (int i = _envelopes.Count - 1; i >= 0; i--)
            {
                EnvelopeData data = _envelopes[i];
                data.Active = newFrequencies.Contains(data.Trigger);
                if (!data.Active && data.Amplitude <= 0.0001f) _envelopes.RemoveAt(i);
            }

            // add new frequencies to envelope map
            foreach (TriggerFrequency frequency in newFrequencies)
            {
                if (_envelopes.Any(data => data.Trigger == frequency)) continue;
                _envelopes.Add(new EnvelopeData(frequency, (float) _random.NextDouble() - 0.5f));
            }

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = FunctionSample();
            }
        }

        private float FunctionSample()
        {
            int midiCount = _envelopes.Count;

            float sample = 0;
            for (int i = 0; i < midiCount; i++)
            {
                EnvelopeData envelope = _envelopes[i];
                float freqValue = envelope.Trigger.Frequency;
                
                // apply phasing to progress
                float progress = envelope.BufferProgress % (GlobalAudioController.SampleRate / freqValue);
                progress *= freqValue / GlobalAudioController.SampleRate;
                progress += phase;
                progress += envelope.RandomPhase * randomPhase;
                progress %= 1;
                
                float wavePart = WaveFunction(progress);
                sample += wavePart * envelope.Amplitude * amplitude;

                // apply attack and release
                if (envelope.Active && envelope.Amplitude < 1) envelope.ModifyAmplitude(1 / _attackSpeed);
                if (!envelope.Active && envelope.Amplitude > 0) envelope.ModifyAmplitude(-1 / _releaseSpeed);
                envelope.AddToBuffer(1);
            }

            return sample;
        }

        protected abstract float WaveFunction(float value);

        private class EnvelopeData
        {
            public TriggerFrequency Trigger { get; }
            public int BufferProgress { get; private set; }
            public float Amplitude { get; private set; }
            public float RandomPhase { get; }
            public bool Active { get; set; }

            public EnvelopeData(TriggerFrequency trigger, float randomPhase)
            {
                Trigger = trigger;
                BufferProgress = 0;
                Amplitude = 0;
                RandomPhase = randomPhase;
                Active = true;
            }

            public void ModifyAmplitude(float amount) => Amplitude = Mathf.Clamp(Amplitude + amount, 0, 1);
            public void AddToBuffer(int amount) => BufferProgress += amount;
        }
    }
}