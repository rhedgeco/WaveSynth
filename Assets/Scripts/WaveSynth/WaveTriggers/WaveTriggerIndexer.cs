using System.Collections.Generic;

namespace WaveSynth.WaveTriggers
{
    public class WaveTriggerIndexer
    {
        private WaveTriggerOutput.Trigger[] _triggers =
            new WaveTriggerOutput.Trigger[WaveSettings.MaxTriggerCount];

        private List<int> _indices = new List<int>();

        public int Count => _indices.Count;

        public WaveTriggerIndexer()
        {
            for (int i = 0; i < _triggers.Length; i++)
            {
                _triggers[i] = new WaveTriggerOutput.Trigger("", 0, 0);
            }
        }

        public void AddTrigger(WaveTriggerOutput.Trigger trigger, int rawIndex)
        {
            _indices.Remove(rawIndex);
            _indices.Add(rawIndex);
            _triggers[rawIndex].UniqueId = trigger.UniqueId;
            _triggers[rawIndex].Frequency = trigger.Frequency;
            _triggers[rawIndex].Amplitude = trigger.Amplitude;
        }
        
        public void AddTrigger(string uniqueId, float frequency, float amplitude, int rawIndex)
        {
            _indices.Remove(rawIndex);
            _indices.Add(rawIndex);
            _triggers[rawIndex].UniqueId = uniqueId;
            _triggers[rawIndex].Frequency = frequency;
            _triggers[rawIndex].Amplitude = amplitude;
        }

        public void RemoveTrigger(int index)
        {
            _indices.Remove(index);
            _triggers[index].UniqueId = "";
        }

        public int GetRawIndex(int relativeIndex)
        {
            return _indices[relativeIndex];
        }

        public void Mirror(WaveTriggerIndexer indexer)
        {
            _indices.Clear();
            _indices.AddRange(indexer._indices);
            for (int i = 0; i < _indices.Count; i++)
            {
                int rawIndex = _indices[i];
                WaveTriggerOutput.Trigger other = indexer._triggers[rawIndex];
                if (_triggers[rawIndex] == null)
                    _triggers[rawIndex] = new WaveTriggerOutput.Trigger(
                        other.UniqueId, other.Frequency, other.Amplitude);
                else if (_triggers[rawIndex].UniqueId != other.UniqueId)
                    _triggers[rawIndex] = new WaveTriggerOutput.Trigger(
                        other.UniqueId, other.Frequency, other.Amplitude);
                else _triggers[rawIndex].Set(other.Frequency, other.Amplitude);
            }
        }

        public void Clear()
        {
            _indices.Clear();
            for (int i = 0; i < _triggers.Length; i++)
            {
                if (_triggers[i] == null) continue;
                _triggers[i].UniqueId = "";
            }
        }

        public WaveTriggerOutput.Trigger GetTrigger(int rawIndex)
        {
            return _triggers[rawIndex];
        }
    }
}