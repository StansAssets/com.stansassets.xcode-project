using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;

namespace StansAssets.IOS.XCode
{
    class XCodeEditorMenu : EditorWindow
    {
        const int k_Priority = 510;

        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + XCodePackage.DisplayName + "/Settings", false, k_Priority)]
        public static void OpenMainPage()
        {
            XCodeSettingsWindow.ShowTowardsInspector("XCode", XCodeWindowSkin.WindowIcon);
        }

        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + XCodePackage.DisplayName + "/Documentation", false, k_Priority)]
        public static void OpenDocumentation()
        {
            Application.OpenURL(XCodePackage.DocumentationUrl);
        }
    }
}
