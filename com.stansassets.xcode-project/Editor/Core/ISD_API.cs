using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.XCode
{
    public static class ISD_API
    {
        //--------------------------------------
        // Build Property
        //--------------------------------------

        /// <summary>
        /// Sets a build property to the given value in all build configurations.
        /// </summary>
        /// <param name="name">The name of the build property. Example: "ENABLE_BITCODE"</param>
        /// <param name="value">The value of the build property.  Example: "NO"</param>
        public static void SetBuildProperty(string name, string value)
        {
            SetBuildProperty(new ISD_BuildProperty(name, value));
        }

        /// <summary>
        /// Sets a build property to the given value in all build configurations.
        /// </summary>
        /// <param name="property">Property object instance.</param>
        public static void SetBuildProperty(ISD_BuildProperty property)
        {
            foreach (var p in ISD_Settings.Instance.BuildProperties)
                if (p.Name.Equals(property.Name))
                {
                    ISD_Settings.Instance.BuildProperties.Remove(p);
                    break;
                }

            ISD_Settings.Instance.BuildProperties.Add(property);
        }

        /// <summary>
        /// Removes the build property.
        /// </summary>
        /// <param name="property">Build propery.</param>
        public static void RemoveBuildProperty(ISD_BuildProperty property)
        {
            RemoveBuildProperty(property.Name);
        }

        /// <summary>
        /// Removes the build property.
        /// </summary>
        /// <param name="name">Property name.</param>
        public static void RemoveBuildProperty(string name)
        {
            foreach (var p in ISD_Settings.Instance.BuildProperties)
                if (p.Name.Equals(name))
                {
                    ISD_Settings.Instance.BuildProperties.Remove(p);
                    break;
                }
        }

        //--------------------------------------
        // Info.plist
        //--------------------------------------

        /// <summary>
        ///  Method will add or replace new <see cref="ISD_PlistKey"/> to the Info.plist keys
        /// </summary>
        /// <param name="name">Info.plist key name</param>  
        public static void SetInfoPlistKey(ISD_PlistKey key)
        {
            var plisKey = GetInfoPlistKey(key.Name);
            if (plisKey != null) ISD_Settings.Instance.PlistVariables.Remove(plisKey);

            ISD_Settings.Instance.PlistVariables.Add(key);
        }

        /// <summary>
        /// Get's <see cref="ISD_PlistKey"/> instance by it's name
        /// </summary>
        /// <param name="name">Info.plist key name</param>
        public static ISD_PlistKey GetInfoPlistKey(string name)
        {
            foreach (var v in ISD_Settings.Instance.PlistVariables)
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
        public static void RemoveInfoPlistKey(ISD_PlistKey key)
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
            if (key != null) ISD_Settings.Instance.RemoveVariable(key, ISD_Settings.Instance.PlistVariables);
        }

        //--------------------------------------
        // Flags
        //--------------------------------------

        /// <summary>
        /// Add's compiler or linker flag to the build configuration
        /// </summary>
        /// <param name="name">flag name.</param>
        /// <param name="type">flag type.</param>
        public static void AddFlag(string name, ISD_FlagType type)
        {
            foreach (var flag in ISD_Settings.Instance.Flags)
                if (flag.Type == type && flag.Name.Equals(name))
                    return;

            var newFlag = new ISD_Flag();
            newFlag.Name = name;
            newFlag.Type = ISD_FlagType.LinkerFlag;

            ISD_Settings.Instance.Flags.Add(newFlag);
        }

        /// <summary>
        /// Removes the flag.
        /// </summary>
        /// <param name="name">Flag name.</param>
        public static void RemoveFlag(string name)
        {
            foreach (var flag in ISD_Settings.Instance.Flags)
                if (flag.Name.Equals(name))
                {
                    ISD_Settings.Instance.Flags.Remove(flag);
                    return;
                }
        }

        //--------------------------------------
        // Framework's
        //--------------------------------------

        /// <summary>
        /// Adds a system framework dependency.
        /// 
        /// The function assumes system frameworks are located in System/Library/Frameworks folder 
        /// in the SDK source tree. The framework is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="framework">The name of the framework.</param>
        /// <param name="weak"><c>True</c> if the framework is optional (i.e. weakly linked), <c>false</c> if the framework is required.</param>
        public static void AddFramework(ISD_iOSFramework framework, bool weak = false)
        {
            AddFramework(new ISD_Framework(framework, weak));
        }

        /// <summary>
        /// Adds a system framework dependency.
        /// 
        /// The function assumes system frameworks are located in System/Library/Frameworks folder 
        /// in the SDK source tree. The framework is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="framework">framework configuration object</param>    
        public static void AddFramework(ISD_Framework framework)
        {
            foreach (var item in ISD_Settings.Instance.Frameworks)
                if (item.Type == framework.Type)
                {
                    ISD_Settings.Instance.Frameworks.Remove(item);
                    break;
                }

            ISD_Settings.Instance.Frameworks.Add(framework);
        }

        /// <summary>
        /// Removes the framework.
        /// </summary>
        /// <param name="framework">Framework type</param>
        public static void RemoveFramework(ISD_iOSFramework framework)
        {
            foreach (var item in ISD_Settings.Instance.Frameworks)
                if (item.Type == framework)
                {
                    ISD_Settings.Instance.Frameworks.Remove(item);
                    break;
                }
        }

        //--------------------------------------
        // Libraries
        //--------------------------------------

        /// <summary>
        /// Adds a system library dependency.
        /// 
        /// The function assumes system library are located in System/Library/Frameworks folder 
        /// in the SDK source tree. The library is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="library">The name of the library.</param>
        /// <param name="weak"><c>True</c> if the framework is optional (i.e. weakly linked), <c>false</c> if the framework is required.</param>
        public static void AddLibrary(ISD_iOSLibrary library, bool weak = false)
        {
            AddLibrary(new ISD_Library(library, weak));
        }

        /// <summary>
        /// Adds a system library dependency.
        /// 
        /// The function assumes system library are located in System/Library/Frameworks folder 
        /// in the SDK source tree. The library is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="library">library configuration object</param>    
        public static void AddLibrary(ISD_Library library)
        {
            foreach (var item in ISD_Settings.Instance.Libraries)
                if (item.Type == library.Type)
                {
                    ISD_Settings.Instance.Libraries.Remove(item);
                    break;
                }

            ISD_Settings.Instance.Libraries.Add(library);
        }

        /// <summary>
        /// Removes the library.
        /// </summary>
        /// <param name="library">Library type.</param>
        public static void RemoveLibrary(ISD_iOSLibrary library)
        {
            foreach (var item in ISD_Settings.Instance.Libraries)
                if (item.Type == library)
                {
                    ISD_Settings.Instance.Libraries.Remove(item);
                    break;
                }
        }

        //--------------------------------------
        // Capabilities
        //--------------------------------------

        public static ISD_CapabilitySettings Capability => ISD_Settings.Instance.Capability;

        //--------------------------------------
        // Files
        //--------------------------------------

        public static bool HasFile(Object asset)
        {
            foreach (var assetFile in ISD_Settings.Instance.Files)
                if (assetFile.Asset.Equals(asset))
                    return true;

            return false;
        }

        public static void AddFile(Object asset, string xCodePath = "")
        {
            var file = new ISD_AssetFile();
            file.Asset = asset;
            file.XCodePath = xCodePath;

            AddFile(file);
        }

        public static void AddFile(ISD_AssetFile file)
        {
            foreach (var assetFile in ISD_Settings.Instance.Files)
                if (assetFile.Asset.Equals(file.Asset))
                {
                    ISD_Settings.Instance.Files.Remove(assetFile);
                    break;
                }

            ISD_Settings.Instance.Files.Add(file);
        }
    }
}
