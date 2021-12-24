using UnityEngine;

namespace WaveSynth
{
    public class WaveSpeaker : MonoBehaviour
    {
        private float lastPhase = 0;
        private bool buffersCreated = false;
        private float[] audioBuffer;
        private NativeWaveSynth.WaveData[] defaultWaveData;

        public float[] AudioBuffer => audioBuffer;

        public float[] ProcessChain()
        {
            if (!buffersCreated) CreateBuffers();
            lastPhase = NativeWaveSynth.GenerateSinWave(
                audioBuffer, defaultWaveData, lastPhase, WaveSettings.SampleRate);
            return audioBuffer;
        }

        private void CreateBuffers()
        {
            audioBuffer = new float[WaveSettings.BufferSize];
            defaultWaveData = new NativeWaveSynth.WaveData[WaveSettings.BufferSize];
            for (int i = 0; i < defaultWaveData.Length; i+=2)
            {
                NativeWaveSynth.WaveData data = new NativeWaveSynth.WaveData()
                {
                    frequency = 523.25f,
                    amplitude = 1
                };

                defaultWaveData[i + 0] = data;
                defaultWaveData[i + 1] = data;
            }

            buffersCreated = true;
        }
        
        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() => WaveSettings.DetachSpeaker(this);
    }
}