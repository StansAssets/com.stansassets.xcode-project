using System.Collections;
using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;

namespace SA.iOS.XCode
{
    public class ISD_InfoPlistWindowTab : IMGUILayoutElement
    {
        static string s_NewPlistValueName = string.Empty;
        static string s_NewValueName = string.Empty;

        public override void OnGUI()
        {
            IMGUILayout.Header("PLIST VALUES");

            foreach (var plistKey in ISD_Settings.Instance.PlistVariables)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                DrawPlistVariable(plistKey, plistKey, ISD_Settings.Instance.PlistVariables);
                EditorGUILayout.EndVertical();

                if (!ISD_Settings.Instance.PlistVariables.Contains(plistKey)) return;
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("New Variable Name");
            s_NewPlistValueName = EditorGUILayout.TextField(s_NewPlistValueName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Add", GUILayout.Width(100)))
            {
                if (s_NewPlistValueName.Length > 0)
                {
                    var v = new ISD_PlistKey();
                    v.Name = s_NewPlistValueName;
                    ISD_API.SetInfoPlistKey(v);
                }

                s_NewPlistValueName = string.Empty;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        public static void DrawPlistVariable(ISD_PlistKey plistKey, object valuePointer, IList valueOrigin)
        {
            EditorGUILayout.BeginHorizontal();

            if (plistKey.Name.Length > 0)
                plistKey.IsOpen = EditorGUILayout.Foldout(plistKey.IsOpen, plistKey.Name + "   (" + plistKey.Type.ToString() + ")");
            else
                plistKey.IsOpen = EditorGUILayout.Foldout(plistKey.IsOpen, plistKey.Type.ToString());

            var itemWasRemoved = SortingButtons(valuePointer, valueOrigin);
            if (itemWasRemoved)
            {
                ISD_Settings.Instance.RemoveVariable(plistKey, valueOrigin);
                return;
            }

            EditorGUILayout.EndHorizontal();

            if (plistKey.IsOpen)
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Type");
                    if (plistKey.ChildrensIds.Count > 0)
                    {
                        GUI.enabled = false;
                        plistKey.Type = (ISD_PlistKeyType)EditorGUILayout.EnumPopup(plistKey.Type);
                    }
                    else
                    {
                        plistKey.Type = (ISD_PlistKeyType)EditorGUILayout.EnumPopup(plistKey.Type);
                    }

                    EditorGUILayout.EndHorizontal();

                    if (plistKey.Type == ISD_PlistKeyType.Array)
                    {
                        DrawArrayValues(plistKey);
                    }
                    else if (plistKey.Type == ISD_PlistKeyType.Dictionary)
                    {
                        DrawDictionaryValues(plistKey);
                    }
                    else if (plistKey.Type == ISD_PlistKeyType.Boolean)
                    {
                        plistKey.BooleanValue = IMGUILayout.ToggleFiled("Value", plistKey.BooleanValue, IMGUIToggleStyle.ToggleType.YesNo);
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Value");
                        switch (plistKey.Type)
                        {
                            case ISD_PlistKeyType.Integer:
                                plistKey.IntegerValue = EditorGUILayout.IntField(plistKey.IntegerValue);
                                break;
                            case ISD_PlistKeyType.String:
                                plistKey.StringValue = EditorGUILayout.TextField(plistKey.StringValue);
                                break;
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        public static void DrawArrayValues(ISD_PlistKey plistKey)
        {
            plistKey.IsListOpen = EditorGUILayout.Foldout(plistKey.IsListOpen, "Array Values (" + plistKey.ChildrensIds.Count + ")");

            if (plistKey.IsListOpen)
            {
                EditorGUI.indentLevel++;
                {
                    foreach (var uniqueKey in plistKey.ChildrensIds)
                    {
                        var v = ISD_Settings.Instance.getVariableById(uniqueKey);
                        DrawPlistVariable(v, uniqueKey, plistKey.ChildrensIds);

                        if (!plistKey.ChildrensIds.Contains(uniqueKey)) return;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Add Value", GUILayout.Width(100)))
                    {
                        var newVar = new ISD_PlistKey();

                        plistKey.AddChild(newVar);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                EditorGUI.indentLevel--;
            }
        }

        public static void DrawDictionaryValues(ISD_PlistKey plistKey)
        {
            plistKey.IsListOpen = EditorGUILayout.Foldout(plistKey.IsListOpen, "Dictionary Values");

            if (plistKey.IsListOpen)
            {
                EditorGUI.indentLevel++;
                {
                    foreach (var uniqueKey in plistKey.ChildrensIds)
                    {
                        var v = ISD_Settings.Instance.getVariableById(uniqueKey);
                        DrawPlistVariable(v, uniqueKey, plistKey.ChildrensIds);

                        if (!plistKey.ChildrensIds.Contains(uniqueKey)) return;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("New Key");
                    s_NewValueName = EditorGUILayout.TextField(s_NewValueName);

                    if (GUILayout.Button("Add", GUILayout.Width(50)))
                        if (s_NewValueName.Length > 0)
                        {
                            var v = new ISD_PlistKey();
                            v.Name = s_NewValueName;
                            plistKey.AddChild(v);
                        }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }

        public static bool SortingButtons(object currentObject, IList ObjectsList)
        {
            var objectIndex = ObjectsList.IndexOf(currentObject);
            if (objectIndex == 0) GUI.enabled = false;

            var up = GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(20));
            if (up)
            {
                var c = currentObject;
                ObjectsList[objectIndex] = ObjectsList[objectIndex - 1];
                ObjectsList[objectIndex - 1] = c;
            }

            if (objectIndex >= ObjectsList.Count - 1)
                GUI.enabled = false;
            else
                GUI.enabled = true;

            var down = GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(20));
            if (down)
            {
                var c = currentObject;
                ObjectsList[objectIndex] = ObjectsList[objectIndex + 1];
                ObjectsList[objectIndex + 1] = c;
            }

            GUI.enabled = true;
            var r = GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20));
            if (r) ObjectsList.Remove(currentObject);

            return r;
        }
    }
}
