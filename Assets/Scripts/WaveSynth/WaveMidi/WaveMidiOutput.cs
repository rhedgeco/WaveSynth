using UnityEngine;

namespace WaveSynth.WaveMidi
{
    public abstract class WaveMidiOutput : MonoBehaviour
    {
        public abstract MidiState[] GetMidiBuffer();
    }
}