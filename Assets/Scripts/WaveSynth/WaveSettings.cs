using System.Collections.Generic;
using UnityEngine;
using WaveSynth.Exceptions;
using WaveSynth.Extensions;

namespace WaveSynth
{
    public class WaveSettings : MonoBehaviour
    {
        public static int AccessID { get; private set; }
        public static int SampleRate => GetSampleRate();
        public static int BufferSize => ChannelBufferSize * Channels;
        public static int ChannelBufferSize => GetBufferSize();
        public static int Channels => 2;

        private static int _sampleRate = -1;
        private static int _bufferSize = -1;
        private static int _numBuffer = -1;

        private static object _speakerLock = new object();
        private static HashSet<WaveSpeaker> _speakers = new HashSet<WaveSpeaker>();

        private AudioSource _source;

        private void Awake()
        {
            AccessID = 0;
            AudioSettings.GetDSPBufferSize(out _bufferSize, out _numBuffer);
            _sampleRate = AudioSettings.GetConfiguration().sampleRate;
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.spatialBlend = 0;
            _source.reverbZoneMix = 0;
            _source.Play();
            print($"Buffer Size\t: {BufferSize}\n" +
                  $"Sample Rate\t: {SampleRate}");

            lock (_speakerLock)
            {
                _speakers.Clear();
            }
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            lock (_speakerLock)
            {
                foreach (WaveSpeaker producer in _speakers)
                    data.AddList(producer.ProcessChain());
            }

            AccessID += 1; // switch access ID so generators can reset cache
        }

        private static int GetSampleRate()
        {
            if (_sampleRate < 0) throw new WaveSettingsNotCreated();
            return _sampleRate;
        }

        private static int GetBufferSize()
        {
            if (_bufferSize < 0) throw new WaveSettingsNotCreated();
            return _bufferSize;
        }

        public static void AttachSpeaker(WaveSpeaker speaker)
        {
            lock (_speakerLock)
            {
                _speakers.Add(speaker);
            }
        }

        public static void DetachSpeaker(WaveSpeaker speaker)
        {
            lock (_speakerLock)
            {
                _speakers.Remove(speaker);
            }
        }
    }
}