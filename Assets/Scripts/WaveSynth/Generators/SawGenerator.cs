namespace WaveSynth.Generators
{
    public class SawGenerator : FunctionGenerator
    {
        protected override float WaveFunction(float value)
        {
            return -1 + value * 2;
        }
    }
}