using UnityEngine;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public abstract class WaveGenerator : MonoBehaviour
    {
        public abstract void ProcessBuffer(ref float[] buffer);
    }
}