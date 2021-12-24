using System;
using System.Runtime.InteropServices;

namespace WaveSynth
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
        private static extern void MergeAudioBuffers(Array<float> mainBuffer, Array<float> addBuffer);

        [DllImport("native_wave_synth", EntryPoint = "generate_sin_wave")]
        private static extern float GenerateSinWave(
            Array<float> buffer, Array<WaveData> waveData,
            float phaseStart, int sampleRate);

        public static void MergeAudioBuffers(float[] mainBuffer, float[] addBuffer)
        {
            Array<float> m = new Array<float>(mainBuffer);
            Array<float> a = new Array<float>(addBuffer);
            MergeAudioBuffers(m, a);
        }

        public static float GenerateSinWave(float[] buffer, WaveData[] waveData, float phaseStart, int sampleRate)
        {
            Array<float> b = new Array<float>(buffer);
            Array<WaveData> wd = new Array<WaveData>(waveData);
            return GenerateSinWave(b, wd, phaseStart, sampleRate);
        }
    }
}