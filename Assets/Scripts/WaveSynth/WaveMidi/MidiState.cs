namespace WaveSynth.WaveMidi
{
    public class MidiState
    {
        public const int MidiKeyCount = 127;
        
        public readonly Key[] Keys = new Key[MidiKeyCount];
        
        public struct Key
        {
            public bool Active;
            public float Amplitude;
            public int SampleTime;
        }
    }
}