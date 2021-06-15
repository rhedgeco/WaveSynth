namespace WaveSynth.WaveTriggers
{
    public abstract class WaveTriggerRoot : WaveTriggerOutput
    {
        public override WaveTriggerIndexer GetSampleTriggers()
        {
            return ProcessTriggerBuffer();
        }

        protected abstract WaveTriggerIndexer ProcessTriggerBuffer();
    }
}