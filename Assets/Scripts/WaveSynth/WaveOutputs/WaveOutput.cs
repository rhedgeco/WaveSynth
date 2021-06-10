using System.Collections.Generic;
using UnityEngine;

namespace WaveSynth.WaveOutputs
{
    public abstract class WaveOutput : MonoBehaviour
    {
        private int _lastAccess = -1;
        private Queue<float[]> _availableCaches = new Queue<float[]>();
        private Dictionary<int, float[]> _waveCaches = new Dictionary<int, float[]>();
        private float[] _waveCache;

        public float[] GetWaveBuffer(int accessHash)
        {
            float[] cache;
            if (_lastAccess != WaveSettings.AccessID)
            {
                _lastAccess = WaveSettings.AccessID;
                CollectWaveCaches();
                cache = AddAccessCache(accessHash);
                ProcessWaveBuffer(ref cache);
                return cache;
            }

            if (_waveCaches.ContainsKey(accessHash))
            {
                cache = _waveCaches[accessHash];
                ProcessWaveBuffer(ref cache);
                return cache;
            }

            cache = AddAccessCache(accessHash);
            ProcessWaveBuffer(ref cache);
            return cache;
        }

        private void CollectWaveCaches()
        {
            foreach (float[] cache in _waveCaches.Values)
            {
                _availableCaches.Enqueue(cache);
            }

            _waveCaches.Clear();
        }

        private float[] AddAccessCache(int accessHash)
        {
            float[] cache;
            if (_availableCaches.Count > 0) cache = _availableCaches.Dequeue();
            else cache = new float[WaveSettings.BufferSize];
            _waveCaches.Add(accessHash, cache);
            return cache;
        }

        protected abstract void ProcessWaveBuffer(ref float[] buffer);
    }
}