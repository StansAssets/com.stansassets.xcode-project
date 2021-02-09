using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using StansAssets.Plugins.Editor;

namespace SA.Foundation.Editor
{
    [Serializable]
    public class SA_CollapsableWindowBlockLayout
    {
        Action m_onGUI;
        IMGUIHyperLabel m_header;
        IMGUIHyperLabel m_arrrow;

        AnimBool m_showExtraFields = new AnimBool(false);

        GUIContent m_collapsedContent;
        GUIContent m_expandedContent;

        public SA_CollapsableWindowBlockLayout(GUIContent content, Action onGUI)
        {
            if (content.image != null) content.text = " " + content.text;

            m_onGUI = onGUI;
            m_header = new IMGUIHyperLabel(content, SettingsWindowStyles.ServiceBlockHeader);
            m_header.SetMouseOverColor(SettingsWindowStyles.SelectedElementColor);

            var rightArrow = PluginsEditorSkin.GetGenericIcon("arrow_right.png");
            var arrow_down = PluginsEditorSkin.GetGenericIcon("arrow_down.png");
            m_collapsedContent = new GUIContent(rightArrow);
            m_expandedContent = new GUIContent(arrow_down);

            m_arrrow = new IMGUIHyperLabel(m_collapsedContent, SettingsWindowStyles.ServiceBlockHeader);
        }

        protected virtual void OnAfterHeaderGUI() { }

        public void OnGUI()
        {
            GUILayout.Space(5);
            using (new IMGUIBeginHorizontal())
            {
                GUILayout.Space(10);

                var content = m_collapsedContent;
                if (m_showExtraFields.target) content = m_expandedContent;

                m_arrrow.SetContent(content);
                var arClick = m_arrrow.Draw(GUILayout.Width(20));
                GUILayout.Space(-5);

                var headerWidth = m_header.CalcSize().x;
                var click = m_header.Draw(GUILayout.Width(headerWidth));
                if (click || arClick) m_showExtraFields.target = !m_showExtraFields.target;

                OnAfterHeaderGUI();
            }

            using (new SA_GuiHorizontalSpace(10))
            {
                if (EditorGUILayout.BeginFadeGroup(m_showExtraFields.faded))
                {
                    GUILayout.Space(5);
                    m_onGUI.Invoke();
                    GUILayout.Space(5);
                }

                EditorGUILayout.EndFadeGroup();
            }

            GUILayout.Space(5);
            EditorGUILayout.BeginVertical(SettingsWindowStyles.SeparationStyle);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }
    }
}
