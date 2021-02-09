using System;
using UnityEngine;
using StansAssets.Plugins.Editor;
using UnityEditor;

namespace StansAssets.IOS.XCode
{
    class CapabilityLayout : IMGUICollapsableWindowBlockLayout
    {
        public delegate XCodeCapabilitySettings.Capability GetCapability();

        readonly IMGUIHyperLabel m_StateLabel;

        //private ISD_CapabilitySettings.Capability m_capability;

        readonly GUIContent m_Off;
        readonly GUIContent m_ON;

        readonly Color m_NormalColor;
        readonly GetCapability m_GETCapability;

        public CapabilityLayout(string name, string image, GetCapability getCapability, Action onGUI)
            : base(new GUIContent(name, EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.CapabilityIconsPath + image)), onGUI)
        {
            m_GETCapability = getCapability;

            m_ON = new GUIContent("ON");
            m_Off = new GUIContent("OFF");
            m_NormalColor = EditorStyles.boldLabel.normal.textColor;
            m_StateLabel = new IMGUIHyperLabel(m_ON, EditorStyles.boldLabel);
            m_StateLabel.SetMouseOverColor(SettingsWindowStyles.SelectedElementColor);
        }

        protected override void OnAfterHeaderGUI()
        {
            var capability = m_GETCapability();

            if (capability.Enabled)
            {
                m_StateLabel.SetContent(m_ON);
                m_StateLabel.SetColor(SettingsWindowStyles.SelectedElementColor);
            }
            else
            {
                m_StateLabel.SetContent(m_Off);
                m_StateLabel.SetColor(m_NormalColor);
            }

            GUILayout.FlexibleSpace();
            var click = m_StateLabel.Draw(GUILayout.Width(40));
            if (click) capability.Enabled = !capability.Enabled;
        }
    }
}
