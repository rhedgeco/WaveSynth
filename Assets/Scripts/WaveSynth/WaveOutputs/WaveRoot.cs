namespace WaveSynth.WaveOutputs
{
    public abstract class WaveRoot : WaveOutput
    {
        private int _lastAccess = -1;
        private float[] _waveCache = new float[0];

        public override float[] GetWaveBuffer()
        {
            if (ValidateCache()) return _waveCache;
            ProcessWaveBuffer(ref _waveCache);
            return _waveCache;
        }

        private bool ValidateCache()
        {
            if (_waveCache.Length != WaveSettings.BufferSize)
                _waveCache = new float[WaveSettings.BufferSize];
            
            bool valid = _lastAccess == WaveSettings.AccessID;
            _lastAccess = WaveSettings.AccessID;
            return valid;
        }

        protected abstract void ProcessWaveBuffer(ref float[] buffer);
    }
}