using System.Collections.Generic;
using Minis;

namespace WaveSynth.Triggers
{
    public class MidiTrigger : AudioTrigger
    {
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        private List<TriggerFrequency> _frequencies = new List<TriggerFrequency>();
        private object _notesLock = new object();

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
                }
            };

            midi.onWillNoteOff += control =>
            {
                lock (_notesLock)
                {
                    KeyboardTrigger key = new KeyboardTrigger(midi, control.noteNumber);
                    _notes.Remove(key);
                }
            };
        }

        protected override List<TriggerFrequency> ProcessFrequencies(int bufferLength)
        {
            _frequencies.Clear();
            lock (_notesLock)
            {
                foreach (TriggerFrequency frequency in _notes.Values)
                {
                    _frequencies.Add(frequency);
                }
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