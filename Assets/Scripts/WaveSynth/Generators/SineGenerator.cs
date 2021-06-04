using UnityEngine;

namespace WaveSynth.Generators
{
    public class SineGenerator : FunctionGenerator
    {
        protected override float WaveFunction(float value)
        {
            return Mathf.Sin(value * 2 * Mathf.PI);
        }
    }
}