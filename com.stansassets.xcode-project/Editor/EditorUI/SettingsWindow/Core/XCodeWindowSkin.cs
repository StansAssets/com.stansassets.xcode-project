using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;

namespace StansAssets.IOS.XCode
{
    static class XCodeWindowSkin
    {
        public static readonly string IconsPath = $"{XCodePackageEditor.RootPath}/Art/Icons/";
        public static readonly string CapabilityIconsPath = $"{XCodePackageEditor.RootPath}/Art/CapabilityIcon/";

        public static Texture2D WindowIcon
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return EditorAssetDatabase.GetTextureAtPath(IconsPath + "isd_pro.png");
                else
                    return EditorAssetDatabase.GetTextureAtPath(IconsPath + "isd.png");
            }
        }

        public static Texture2D GetIcon(string iconName)
        {
            return EditorAssetDatabase.GetTextureAtPath(IconsPath + iconName);
        }
    }
}
