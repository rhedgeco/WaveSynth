using UnityEngine;
using WaveSynth.NativePluginHandler;

namespace WaveSynth
{
    public class WaveSpeaker : MonoBehaviour
    {
        private float _lastPhase;
        private bool _buffersCreated;
        private float[] _audioBuffer;
        private NativeWaveSynth.WaveData[] _defaultWaveData;

        public float[] AudioBuffer => _audioBuffer;

        public float[] ProcessChain()
        {
            if (!_buffersCreated) CreateBuffers();
            _lastPhase = NativeWaveSynth.GenerateSinWave(_audioBuffer, _defaultWaveData,
                _lastPhase, WaveSettings.Channels, WaveSettings.SampleRate);
            return _audioBuffer;
        }

        private void CreateBuffers()
        {
            _audioBuffer = new float[WaveSettings.BufferSize];
            _defaultWaveData = new NativeWaveSynth.WaveData[WaveSettings.ChannelBufferSize];
            for (int i = 0; i < _defaultWaveData.Length; i += 2)
            {
                NativeWaveSynth.WaveData data = new NativeWaveSynth.WaveData()
                {
                    frequency = 523.25f,
                    amplitude = 1
                };

                _defaultWaveData[i + 0] = data;
                _defaultWaveData[i + 1] = data;
            }

            _buffersCreated = true;
        }

        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() => WaveSettings.DetachSpeaker(this);
    }
}