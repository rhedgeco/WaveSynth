using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WaveSynth.WaveTriggers
{
    public class KeyWaveTrigger : WaveTrigger
    {
        [SerializeField] private float frequency = 440;
        [SerializeField] private InputAction _action;

        private bool _changed;
        private bool _playing;
        private HashSet<Trigger> _triggerCache = new HashSet<Trigger>();

        private void Awake()
        {
            _action.started += context =>
            {
                print("start");
                _playing = true;
                _changed = true;
            };
            _action.canceled += context =>
            {
                print("stop");
                _playing = false;
                _changed = true;
            };
            _action.Enable();
        }

        public override HashSet<Trigger> GetTriggers(int bufferLength)
        {
            if (!_changed) return _triggerCache;
            _triggerCache.Clear();
            if (_playing) _triggerCache.Add(new Trigger(frequency, 1));
            _changed = false;
            return _triggerCache;
        }
    }
}