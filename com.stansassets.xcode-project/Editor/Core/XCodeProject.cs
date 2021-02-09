using UnityEngine;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// The main enter point for the XCode project setup.
    /// </summary>
    public static class XCodeProject
    {
        /// <summary>
        /// Capability Settings.
        /// </summary>
        public static XCodeCapabilitySettings Capability => XCodeProjectSettings.Instance.Capability;

        /// <summary>
        /// Sets a build property to the given value in all build configurations.
        /// </summary>
        /// <param name="name">The name of the build property. Example: "ENABLE_BITCODE"</param>
        /// <param name="value">The value of the build property.  Example: "NO"</param>
        public static void SetBuildProperty(string name, string value)
        {
            SetBuildProperty(new XCodeProjectProperty(name, value));
        }

        /// <summary>
        /// Sets a build property to the given value in all build configurations.
        /// </summary>
        /// <param name="property">Property object instance.</param>
        public static void SetBuildProperty(XCodeProjectProperty property)
        {
            foreach (var p in XCodeProjectSettings.Instance.BuildProperties)
                if (p.Name.Equals(property.Name))
                {
                    XCodeProjectSettings.Instance.BuildProperties.Remove(p);
                    break;
                }

            XCodeProjectSettings.Instance.BuildProperties.Add(property);
        }

        /// <summary>
        /// Removes the build property.
        /// </summary>
        /// <param name="property">Build property.</param>
        public static void RemoveBuildProperty(XCodeProjectProperty property)
        {
            RemoveBuildProperty(property.Name);
        }

        /// <summary>
        /// Removes the build property.
        /// </summary>
        /// <param name="name">Property name.</param>
        public static void RemoveBuildProperty(string name)
        {
            foreach (var p in XCodeProjectSettings.Instance.BuildProperties)
                if (p.Name.Equals(name))
                {
                    XCodeProjectSettings.Instance.BuildProperties.Remove(p);
                    break;
                }
        }

        //--------------------------------------
        // Info.plist
        //--------------------------------------

        /// <summary>
        ///  Method will add or replace new <see cref="InfoPlistKey"/> to the Info.plist keys
        /// </summary>
        /// <param name="key">Info.plist key name</param>
        public static void SetInfoPlistKey(InfoPlistKey key)
        {
            var infoPlistKey = GetInfoPlistKey(key.Name);
            if (infoPlistKey != null) XCodeProjectSettings.Instance.PlistVariables.Remove(infoPlistKey);

            XCodeProjectSettings.Instance.PlistVariables.Add(key);
        }

        /// <summary>
        /// Get's <see cref="InfoPlistKey"/> instance by it's name
        /// </summary>
        /// <param name="name">Info.plist key name</param>
        public static InfoPlistKey GetInfoPlistKey(string name)
        {
            foreach (var v in XCodeProjectSettings.Instance.PlistVariables)
                if (v.Name.Equals(name))
                    return v;
            return null;
        }

        /// <summary>
        /// Returns true if this key already exists inside Info.plist
        /// </summary>
        /// <param name="name">Info.plist key name</param>
        public static bool ContainsKey(string name)
        {
            return GetInfoPlistKey(name) != null;
        }

        /// <summary>
        /// Removes Info.plist key
        /// </summary>
        /// <param name="key">Info.plist key</param>
        public static void RemoveInfoPlistKey(InfoPlistKey key)
        {
            RemoveInfoPlistKey(key.Name);
        }

        /// <summary>
        /// Removes Info.plist key
        /// </summary>
        /// <param name="name">Info.plist key name</param>
        public static void RemoveInfoPlistKey(string name)
        {
            var key = GetInfoPlistKey(name);
            if (key != null) XCodeProjectSettings.Instance.RemoveVariable(key, XCodeProjectSettings.Instance.PlistVariables);
        }

        /// <summary>
        /// Adds compiler or linker flag to the build configuration
        /// </summary>
        /// <param name="name">flag name.</param>
        /// <param name="type">flag type.</param>
        public static void AddFlag(string name, XCodeFlagType type)
        {
            foreach (var flag in XCodeProjectSettings.Instance.Flags)
                if (flag.Type == type && flag.Name.Equals(name))
                    return;

            var newFlag = new XCodeProjectFlag();
            newFlag.Name = name;
            newFlag.Type = XCodeFlagType.LinkerFlag;

            XCodeProjectSettings.Instance.Flags.Add(newFlag);
        }

        /// <summary>
        /// Removes the flag.
        /// </summary>
        /// <param name="name">Flag name.</param>
        public static void RemoveFlag(string name)
        {
            foreach (var flag in XCodeProjectSettings.Instance.Flags)
                if (flag.Name.Equals(name))
                {
                    XCodeProjectSettings.Instance.Flags.Remove(flag);
                    return;
                }
        }

        /// <summary>
        /// Adds a system framework dependency.
        ///
        /// The function assumes system frameworks are located in System/Library/Frameworks folder
        /// in the SDK source tree. The framework is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="frameworkName">The name of the framework.</param>
        /// <param name="weak"><c>True</c> if the framework is optional (i.e. weakly linked), <c>false</c> if the framework is required.</param>
        public static void AddFramework(XCodeFrameworkName frameworkName, bool weak = false)
        {
            AddFramework(new XCodeFramework(frameworkName, weak));
        }

        /// <summary>
        /// Adds a system framework dependency.
        ///
        /// The function assumes system frameworks are located in System/Library/Frameworks folder
        /// in the SDK source tree. The framework is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="framework">framework configuration object</param>
        public static void AddFramework(XCodeFramework framework)
        {
            foreach (var item in XCodeProjectSettings.Instance.Frameworks)
                if (item.FrameworkName == framework.FrameworkName)
                {
                    XCodeProjectSettings.Instance.Frameworks.Remove(item);
                    break;
                }

            XCodeProjectSettings.Instance.Frameworks.Add(framework);
        }

        /// <summary>
        /// Removes the framework.
        /// </summary>
        /// <param name="frameworkName">Framework type</param>
        public static void RemoveFramework(XCodeFrameworkName frameworkName)
        {
            foreach (var item in XCodeProjectSettings.Instance.Frameworks)
                if (item.FrameworkName == frameworkName)
                {
                    XCodeProjectSettings.Instance.Frameworks.Remove(item);
                    break;
                }
        }

        /// <summary>
        /// Adds a system library dependency.
        ///
        /// The function assumes system library are located in System/Library/Frameworks folder
        /// in the SDK source tree. The library is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="libraryName">The name of the library.</param>
        /// <param name="weak"><c>True</c> if the framework is optional (i.e. weakly linked), <c>false</c> if the framework is required.</param>
        public static void AddLibrary(XCodeLibraryName libraryName, bool weak = false)
        {
            AddLibrary(new XCodeLibrary(libraryName, weak));
        }

        /// <summary>
        /// Adds a system library dependency.
        ///
        /// The function assumes system library are located in System/Library/Frameworks folder
        /// in the SDK source tree. The library is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="library">library configuration object</param>
        public static void AddLibrary(XCodeLibrary library)
        {
            foreach (var item in XCodeProjectSettings.Instance.Libraries)
                if (item.Name == library.Name)
                {
                    XCodeProjectSettings.Instance.Libraries.Remove(item);
                    break;
                }

            XCodeProjectSettings.Instance.Libraries.Add(library);
        }

        /// <summary>
        /// Removes the library.
        /// </summary>
        /// <param name="libraryName">Library type.</param>
        public static void RemoveLibrary(XCodeLibraryName libraryName)
        {
            foreach (var item in XCodeProjectSettings.Instance.Libraries)
                if (item.Name == libraryName)
                {
                    XCodeProjectSettings.Instance.Libraries.Remove(item);
                    break;
                }
        }

        /// <summary>
        /// Method that allow to check if certain asset was already added to the Xcode project.
        /// </summary>
        /// <param name="asset">A reference to the asset.</param>
        /// <returns>Returns `true` if asset is added and `false` otherwise. </returns>
        public static bool HasFile(Object asset)
        {
            foreach (var assetFile in XCodeProjectSettings.Instance.Files)
                if (assetFile.Asset.Equals(asset))
                    return true;

            return false;
        }

        /// <summary>
        /// Adds asset copy step.
        /// </summary>
        /// <param name="asset">A reference to the asset that will be copied.</param>
        /// <param name="xCodePath">The Xcode project related path of where to copy the file. Will copy in the project root by default.</param>
        public static void AddFile(Object asset, string xCodePath = "")
        {
            var file = new XCodeAsset();
            file.Asset = asset;
            file.XCodePath = xCodePath;

            AddFile(file);
        }

        /// <summary>
        /// Adds asset copy step.
        /// </summary>
        /// <param name="asset">Asset model to copy.</param>
        public static void AddFile(XCodeAsset asset)
        {
            foreach (var assetFile in XCodeProjectSettings.Instance.Files)
                if (assetFile.Asset.Equals(asset.Asset))
                {
                    XCodeProjectSettings.Instance.Files.Remove(assetFile);
                    break;
                }

            XCodeProjectSettings.Instance.Files.Add(asset);
        }
    }
}
