using System;
using System.Collections.Generic;
using Minis;
using UnityEngine;
using WaveSynth.FrequencyHandlers;

namespace WaveSynth.Triggers
{
    public class MidiTrigger : AudioTrigger
    {
        private List<NoteInfo> _notes = new List<NoteInfo>();
        private List<float> _frequencies = new List<float>(256);
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();

        private void Update()
        {
            MidiDevice midi = MidiDevice.current;
            if (midi == null) return;
            if (_devices.Contains(midi)) return;
            _devices.Add(midi);

            midi.onWillNoteOn += (control, f) => _notes.Add(new NoteInfo(control.noteNumber));
            midi.onWillNoteOff += control => _notes.Remove(new NoteInfo(control.noteNumber));
        }

        public override List<float> GetActiveFrequencies()
        {
            _frequencies.Clear();

            NoteInfo note;
            for (int i = 0; i < _notes.Count; i++)
            {
                note = _notes[i];
                _frequencies.Add(FrequencyTable.GetEqualTemperedFrequency(note.HalfStep, note.Octave));
            }

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