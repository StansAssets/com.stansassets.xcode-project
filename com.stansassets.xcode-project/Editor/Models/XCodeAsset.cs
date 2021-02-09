using UnityEngine;
using System.IO;
using StansAssets.Foundation.Editor;
using UnityEditor;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// Additional XCode asset.
    /// </summary>
    [System.Serializable]
    public class XCodeAsset
    {
        public string XCodePath = string.Empty;
        public Object Asset = null;

        public string FileName
        {
            get
            {
                if (Asset == null)
                    return "No File";
                return Path.GetFileName(RelativeFilePath);
            }
        }

        public string RelativeFilePath
        {
            get
            {
                if (Asset == null)
                    return string.Empty;

                return AssetDatabase.GetAssetPath(Asset);
            }
        }

        public string AbsoluteFilePath
        {
            get
            {
                if (Asset == null)
                    return string.Empty;

                return AssetDatabaseUtility.GetAssetAbsolutePath(Asset);
            }
        }

        public string XCodeRelativePath => XCodePath + FileName;

        public bool IsDirectory
        {
            get
            {
                var attr = File.GetAttributes(RelativeFilePath);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return true;
                return false;
            }
        }
    }
}
