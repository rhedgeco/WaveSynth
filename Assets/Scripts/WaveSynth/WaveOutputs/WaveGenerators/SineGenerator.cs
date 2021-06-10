using UnityEngine;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class SineGenerator : FunctionGenerator
    {
        protected override float SampleFunction(float phase)
        {
            return Mathf.Sin(phase * 2 * Mathf.PI);
        }
    }
}