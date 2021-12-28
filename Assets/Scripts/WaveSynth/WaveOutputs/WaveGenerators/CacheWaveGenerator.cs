using System;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using WaveSynth.FrequencyHandlers;
using WaveSynth.NativePluginHandler;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class CacheWaveGenerator : WaveGenerator
    {
        [SerializeField] private string function;
        [SerializeField] [Range(1, 2048)] private int cacheSize = 1024;

        private float _lastPhase;
        private bool _buffersCreated;
        private NativeWaveSynth.WaveData[] _defaultWaveData;
        private float[] _cache;

        public override void ProcessBuffer(ref float[] buffer)
        {
            if (!_buffersCreated) CreateBuffers();
            _lastPhase = NativeWaveSynth.GenerateCacheWave(buffer, _defaultWaveData,
                _cache, _lastPhase, WaveSettings.Channels, WaveSettings.SampleRate);
        }

        private void CreateBuffers()
        {
            _cache = new float[cacheSize];
            _defaultWaveData = new NativeWaveSynth.WaveData[WaveSettings.ChannelBufferSize];
            for (int i = 0; i < _defaultWaveData.Length; i++)
            {
                NativeWaveSynth.WaveData data = new NativeWaveSynth.WaveData
                {
                    frequency = FrequencyTable.GetEqualTemperedFrequency(KeyboardKey.C, 6),
                    velocity = 1
                };

                _defaultWaveData[i] = data;
            }

            Argument a = new Argument("x", 0);
            Expression e = new Expression(function, a);
            for (int i = 0; i < cacheSize; i++)
            {
                a.setArgumentValue((i / (cacheSize - 1f)) * 2 - 1);
                float value = Convert.ToSingle(e.calculate());
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                _cache[i] = value;
            }

            _buffersCreated = true;
        }
    }
}