using UnityEngine;

namespace WaveSynth
{
    public class AudioProducer : MonoBehaviour
    {
        [SerializeField] private AudioOutput source;

        public float[] ProcessChain()
        {
            if (!source) return null;
            return source.GetBuffer();
        }
        
        private void Start() => GlobalAudioController.AttachProducer(this);
        private void OnDestroy() => GlobalAudioController.DetachProducer(this);
    }
}