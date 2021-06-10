namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class SawGenerator : FunctionGenerator
    {
        protected override float SampleFunction(float phase)
        {
            return -1 + phase * 2;
        }
    }
}