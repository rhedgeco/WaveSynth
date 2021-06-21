using System.Collections.Generic;
using Minis;
using UnityEngine;

namespace WaveSynth.WaveMidi.MidiGenerators
{
    public class KeyboardMidiGenerator : WaveMidiCache
    {
        [SerializeField] [Range(0, 1)] private float velocityFloor;
        
        private KeyState[] _keyStates = new KeyState[MidiState.MidiKeyCount];
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        
        private void Update()
        {
            MidiDevice midi = MidiDevice.current;
            if (midi == null) return;
            if (_devices.Contains(midi)) return;
            _devices.Add(midi);

            midi.onWillNoteOn += (control, f) =>
            {
                int noteNumber = control.noteNumber;
                _keyStates[noteNumber].Active = true;
                _keyStates[noteNumber].SampleTime = 0;
                _keyStates[noteNumber].Velocity = Mathf.Lerp(velocityFloor, 1, f);
            };

            midi.onWillNoteOff += control =>
            {
                int noteNumber = control.noteNumber;
                _keyStates[noteNumber].Active = false;
            };
        }

        protected override void ProcessMidiBuffer(ref MidiState[] buffer)
        {
            foreach (MidiState state in buffer)
            {
                for (int i = 0; i < MidiState.MidiKeyCount; i++)
                {
                    MidiState.Key[] keys = state.Keys;
                    keys[i].Active = _keyStates[i].Active;
                    keys[i].Amplitude = _keyStates[i].Velocity;
                    keys[i].SampleTime = _keyStates[i].SampleTime++;
                }
            }
        }
        
        private struct KeyState
        {
            public bool Active;
            public float Velocity;
            public int SampleTime;
        }
    }
}