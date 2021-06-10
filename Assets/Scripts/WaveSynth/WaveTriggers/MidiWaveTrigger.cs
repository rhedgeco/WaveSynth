using System.Collections.Generic;
using Minis;
using UnityEngine;
using WaveSynth.FrequencyHandlers;

namespace WaveSynth.WaveTriggers
{
    public class MidiWaveTrigger : WaveTrigger
    {
        [SerializeField] [Range(0, 1)] private float velocityFloor;

        private bool _notesChanged = false;
        private object _notesLock = new object();
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        private HashSet<Trigger> _triggerCache = new HashSet<Trigger>();

        private Dictionary<KeyboardTrigger, Trigger> _notes =
            new Dictionary<KeyboardTrigger, Trigger>();

        private void Update()
        {
            MidiDevice midi = MidiDevice.current;
            if (midi == null) return;
            if (_devices.Contains(midi)) return;
            _devices.Add(midi);

            midi.onWillNoteOn += (control, f) =>
            {
                lock (_notesLock)
                {
                    KeyboardTrigger key = new KeyboardTrigger(midi, control.noteNumber);
                    if (_notes.ContainsKey(key)) _notes.Remove(key);
                    int noteNumber = key.NoteNumber;
                    int halfStep = noteNumber % 12;
                    uint octave = (uint) (noteNumber / 12);
                    float frequency = FrequencyTable.GetEqualTemperedFrequency(halfStep, octave);
                    float amplitude = Mathf.Lerp(velocityFloor, 1, f);
                    Trigger freq = new Trigger(frequency, amplitude);
                    _notes.Add(key, freq);
                    _notesChanged = true;
                }
            };

            midi.onWillNoteOff += control =>
            {
                lock (_notesLock)
                {
                    KeyboardTrigger key = new KeyboardTrigger(midi, control.noteNumber);
                    _notes.Remove(key);
                    _notesChanged = true;
                }
            };
        }

        public override HashSet<Trigger> GetTriggers(int bufferLength)
        {
            if (!_notesChanged) return _triggerCache;
            lock (_notesLock)
            {
                _triggerCache.Clear();
                foreach (Trigger trigger in _notes.Values)
                    _triggerCache.Add(trigger);
                _notesChanged = false;
            }

            return _triggerCache;
        }

        private struct KeyboardTrigger
        {
            public MidiDevice Device { get; }
            public int NoteNumber { get; }

            public KeyboardTrigger(MidiDevice device, int noteNumber)
            {
                Device = device;
                NoteNumber = noteNumber;
            }
        }
    }
}