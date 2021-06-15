using System;
using UnityEngine;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class SineGenerator : FunctionGenerator
    {
        protected override float SampleFunction(double phase)
        {
            return (float) Math.Sin(phase * 2 * Mathf.PI);
        }
    }
}