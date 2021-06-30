using System;

namespace WaveSynth.Extensions
{
    public static class FloatArrayExtensions
    {
        public static void AddList(this float[] a, float[] array)
        {
            int cap = Math.Min(a.Length, array.Length);
            for (int i = 0; i < cap; i++)
            {
                a[i] += array[i];
                if (a[i] > 1) a[i] = 1;
                if (a[i] < -1) a[i] = -1;
            }
        }
    }
}