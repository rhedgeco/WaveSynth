using UnityEngine;
using UnityEngine.InputSystem;

namespace WaveSynth.WaveTriggers
{
    public class KeyWaveTrigger : WaveTriggerRoot
    {
        [SerializeField] private float frequency = 440;
        [SerializeField] private InputAction action;

        private bool _playing;
        private int _timeProgress;

        private void Awake()
        {
            _playing = false;
            _timeProgress = 0;
            
            action.started += context => _playing = true;
            action.canceled += context => _playing = false;
            action.Enable();
        }

        protected override void ProcessTriggerBuffer(ref TriggerRange[] triggers)
        {
            for (int i = 0; i < triggers.Length; i++)
            {
                TriggerRange range = triggers[i];
                if (!_playing)
                {
                    range.ActiveLength = 0;
                    _timeProgress = 0;
                    continue;
                }

                range.ActiveLength = 1;
                range.Triggers[0].Frequency = frequency;
                range.Triggers[0].Amplitude = 1;
                range.Triggers[0].SampleProgress = _timeProgress++;
            }
        }
    }
}