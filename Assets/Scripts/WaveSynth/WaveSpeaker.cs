using UnityEngine;
using WaveSynth.WaveOutputs.WaveGenerators;

namespace WaveSynth
{
    public class WaveSpeaker : MonoBehaviour
    {
        [SerializeField] private WaveGenerator generator;
        
        private float _lastPhase;
        private bool _buffersCreated;
        private float[] _audioBuffer;

        public float[] AudioBuffer => _audioBuffer;

        public float[] ProcessChain()
        {
            if (!_buffersCreated) CreateBuffers();
            if (generator != null) generator.ProcessBuffer(ref _audioBuffer);
            return _audioBuffer;
        }

        private void CreateBuffers()
        {
            _audioBuffer = new float[WaveSettings.BufferSize];
            _buffersCreated = true;
        }

        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() => WaveSettings.DetachSpeaker(this);
    }
}