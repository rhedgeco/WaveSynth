using Minis;
using UnityEngine;
using System.Collections.Generic;
using WaveSynth.FrequencyHandlers;

namespace WaveSynth.WaveTriggers
{
    public class MidiWaveTrigger : WaveTriggerRoot
    {
        [SerializeField] [Range(0, 1)] private float velocityFloor;

        private bool _notesChanged;
        private object _notesLock = new object();
        private WaveTriggerIndexer _indexer = new WaveTriggerIndexer();
        private HashSet<MidiDevice> _devices = new HashSet<MidiDevice>();
        private Dictionary<int, Trigger> _notes = new Dictionary<int, Trigger>();

        private void Update()
        {
            MidiDevice midi = MidiDevice.current;
            if (midi == null) return;
            if (_devices.Contains(midi)) return;
            _devices.Add(midi);

            midi.onWillNoteOn += (control, f) =>
            {
                int noteNumber = control.noteNumber;
                int halfStep = noteNumber % 12;
                uint octave = (uint) (noteNumber / 12);
                float frequency = FrequencyTable.GetEqualTemperedFrequency(halfStep, octave);
                float amplitude = Mathf.Lerp(velocityFloor, 1, f);
                lock (_notesLock)
                {
                    if (_notes.ContainsKey(noteNumber)) _notes.Remove(noteNumber);
                    _notes.Add(noteNumber, new Trigger(frequency, amplitude));
                    _notesChanged = true;
                }
            };

            midi.onWillNoteOff += control =>
            {
                int noteNumber = control.noteNumber;
                lock (_notesLock)
                {
                    if (_notes.ContainsKey(noteNumber)) _notes.Remove(noteNumber);
                    _notesChanged = true;
                }
            };
        }
        
        protected override WaveTriggerIndexer ProcessTriggerBuffer()
        {
            lock (_notesLock)
            {
                if (!_notesChanged) return _indexer;
                _notesChanged = false;
                
                _indexer.Clear();
                foreach (KeyValuePair<int,Trigger> pair in _notes)
                {
                    _indexer.AddTrigger(pair.Value, pair.Key);
                }

                return _indexer;
            }
        }
    }
}