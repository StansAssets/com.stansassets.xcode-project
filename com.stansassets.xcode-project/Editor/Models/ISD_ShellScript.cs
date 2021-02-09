using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace SA.iOS.XCode
{
    [System.Serializable]
    public class ISD_ShellScript
    {
        public string Name = "New Run Script";
        public string Shell = string.Empty;
        public string Script = string.Empty;

        public string ShellScriptPath => "\\\"" + Script + "\\\"";
    }
}
