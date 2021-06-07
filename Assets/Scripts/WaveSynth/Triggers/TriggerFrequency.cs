using WaveSynth.FrequencyHandlers;

namespace WaveSynth.Triggers
{
    public class TriggerFrequency
    {
        public float Frequency { get; }
        public float Amplitude { get; }
        public float Panning { get; }

        public TriggerFrequency(float frequency, float amplitude, float panning = 0)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Panning = panning;
        }

        public TriggerFrequency(int noteNumber, float amplitude, float panning = 0)
        {
            int halfStep = noteNumber % 12;
            uint octave = (uint) (noteNumber / 12);
            Frequency = FrequencyTable.GetEqualTemperedFrequency(halfStep, octave);
            Amplitude = amplitude;
            Panning = panning;
        }
    }
}