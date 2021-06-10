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
            return source.GetWaveBuffer(GetHashCode());
        }
        
        private void Start() => WaveSettings.AttachProducer(this);
        private void OnDestroy() => WaveSettings.DetachProducer(this);
    }
}