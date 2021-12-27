using System;
using System.Runtime.InteropServices;
using org.mariuszgromada.math.mxparser.parsertokens;
using UnityEngine;

namespace WaveSynth.NativePluginHandler
{
    public static class NativeWaveSynth
    {
        private static NativePlugin _plugin = new NativePlugin("native_wave_synth");

        [StructLayout(LayoutKind.Sequential)]
        public struct WaveData
        {
            public float frequency;
            public float amplitude;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Response<T>
        {
            public T data;
            public bool error;
            public string message;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Array<T>
        {
            public T[] array;
            public UIntPtr length;

            public Array(T[] array) : this()
            {
                this.array = array;
                length = (UIntPtr) array.Length;
            }
        }
        
        private static MergeAudioBuffersNative _mergeAudioBuffers;
        private delegate Response<int> MergeAudioBuffersNative(
            Array<float> mainBuffer, Array<float> addBuffer);
        
        private static GenerateSinWaveNative _generateSinWave;
        private delegate Response<float> GenerateSinWaveNative(
            Array<float> buffer, Array<WaveData> waveData,
            float phaseStart, int numChannels, int sampleRate);

        private static GenerateCacheWaveNative _generateCacheWave;
        private delegate Response<float> GenerateCacheWaveNative(
            Array<float> buffer, Array<WaveData> waveData, Array<float> cache,
        float phaseStart, int numChannels, int sampleRate);

        public static void MergeAudioBuffers(float[] mainBuffer, float[] addBuffer)
        {
            EnsurePluginLoaded();
            Array<float> m = new Array<float>(mainBuffer);
            Array<float> a = new Array<float>(addBuffer);
            Process(_mergeAudioBuffers(m, a));
        }

        public static float GenerateSinWave(
            float[] buffer, WaveData[] waveData,
            float phaseStart, int numChannels, int sampleRate)
        {
            EnsurePluginLoaded();
            Array<float> b = new Array<float>(buffer);
            Array<WaveData> wd = new Array<WaveData>(waveData);
            return Process(_generateSinWave(b, wd, phaseStart, numChannels, sampleRate));
        }
        
        public static float GenerateCacheWave(
            float[] buffer, WaveData[] waveData, float[] cache,
            float phaseStart, int numChannels, int sampleRate)
        {
            EnsurePluginLoaded();
            Array<float> b = new Array<float>(buffer);
            Array<WaveData> wd = new Array<WaveData>(waveData);
            Array<float> c = new Array<float>(cache);
            return Process(_generateCacheWave(b, wd, c, phaseStart, numChannels, sampleRate));
        }

        public static void LoadPlugin()
        {
            _plugin.LoadPlugin();
            _mergeAudioBuffers = _plugin.ExtractFunction<MergeAudioBuffersNative>("merge_audio_buffers");
            _generateSinWave = _plugin.ExtractFunction<GenerateSinWaveNative>("generate_sin_wave");
            _generateCacheWave = _plugin.ExtractFunction<GenerateCacheWaveNative>("generate_cache_wave");
        }

        public static void UnloadPlugin()
        {
            _plugin.UnloadPlugin();
        }

        private static void EnsurePluginLoaded()
        {
            if (!_plugin.Loaded)
                throw new Exception("Plugin is not loaded.");
        }

        private static T Process<T>(Response<T> response)
        {
            if (response.error)
            {
                throw new NativeException($"[NATIVE PLUGIN ERROR]: {response.message}");
            }

            return response.data;
        }
    }
}