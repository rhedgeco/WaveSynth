namespace WaveSynth.Generators
{
    public class SquareGenerator : FunctionGenerator
    {
        protected override float WaveFunction(float value)
        {
            return value < 0.5f ? -1 : 1;
        }
    }
}