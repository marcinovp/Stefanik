using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EnliStandardAssets.SimpleMessaging
{
    [CustomEditor(typeof(Toast))]
    public class ToastInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Toast myScript = (Toast)target;
            if (GUILayout.Button("Test"))
            {
                myScript.ShowMessage("Test message", 2f);
            }

            DrawDefaultInspector();
        }
    }
}
