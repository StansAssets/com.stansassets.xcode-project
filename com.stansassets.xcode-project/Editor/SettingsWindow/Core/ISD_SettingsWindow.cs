using UnityEditor;
using StansAssets.IOS.XCode;
using StansAssets.Plugins.Editor;

namespace SA.iOS.XCode
{
    public class ISD_SettingsWindow : IMGUISettingsWindow<ISD_SettingsWindow>
    {
        protected override void OnAwake()
        {
            SetPackageName(XCodePackage.PackageName);
            SetDocumentationUrl(ISD_Settings.DOCUMENTATION_URL);

            AddMenuItem("GENERAL", CreateInstance<ISD_GeneralWindowTab>());
            AddMenuItem("COMPATIBILITIES", CreateInstance<ISD_CapabilitiesTab>());
            AddMenuItem("INFO", CreateInstance<ISD_InfoPlistWindowTab>());
            AddMenuItem("ABOUT", CreateInstance<IMGUIAboutTab>());
        }

        protected override void BeforeGUI()
        {
            EditorGUI.BeginChangeCheck();
        }

        protected override void AfterGUI()
        {
            if (EditorGUI.EndChangeCheck()) ISD_Settings.Save();
        }
    }
}
