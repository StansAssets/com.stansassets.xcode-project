using StansAssets.IOS.XCode;
using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;

namespace SA.iOS.XCode
{
    public static class ISD_Skin
    {
        public static readonly string ICONS_PATH = $"{XCodePackageEditor.RootPath}/Art/Icons/";
        public static readonly string CAPABILITY_ICONS_PATH = $"{XCodePackageEditor.RootPath}/Art/CapabilityIcon/";

        public static Texture2D WindowIcon
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return EditorAssetDatabase.GetTextureAtPath(ICONS_PATH + "isd_pro.png");
                else
                    return EditorAssetDatabase.GetTextureAtPath(ICONS_PATH + "isd.png");
            }
        }

        public static Texture2D GetIcon(string iconName)
        {
            return EditorAssetDatabase.GetTextureAtPath(ICONS_PATH + iconName);
        }
    }
}
