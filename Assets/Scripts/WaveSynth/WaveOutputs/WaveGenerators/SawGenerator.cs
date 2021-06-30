namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class SawGenerator : FunctionGenerator
    {
        protected override float SampleFunction(double phase)
        {
            return (float) (-1 + phase * 2);
        }
    }
}