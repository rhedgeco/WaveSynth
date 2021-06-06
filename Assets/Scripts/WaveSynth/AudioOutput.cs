using UnityEngine;

namespace WaveSynth
{
    public abstract class AudioOutput : MonoBehaviour
    {
        private int _lastAccessID = -1;
        private float[] _cachedBuffer;

        private void Start()
        {
            _cachedBuffer = new float[GlobalAudioController.BufferSize];
        }

        public float[] GetBuffer()
        {
            if (GlobalAudioController.AccessID == _lastAccessID) 
                return _cachedBuffer;
            _lastAccessID = GlobalAudioController.AccessID;

            for (int i = 0; i < _cachedBuffer.Length; i++) _cachedBuffer[i] = 0;
            ProcessBuffer(ref _cachedBuffer);
            return _cachedBuffer;
        }

        protected abstract void ProcessBuffer(ref float[] buffer);
    }
}