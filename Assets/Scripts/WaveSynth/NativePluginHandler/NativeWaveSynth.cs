using System;
using System.Runtime.InteropServices;

namespace WaveSynth.NativePluginHandler
{
    public class NativeWaveSynth
    {
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

        [DllImport("native_wave_synth", EntryPoint = "merge_audio_buffers")]
        private static extern Response<int> MergeAudioBuffers(
            Array<float> mainBuffer, Array<float> addBuffer);

        [DllImport("native_wave_synth", EntryPoint = "generate_sin_wave")]
        private static extern Response<float> GenerateSinWave(
            Array<float> buffer, Array<WaveData> waveData,
            float phaseStart, int numChannels, int sampleRate);

        public static void MergeAudioBuffers(float[] mainBuffer, float[] addBuffer)
        {
            Array<float> m = new Array<float>(mainBuffer);
            Array<float> a = new Array<float>(addBuffer);
            Process(MergeAudioBuffers(m, a));
        }

        public static float GenerateSinWave(
            float[] buffer, WaveData[] waveData,
            float phaseStart, int numChannels, int sampleRate)
        {
            Array<float> b = new Array<float>(buffer);
            Array<WaveData> wd = new Array<WaveData>(waveData);
            return Process(GenerateSinWave(b, wd, phaseStart, numChannels, sampleRate));
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