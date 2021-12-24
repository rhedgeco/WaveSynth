using UnityEngine;
using WaveSynth.WaveMidi;

namespace WaveSynth.FrequencyHandlers
{
    public class FrequencyTable
    {
        private const float Root12 = 1.0594630943592952645618252949463f;
        private static float _baseAFrequency = 27.50f;
        private static float _a4Frequency = 440;
        private static float[] _cache = new float[MidiState.MidiKeyCount];

        public static float A4Frequency
        {
            get => _a4Frequency;
            set
            {
                _a4Frequency = value;
                _baseAFrequency = value / 16;
                for (int i = 0; i < _cache.Length; i++) _cache[i] = 0;
            }
        }

        public static float GetEqualTemperedFrequency(KeyboardKey key, uint octave)
        {
            int halfSteps = GetHalfStepsFromA(key);
            return GetEqualTemperedFrequency(halfSteps, octave);
        }
        
        public static float GetEqualTemperedFrequency(int halfSteps, uint octave)
        {
            long cacheIndex = octave * 12 + halfSteps;
            if (_cache[cacheIndex] == 0) 
                _cache[cacheIndex] = _baseAFrequency * Mathf.Pow(2, octave) * Mathf.Pow(Root12, halfSteps - 9);
            return _cache[cacheIndex];
        }

        private static int GetHalfStepsFromA(KeyboardKey key)
        {
            return key switch
            {
                KeyboardKey.C => 0,
                KeyboardKey.CS => 1,
                KeyboardKey.D => 2,
                KeyboardKey.DS => 3,
                KeyboardKey.E => 4,
                KeyboardKey.F => 5,
                KeyboardKey.FS => 6,
                KeyboardKey.G => 7,
                KeyboardKey.GS => 8,
                KeyboardKey.A => 9,
                KeyboardKey.AS => 10,
                KeyboardKey.B => 11,
                _ => 0
            };
        }
    }
}