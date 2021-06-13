using UnityEngine;
using WaveSynth.WaveOutputs;

namespace WaveSynth
{
    public class WaveSpeaker : MonoBehaviour 
    {
        [SerializeField] private WaveOutput source;

        public float[] ProcessChain()
        {
            if (!source) return null;
            return source.GetWaveBuffer();
        }
        
        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() => WaveSettings.DetachSpeaker(this);
    }
}