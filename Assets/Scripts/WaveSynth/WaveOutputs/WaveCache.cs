using Unity.Collections;

namespace WaveSynth.WaveOutputs
{
    public abstract class WaveCache : WaveOutput
    {
        private int _lastAccess = -1;
        private NativeArray<float> _waveCache = new NativeArray<float>(0, Allocator.Persistent);

        public override NativeArray<float> GetWaveBuffer()
        {
            if (ValidateCache()) return _waveCache;
            ProcessWaveBuffer(ref _waveCache);
            return _waveCache;
        }

        private bool ValidateCache()
        {
            if (_waveCache.Length != WaveSettings.BufferSize) {
                _waveCache.Dispose();
                _waveCache = new NativeArray<float>(WaveSettings.BufferSize, Allocator.Persistent);
            }
            
            bool valid = _lastAccess == WaveSettings.AccessID;
            _lastAccess = WaveSettings.AccessID;
            return valid;
        }

        protected abstract void ProcessWaveBuffer(ref NativeArray<float> buffer);
    }
}