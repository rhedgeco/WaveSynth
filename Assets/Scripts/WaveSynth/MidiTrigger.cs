using System.Collections.Generic;
using UnityEngine;
using WaveSynth.FrequencyHandlers;

namespace WaveSynth
{
    public class MidiTrigger : MonoBehaviour
    {
        [SerializeField] private List<KeyboardNote> keys = new List<KeyboardNote>();

        public List<float> Frequencies => _pressed ? _frequencies : _empty;

        private bool _pressed;
        private List<float> _empty = new List<float>();
        private List<float> _frequencies = new List<float>();

        private void Awake()
        {
            foreach (KeyboardNote key in keys)
            {
                _frequencies.Add(FrequencyTable.GetEqualTemperedFrequency(key.key, key.octave));
            }
        }

        private void Update()
        {
            _pressed = Input.GetKey(KeyCode.Space);
        }
    }
}