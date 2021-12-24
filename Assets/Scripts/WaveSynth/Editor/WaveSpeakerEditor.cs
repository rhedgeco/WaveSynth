using UnityEditor;
using UnityEngine;

namespace WaveSynth.Editor
{
    [CustomEditor(typeof(WaveSpeaker))]
    public class WaveSpeakerEditor : UnityEditor.Editor
    {
        private WaveSpeaker speaker;
        private Material material;

        private void OnEnable()
        {
            speaker = target as WaveSpeaker;
            material = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        public override void OnInspectorGUI()
        {
            float[] buffer = speaker.AudioBuffer;
            if (buffer == null) return;

            Rect rect = GUILayoutUtility.GetRect(10, 10000, 200, 200);
            if (Event.current.type == EventType.Repaint)
            {
                GUI.BeginClip(rect);
                GL.PushMatrix();

                GL.Clear(true, false, Color.black);
                material.SetPass(0);

                GL.Begin(GL.QUADS);
                GL.Color(Color.black);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(rect.width, 0, 0);
                GL.Vertex3(rect.width, rect.height, 0);
                GL.Vertex3(0, rect.height, 0);
                GL.End();

                GL.Begin(GL.LINES);

                float amp = rect.height / 2;
                float baseY = rect.height / 2;
                
                GL.Color(Color.grey);
                GL.Vertex3(0,baseY,0);
                GL.Vertex3(rect.width, baseY, 0);

                GL.Color(Color.green);
                float offset = rect.width / buffer.Length;
                for (int i = 0; i < buffer.Length - 4; i+=2)
                {
                    GL.Vertex3(i * offset, baseY + buffer[i + 0] * amp, 0);
                    GL.Vertex3(i * offset, baseY + buffer[i + 2] * amp, 0);
                }
                
                GL.End();

                GL.PopMatrix();
                GUI.EndClip();
            }
        }
    }
}