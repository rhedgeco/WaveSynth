using UnityEngine;

namespace WaveSynth.WaveOutputs
{
    public abstract class WaveOutput : MonoBehaviour
    {
        public abstract float[] GetWaveBuffer();
    }
}