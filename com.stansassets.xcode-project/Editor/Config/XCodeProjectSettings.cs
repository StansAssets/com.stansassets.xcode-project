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
    /// <summary>
    /// Xcode projects settings object.
    /// Through it you can get access to all frameworks, libraries plist.list variables and others settings.
    /// </summary>
    public class XCodeProjectSettings : PackageScriptableSettingsSingleton<XCodeProjectSettings>
    {
        /// <summary>
        /// Name of the package.
        /// </summary>
        public override string PackageName => XCodePackage.PackageName;
        protected override bool IsEditorOnly => true;
        public const string DefaultEntitlementsFileName = "default.entitlements";
        public const string CfLocalizationsPlistKey = "CFBundleLocalizations";

        //Post Process Libs
        /// <summary>
        /// Entry point for all frameworks in the xcode project.
        /// </summary>
        public List<XCodeFramework> Frameworks = new List<XCodeFramework>();
        /// <summary>
        /// List of all embedded frameworks in the xcode project.
        /// </summary>
        public List<XCodeEmbedFramework> EmbededFrameworks = new List<XCodeEmbedFramework>();
        /// <summary>
        /// List of all libraries in the xcode project.
        /// </summary>
        public List<XCodeLibrary> Libraries = new List<XCodeLibrary>();
        /// <summary>
        /// List of all flags in the xcode project.
        /// </summary>
        public List<XCodeProjectFlag> Flags = new List<XCodeProjectFlag>();
        /// <summary>
        /// List of info plist variables in the xcode project.
        /// </summary>
        public List<InfoPlistKey> PlistVariables = new List<InfoPlistKey>();
        /// <summary>
        /// List of plist id's variables in the xcode project.
        /// </summary>
        public List<PlistKeyId> VariableDictionary = new List<PlistKeyId>();
        /// <summary>
        /// List of all languages in the xcode project.
        /// </summary>
        public List<SA_ISOLanguage> Languages = new List<SA_ISOLanguage>();
        /// <summary>
        /// Entry point for all shell scripts in the xcode project.
        /// </summary>
        public List<XCodeShellScript> ShellScripts = new List<XCodeShellScript>();

        [SerializeField]
        List<XCodeProjectProperty> m_BuildProperties = new List<XCodeProjectProperty>();

        /// <summary>
        /// Is Post Process Enabled.
        /// </summary>
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

        /// <summary>
        /// Capability Settings.
        /// </summary>
        public XCodeCapabilitySettings Capability = new XCodeCapabilitySettings();
        /// <summary>
        /// Xcode assets.
        /// </summary>
        public List<XCodeAsset> Files = new List<XCodeAsset>();
        public EntitlementsGenerationMode EntitlementsMode = EntitlementsGenerationMode.Automatic;
        public Object EntitlementsFile = null;

        //--------------------------------------
        // Variables
        //--------------------------------------

        /// <summary>
        /// Method for adding new variable into <c>VariableDictionary</c> list.
        /// </summary>
        /// <param name="uniqueIdKey"> Unique id key</param>
        /// <param name="var"> Info plist key value</param>
        public void AddVariableToDictionary(string uniqueIdKey, InfoPlistKey var)
        {
            var newVar = new PlistKeyId();
            newVar.uniqueIdKey = uniqueIdKey;
            newVar.VariableValue = var;
            VariableDictionary.Add(newVar);
        }

        /// <summary>
        /// Method that removes info plist key from <c>VariableDictionary</c> list.
        /// </summary>
        /// <param name="v"> Info plist key value.</param>
        /// <param name="listWithThisVariable"> </param>
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

        /// <summary>
        /// Method that returns does <c>list</c> contains children with given <c>key</c>.
        /// </summary>
        /// <param name="id"> Id that we want to check for.</param>
        /// <param name="list"> List of info plist in which we what to check.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method that returns info plist key by given key id.
        /// </summary>
        /// <param name="uniqueIdKey"> Uniques id</param>
        /// <returns> <c>InfoPlistKey</c> or null if we couldn't find any.</returns>
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

        /// <summary>
        /// List of project build properties.
        /// </summary>
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
