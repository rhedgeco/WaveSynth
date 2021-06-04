using System.Collections.Generic;
using UnityEngine;
using WaveSynth.Extensions;

namespace WaveSynth
{
    public class GlobalAudioController : MonoBehaviour
    {
        public static float SampleRate { get; private set; } = 44100;
        public static int BufferSize => _bufferSize;
        public static int AccessID { get; private set; }

        private static int _bufferSize = 1024;
        private static int _numBuffers = 4;
        private static HashSet<AudioProducer> _outputs = new HashSet<AudioProducer>();

        [SerializeField] private float sampleRate = SampleRate;

        private AudioSource _source;

        private void Awake()
        {
            AudioSettings.GetDSPBufferSize(out _bufferSize, out _numBuffers);
            _bufferSize *= 2; // multiply the bufferSize by 2 stereo channels
            SampleRate = sampleRate;
            AccessID = 0;

            _outputs.Clear();
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.spatialBlend = 0;
            _source.reverbZoneMix = 0;
            _source.Play();
        }
        
        private void OnAudioFilterRead(float[] data, int channels)
        {
            float[] buffer;
            foreach (AudioProducer producer in _outputs)
            {
                buffer = producer.ProcessChain();
                if (buffer != null) data.AddList(buffer);
            }
            
            AccessID += 1; // switch access ID so generators can reset cache
        }

        public static void AttachProducer(AudioProducer producer) => _outputs.Add(producer);
        public static void DetachProducer(AudioProducer producer) => _outputs.Remove(producer);
    }
}