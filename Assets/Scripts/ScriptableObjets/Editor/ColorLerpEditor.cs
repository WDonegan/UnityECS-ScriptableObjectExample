using UnityEngine;
using UnityEditor;
using SOExample.DataObjects;

namespace SOExample.DataObjects.EditorData
{
    [CustomEditor(typeof(ColorLerp))]
    public class ColorLerpEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ColorLerp c = target as ColorLerp;

            GUI.enabled = true;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Initialize"))
                c.Initialize();

            if (GUILayout.Button("Clear"))
                c.Clear();

            GUILayout.EndHorizontal();
        }
    }
}
