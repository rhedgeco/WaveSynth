namespace WaveSynth.WaveMidi
{
    public abstract class WaveMidiCache : WaveMidiOutput
    {
        private int _lastAccess = -1;
        private MidiState[] _midiCache = new MidiState[0];

        public override MidiState[] GetMidiBuffer()
        {
            if (ValidateCache()) return _midiCache;
            ProcessMidiBuffer(ref _midiCache);
            return _midiCache;
        }

        private bool ValidateCache()
        {
            if (_midiCache.Length != WaveSettings.ChannelBufferSize) RegenMidiCache();
            bool valid = _lastAccess == WaveSettings.AccessID;
            _lastAccess = WaveSettings.AccessID;
            return valid;
        }

        private void RegenMidiCache()
        {
            _midiCache = new MidiState[WaveSettings.ChannelBufferSize];
            for (int i = 0; i < _midiCache.Length; i++) _midiCache[i] = new MidiState();
        }

        protected abstract void ProcessMidiBuffer(ref MidiState[] buffer);
    }
}