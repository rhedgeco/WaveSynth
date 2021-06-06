using WaveSynth.FrequencyHandlers;

namespace WaveSynth.Triggers
{
    public class TriggerFrequency
    {
        public float Frequency { get; }
        public float Amplitude { get; }

        public TriggerFrequency(float frequency, float amplitude)
        {
            Frequency = frequency;
            Amplitude = amplitude;
        }

        public TriggerFrequency(int noteNumber, float amplitude)
        {
            int halfStep = noteNumber % 12;
            uint octave = (uint) (noteNumber / 12);
            Frequency = FrequencyTable.GetEqualTemperedFrequency(halfStep, octave);
            Amplitude = amplitude;
        }
    }
}