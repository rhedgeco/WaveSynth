using UnityEngine;
using WaveSynth.FrequencyHandlers;
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
            for (int i = 0; i < _defaultWaveData.Length; i++)
            {
                NativeWaveSynth.WaveData data = new NativeWaveSynth.WaveData
                {
                    frequency = FrequencyTable.GetEqualTemperedFrequency(KeyboardKey.C, 7),
                    amplitude = 1
                };

                _defaultWaveData[i] = data;
            }

            _buffersCreated = true;
        }

        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() => WaveSettings.DetachSpeaker(this);
    }
}