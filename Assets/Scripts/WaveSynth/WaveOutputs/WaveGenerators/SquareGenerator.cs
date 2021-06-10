namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class SquareGenerator : FunctionGenerator
    {
        protected override float SampleFunction(float phase)
        {
            return phase < 0.5f ? -1 : 1;
        }
    }
}