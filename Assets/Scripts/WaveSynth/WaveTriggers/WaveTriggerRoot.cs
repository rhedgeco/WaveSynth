namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTriggerRoot : WaveTriggerOutput
    {
        private int _lastAccess = -1;
        private TriggerRange[] _triggerCache = new TriggerRange[0];
        
        public override TriggerRange[] GetTriggers()
        {
            if (ValidateCache()) return _triggerCache;
            ProcessTriggerBuffer(ref _triggerCache);
            return _triggerCache;
        }
        
        private bool ValidateCache()
        {
            if (_triggerCache.Length != WaveSettings.ChannelBufferSize)
                _triggerCache = CreateFreshTriggerRange();
            
            bool valid = _lastAccess == WaveSettings.AccessID;
            _lastAccess = WaveSettings.AccessID;
            return valid;
        }

        private TriggerRange[] CreateFreshTriggerRange()
        {
            TriggerRange[] range = new TriggerRange[WaveSettings.ChannelBufferSize];
            for (int i = 0; i < range.Length; i++) range[i] = new TriggerRange();
            return range;
        }

        protected abstract void ProcessTriggerBuffer(ref TriggerRange[] triggers);
    }
}