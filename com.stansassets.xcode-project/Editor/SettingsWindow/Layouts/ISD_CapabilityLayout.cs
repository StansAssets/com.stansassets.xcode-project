using System;
using SA.Foundation.Editor;
using UnityEngine;
using StansAssets.Plugins.Editor;
using UnityEditor;

namespace SA.iOS.XCode
{
    public class ISD_CapabilityLayout : SA_CollapsableWindowBlockLayout
    {
        public delegate ISD_CapabilitySettings.Capability GetCapability();

        readonly IMGUIHyperLabel m_stateLabel;

        //private ISD_CapabilitySettings.Capability m_capability;

        readonly GUIContent m_off;
        readonly GUIContent m_on;

        readonly Color m_normalColor;
        readonly GetCapability m_getCapability;

        public ISD_CapabilityLayout(string name, string image, GetCapability getCapability, Action onGUI)
            : base(new GUIContent(name, EditorAssetDatabase.GetTextureAtPath(ISD_Skin.CAPABILITY_ICONS_PATH + image)), onGUI)
        {
            //m_capability = capability;

            m_getCapability = getCapability;

            m_on = new GUIContent("ON");
            m_off = new GUIContent("OFF");
            m_normalColor = EditorStyles.boldLabel.normal.textColor;
            m_stateLabel = new IMGUIHyperLabel(m_on, EditorStyles.boldLabel);
            m_stateLabel.SetMouseOverColor(SettingsWindowStyles.SelectedElementColor);
        }

        protected override void OnAfterHeaderGUI()
        {
            var capability = m_getCapability();

            if (capability.Enabled)
            {
                m_stateLabel.SetContent(m_on);
                m_stateLabel.SetColor(SettingsWindowStyles.SelectedElementColor);
            }
            else
            {
                m_stateLabel.SetContent(m_off);
                m_stateLabel.SetColor(m_normalColor);
            }

            GUILayout.FlexibleSpace();
            var click = m_stateLabel.Draw(GUILayout.Width(40));
            if (click) capability.Enabled = !capability.Enabled;
        }
    }
}
