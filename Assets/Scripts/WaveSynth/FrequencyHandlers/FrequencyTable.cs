using UnityEngine;

namespace WaveSynth.FrequencyHandlers
{
    public class FrequencyTable
    {
        private const float Root12 = 1.0594630943592952645618252949463f;
        public static float BaseFrequency = 27.50f;

        public static float GetEqualTemperedFrequency(KeyboardKey key, uint octave)
        {
            int halfSteps = GetHalfStepsFromA(key);
            return BaseFrequency * Mathf.Pow(2, octave) * Mathf.Pow(Root12, halfSteps);
        }

        private static int GetHalfStepsFromA(KeyboardKey key)
        {
            return key switch
            {
                KeyboardKey.C => -9,
                KeyboardKey.CS => -8,
                KeyboardKey.D => -7,
                KeyboardKey.DS => -6,
                KeyboardKey.E => -5,
                KeyboardKey.F => -4,
                KeyboardKey.FS => -3,
                KeyboardKey.G => -2,
                KeyboardKey.GS => -1,
                KeyboardKey.A => 0,
                KeyboardKey.AS => 1,
                KeyboardKey.B => 2,
                _ => 0
            };
        }
    }
}