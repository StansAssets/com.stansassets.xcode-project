using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Foundation.Editor
{
    [Serializable]
    public class SA_GuiHorizontalSpace : IDisposable
    {

        public SA_GuiHorizontalSpace(int space) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUILayout.BeginVertical();

        }

        public void Dispose() {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}



