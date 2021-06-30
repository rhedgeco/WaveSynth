using Unity.Collections;
using UnityEngine;
using WaveSynth.WaveOutputs;

namespace WaveSynth
{
    public class WaveSpeaker : MonoBehaviour 
    {
        [SerializeField] private WaveOutput source;
        private NativeArray<float> _empty = new NativeArray<float>(0, Allocator.Persistent);

        public NativeArray<float> ProcessChain() {
            if (!source) return _empty;
            return source.GetWaveBuffer();
        }
        
        private void Start() => WaveSettings.AttachSpeaker(this);
        private void OnDestroy() {
            _empty.Dispose();
            WaveSettings.DetachSpeaker(this);
        }
    }
}