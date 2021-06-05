using UnityEngine;

namespace WaveSynth.FrequencyHandlers
{
    public class FrequencyTable
    {
        private const float Root12 = 1.0594630943592952645618252949463f;
        public static float BaseAFrequency = 27.50f;

        public static float GetEqualTemperedFrequency(KeyboardKey key, uint octave)
        {
            int halfSteps = GetHalfStepsFromA(key);
            return GetEqualTemperedFrequency(halfSteps, octave);
        }
        
        public static float GetEqualTemperedFrequency(int halfSteps, uint octave)
        {
            return BaseAFrequency * Mathf.Pow(2, octave) * Mathf.Pow(Root12, halfSteps - 9);
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