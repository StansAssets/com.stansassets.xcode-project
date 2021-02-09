////////////////////////////////////////////////////////////////////////////////
//
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets)
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SA.Foundation.Localization;
using StansAssets.Plugins;

namespace StansAssets.IOS.XCode
{
    class XCodeProjectSettings : PackageScriptableSettingsSingleton<XCodeProjectSettings>
    {
        public override string PackageName => XCodePackage.PackageName;
        protected override bool IsEditorOnly => true;
        public const string DefaultEntitlementsFileName = "default.entitlements";
        public const string CfLocalizationsPlistKey = "CFBundleLocalizations";

        //Post Process Libs
        public List<XCodeFramework> Frameworks = new List<XCodeFramework>();
        public List<XCodeEmbedFramework> EmbededFrameworks = new List<XCodeEmbedFramework>();
        public List<XCodeLibrary> Libraries = new List<XCodeLibrary>();
        public List<XCodeProjectFlag> Flags = new List<XCodeProjectFlag>();
        public List<InfoPlistKey> PlistVariables = new List<InfoPlistKey>();
        public List<PlistKeyId> VariableDictionary = new List<PlistKeyId>();
        public List<SA_ISOLanguage> Languages = new List<SA_ISOLanguage>();

        public List<XCodeShellScript> ShellScripts = new List<XCodeShellScript>();

        [SerializeField]
        List<XCodeProjectProperty> m_BuildProperties = new List<XCodeProjectProperty>();

        public static bool PostProcessEnabled
        {
            get
            {
#if ISD_DISABLED
                return false;
#endif
                return true;
            }
        }

        public XCodeCapabilitySettings Capability = new XCodeCapabilitySettings();
        public List<XCodeAsset> Files = new List<XCodeAsset>();
        public EntitlementsGenerationMode EntitlementsMode = EntitlementsGenerationMode.Automatic;
        public Object EntitlementsFile = null;

        //--------------------------------------
        // Variables
        //--------------------------------------

        public void AddVariableToDictionary(string uniqueIdKey, InfoPlistKey var)
        {
            var newVar = new PlistKeyId();
            newVar.uniqueIdKey = uniqueIdKey;
            newVar.VariableValue = var;
            VariableDictionary.Add(newVar);
        }

        public void RemoveVariable(InfoPlistKey v, IList listWithThisVariable)
        {
            if (Instance.PlistVariables.Contains(v))
                Instance.PlistVariables.Remove(v);
            else
                foreach (var vid in VariableDictionary)
                    if (vid.VariableValue.Equals(v))
                    {
                        VariableDictionary.Remove(vid);
                        var id = vid.uniqueIdKey;
                        if (listWithThisVariable.Contains(id))
                            listWithThisVariable.Remove(vid.uniqueIdKey);
                        break;
                    }

            //remove junk

            var keysInUse = new List<PlistKeyId>(VariableDictionary);

            foreach (var id in VariableDictionary)
            {
                var isInUse = IsInUse(id.uniqueIdKey, PlistVariables);
                if (!isInUse) keysInUse.Remove(id);
            }

            VariableDictionary = keysInUse;
        }

        bool IsInUse(string id, List<InfoPlistKey> list)
        {
            foreach (var key in list)
                if (key.ChildrenIds.Contains(id))
                {
                    return true;
                }
                else
                {
                    var inUse = IsInUse(id, key.Children);
                    if (inUse) return true;
                }

            return false;
        }

        public InfoPlistKey GetVariableById(string uniqueIdKey)
        {
            foreach (var vid in VariableDictionary)
                if (vid.uniqueIdKey.Equals(uniqueIdKey))
                    return vid.VariableValue;
            return null;
        }

        //--------------------------------------
        // Build Properties
        //--------------------------------------

        public List<XCodeProjectProperty> BuildProperties
        {
            get
            {
                if (m_BuildProperties.Count == 0)
                {
                    var property = new XCodeProjectProperty("ENABLE_BITCODE", "NO");
                    m_BuildProperties.Add(property);

                    property = new XCodeProjectProperty("ENABLE_TESTABILITY", "NO");
                    m_BuildProperties.Add(property);

                    property = new XCodeProjectProperty("GENERATE_PROFILING_CODE", "NO");
                    m_BuildProperties.Add(property);
                }

                return m_BuildProperties;
            }
        }
    }
}
