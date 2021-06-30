using Unity.Collections;
using UnityEngine;

namespace WaveSynth.WaveOutputs
{
    public abstract class WaveOutput : MonoBehaviour
    {
        public abstract NativeArray<float> GetWaveBuffer();
    }
}