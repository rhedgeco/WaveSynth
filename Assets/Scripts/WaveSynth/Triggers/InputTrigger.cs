using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WaveSynth.FrequencyHandlers;

namespace WaveSynth.Triggers
{
    public class InputTrigger : AudioTrigger
    {
        [SerializeField] private InputAction action;
        [SerializeField] private List<KeyboardNote> keys = new List<KeyboardNote>();

        private bool _pressed;
        private List<TriggerFrequency> _empty = new List<TriggerFrequency>();
        private List<TriggerFrequency> _frequencies = new List<TriggerFrequency>();

        private void Awake()
        {
            foreach (KeyboardNote key in keys)
            {
                float freq = FrequencyTable.GetEqualTemperedFrequency(key.key, key.octave);
                _frequencies.Add(new TriggerFrequency(freq, 1));
            }
            
            action.performed += context => _pressed = true;
            action.canceled += context => _pressed = false;
            action.Enable();
        }

        protected override List<TriggerFrequency> ProcessFrequencies()
        {
            return _pressed ? _frequencies : _empty;
        }
    }
}