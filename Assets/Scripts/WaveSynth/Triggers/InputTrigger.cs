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
        private List<float> _empty = new List<float>();
        private List<float> _frequencies = new List<float>();

        private void Awake()
        {
            foreach (KeyboardNote key in keys)
            {
                _frequencies.Add(FrequencyTable.GetEqualTemperedFrequency(key.key, key.octave));
            }
            
            action.performed += context => _pressed = true;
            action.canceled += context => _pressed = false;
            action.Enable();
        }

        public override List<float> GetActiveFrequencies()
        {
            return _pressed ? _frequencies : _empty;
        }
    }
}