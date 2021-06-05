using System.Collections.Generic;
using Minis;
using UnityEngine;

namespace WaveSynth.Triggers
{
    public class MidiTrigger : AudioTrigger
    {
        private List<NoteInfo> _notes = new List<NoteInfo>();
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        private List<TriggerFrequency> _frequencies = new List<TriggerFrequency>();

        private void Update()
        {
            MidiDevice midi = MidiDevice.current;
            if (midi == null) return;
            if (_devices.Contains(midi)) return;
            _devices.Add(midi);

            midi.onWillNoteOn += (control, f) => _notes.Add(new NoteInfo(control.noteNumber));
            midi.onWillNoteOff += control => _notes.Remove(new NoteInfo(control.noteNumber));
        }

        protected override List<TriggerFrequency> ProcessFrequencies()
        {
            // TODO: Process midi notes
            return _frequencies;
        }

        private struct NoteInfo
        {
            public int HalfStep { get; }
            public uint Octave { get; }

            public NoteInfo(int noteNumber)
            {
                HalfStep = Mathf.Clamp(noteNumber - 12, 0, 88) % 12;
                Octave = (uint) (noteNumber / 12);
            }
        }
    }
}