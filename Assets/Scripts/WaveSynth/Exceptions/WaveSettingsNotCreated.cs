using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace WaveSynth.Exceptions
{
    public class WaveSettingsNotCreated : Exception
    {
        public WaveSettingsNotCreated()
        {
        }

        protected WaveSettingsNotCreated([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public WaveSettingsNotCreated(string message) : base(message)
        {
        }

        public WaveSettingsNotCreated(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}