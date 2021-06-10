using UnityEngine;

namespace WaveSynth.WaveOutputs
{
    public abstract class WaveOutput : MonoBehaviour
    {
        private int _lastAccess = -1;
        private float[] _waveCache;

        public float[] GetWaveBuffer(int bufferLength)
        {
            if (_lastAccess == WaveSettings.AccessID) return _waveCache;
            if (_waveCache == null) _waveCache = new float[bufferLength];
            if (_waveCache.Length != bufferLength) _waveCache = new float[bufferLength];
            
            ProcessWaveBuffer(ref _waveCache);
            _lastAccess = WaveSettings.AccessID;
            return _waveCache;
        }

        protected abstract void ProcessWaveBuffer(ref float[] buffer);
    }
}