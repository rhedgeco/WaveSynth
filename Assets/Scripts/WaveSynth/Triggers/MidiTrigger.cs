using System.Collections.Generic;
using Minis;

namespace WaveSynth.Triggers
{
    public class MidiTrigger : AudioTrigger
    {
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        private List<TriggerFrequency> _frequencies = new List<TriggerFrequency>();
        private object _notesLock = new object();
        private bool _notesChanged = false;

        private Dictionary<KeyboardTrigger, TriggerFrequency> _notes =
            new Dictionary<KeyboardTrigger, TriggerFrequency>();

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
                    TriggerFrequency freq = new TriggerFrequency(control.noteNumber, f);
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

        protected override List<TriggerFrequency> ProcessFrequencies(int bufferLength)
        {
            if (!_notesChanged) return _frequencies;
            _frequencies.Clear();
            lock (_notesLock)
            {
                foreach (TriggerFrequency frequency in _notes.Values)
                {
                    _frequencies.Add(frequency);
                }

                _notesChanged = false;
            }

            return _frequencies;
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