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
            // ISD_SettingsWindow.ShowTowardsInspector("IOS Deploy", ISD_Skin.WindowIcon);
        }

        [MenuItem(PluginsDevKitPackage.RootMenu + "/" + XCodePackage.DisplayName + "/Documentation", false, k_Priority)]
        public static void OpenDocumentation()
        {
            var url = "https://unionassets.com/ios-deploy/manual";
            Application.OpenURL(url);
        }
    }
}
