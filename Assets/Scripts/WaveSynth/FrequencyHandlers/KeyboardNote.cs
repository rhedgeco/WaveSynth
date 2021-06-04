using System;

namespace WaveSynth.FrequencyHandlers
{
    [Serializable]
    public struct KeyboardNote
    {
        public uint octave;
        public KeyboardKey key;
    }
}