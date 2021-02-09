namespace StansAssets.IOS.XCode
{
    [System.Serializable]
    class XCodeShellScript
    {
        public string Name = "New Run Script";
        public string Shell = string.Empty;
        public string Script = string.Empty;

        public string ShellScriptPath => "\\\"" + Script + "\\\"";
    }
}
