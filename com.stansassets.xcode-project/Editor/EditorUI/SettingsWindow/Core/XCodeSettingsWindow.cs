using UnityEditor;
using StansAssets.Plugins.Editor;

namespace StansAssets.IOS.XCode
{
    class XCodeSettingsWindow : IMGUISettingsWindow<XCodeSettingsWindow>
    {
        protected override void OnAwake()
        {
            SetPackageName(XCodePackage.PackageName);
            SetDocumentationUrl(XCodePackage.DocumentationUrl);

            AddMenuItem("GENERAL", CreateInstance<GeneralWindowTab>());
            AddMenuItem("COMPATIBILITIES", CreateInstance<CapabilitiesTab>());
            AddMenuItem("INFO", CreateInstance<InfoPlistWindowTab>());
            AddMenuItem("ABOUT", CreateInstance<IMGUIAboutTab>());
        }

        protected override void BeforeGUI()
        {
            EditorGUI.BeginChangeCheck();
        }

        protected override void AfterGUI()
        {
            if (EditorGUI.EndChangeCheck()) XCodeProjectSettings.Save();
        }
    }
}
