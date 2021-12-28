using UnityEditor;
using UnityEngine;
using WaveSynth.Exceptions;

namespace WaveSynth.Editor
{
    [CustomEditor(typeof(WaveSpeaker))]
    public class WaveSpeakerEditor : UnityEditor.Editor
    {
        private WaveSpeaker speaker;
        private Material material;

        private void OnEnable()
        {
            EditorApplication.update += Repaint;
            speaker = target as WaveSpeaker;
            material = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            float[] buffer = speaker.AudioBuffer;
            if (buffer == null) return;
            GUILayout.Label($"Wave scope is the size of the buffer.");
            try
            {
                GUILayout.Label($"Channel buffer size: {WaveSettings.ChannelBufferSize}");
            }
            catch (WaveSettingsNotCreated) { /* do nothing */ }

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

                float amp = rect.height / 4;
                float baseYL = rect.height / 4;
                float baseYR = baseYL * 3;

                GL.Begin(GL.LINES);
                GL.Color(Color.grey);
                GL.Vertex3(0, baseYL, 0);
                GL.Vertex3(rect.width, baseYL, 0);
                GL.Vertex3(0, baseYR, 0);
                GL.Vertex3(rect.width, baseYR, 0);
                GL.End();
                
                GL.Begin(GL.LINE_STRIP);
                GL.Color(Color.green);
                float offset = rect.width / buffer.Length;
                for (int l = 0; l < buffer.Length; l += 2)
                {
                    GL.Vertex3(l * offset, baseYL - buffer[l] * amp, 0);
                }
                GL.End();
                
                GL.Begin(GL.LINE_STRIP);
                GL.Color(Color.green);
                for (int r = 0; r < buffer.Length; r += 2)
                {
                    GL.Vertex3(r * offset, baseYR - buffer[r] * amp, 0);
                }
                GL.End();

                GL.PopMatrix();
                GUI.EndClip();
            }
        }
    }
}