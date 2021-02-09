using UnityEngine;
using System.IO;
using SA.Foundation.UtilitiesEditor;

namespace SA.iOS.XCode
{
    [System.Serializable]
    public class ISD_AssetFile
    {
        public string XCodePath = string.Empty;
        public Object Asset = null;

        public string FileName
        {
            get
            {
                if (Asset == null) return "No File";
                return Path.GetFileName(RelativeFilePath);
            }
        }

        public string RelativeFilePath
        {
            get
            {
                if (Asset == null) return string.Empty;
                return SA_AssetDatabase.GetAssetPath(Asset);
            }
        }

        public string AbsoluteFilePath
        {
            get
            {
                if (Asset == null) return string.Empty;
                return SA_AssetDatabase.GetAbsoluteAssetPath(Asset);
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
                else
                    return false;
            }
        }
    }
}
