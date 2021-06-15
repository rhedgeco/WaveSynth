using System.Collections.Generic;

namespace WaveSynth.WaveTriggers
{
    public class WaveTriggerIndexer
    {
        private WaveTriggerOutput.Trigger[] _triggers =
            new WaveTriggerOutput.Trigger[WaveSettings.MaxTriggerCount];
        private List<int> _indices = new List<int>();

        public int Count => _indices.Count;

        public void AddTrigger(WaveTriggerOutput.Trigger trigger, int rawIndex)
        {
            _indices.Remove(rawIndex);
            _indices.Add(rawIndex);
            _triggers[rawIndex] = trigger;
        }

        public void RemoveTrigger(int index)
        {
            _indices.Remove(index);
        }

        public int GetRawIndex(int relativeIndex)
        {
            return _indices[relativeIndex];
        }

        public void Clear()
        {
            _indices.Clear();
        }

        public WaveTriggerOutput.Trigger GetTrigger(int rawIndex)
        {
            return _triggers[rawIndex];
        }
    }
}