////////////////////////////////////////////////////////////////////////////////
//
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets)
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if (UNITY_IOS || UNITY_TVOS) && !ISD_DISABLED
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;
using System.Text.RegularExpressions;
using StansAssets.Foundation.Editor;

namespace StansAssets.IOS.XCode
{
    static class XCodeProjectBuildPostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget target, string projectPath)
        {
            var pbxProjPath = PBXProject.GetPBXProjectPath(projectPath);
            var proj = new PBXProject();
            proj.ReadFromFile(pbxProjPath);

#if UNITY_2019_3_OR_NEWER
            var targetGuid = proj.GetUnityMainTargetGuid();
            var frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();
#else
            var targetGuid = proj.TargetGuidByName("Unity-iPhone");
            var frameworkTargetGuid = proj.TargetGuidByName("Unity-iPhone");
#endif

            RegisterAppLanguages();

            AddFlags(proj, targetGuid);
            AddLibraries(proj, targetGuid);
            AddFrameworks(proj, frameworkTargetGuid, target);
            AddEmbeddedFrameworks(proj, targetGuid);
            AddPlistVariables(projectPath);
            ApplyBuildSettings(proj, frameworkTargetGuid);
            CopyAssetFiles(proj, projectPath, targetGuid);
            AddShellScriptBuildPhase(proj, targetGuid);

            proj.WriteToFile(pbxProjPath);

            var capManager = new ProjectCapabilityManager(pbxProjPath, XCodeProjectSettings.DefaultEntitlementsFileName, "Unity-iPhone");
            AddCapabilities(capManager);
            capManager.WriteToFile();

            //Some API simply does not work so on this block we are applying a workaround
            //after Unity deploy scrips has stopped working

            //Adding Embedded Frameworks
            foreach (var framework in XCodeProjectSettings.Instance.EmbededFrameworks)
            {
                var contents = File.ReadAllText(pbxProjPath);
                var pattern = "(?<=Embed Frameworks)(?:.*)(\\/\\* " + framework.FileName + "\\ \\*\\/)(?=; };)";
                var oldText = "/* " + framework.FileName + " */";
                var updatedText = "/* " + framework.FileName + " */; settings = {ATTRIBUTES = (CodeSignOnCopy, ); }";
                contents = Regex.Replace(contents, pattern, m => m.Value.Replace(oldText, updatedText));
                File.WriteAllText(pbxProjPath, contents);
            }

            var entitlementsPath = projectPath + "/" + XCodeProjectSettings.DefaultEntitlementsFileName;
            if (XCodeProjectSettings.Instance.EntitlementsMode == EntitlementsGenerationMode.Automatic)
            {
                if (XCodeProjectSettings.Instance.Capability.iCloud.Enabled)
                    if (XCodeProjectSettings.Instance.Capability.iCloud.KeyValueStorage && !XCodeProjectSettings.Instance.Capability.iCloud.iCloudDocument)
                    {
                        var entitlements = new PlistDocument();
                        entitlements.ReadFromFile(entitlementsPath);

                        var plistVariable = new PlistElementArray();
                        entitlements.root["com.apple.developer.icloud-container-identifiers"] = plistVariable;

                        entitlements.WriteToFile(entitlementsPath);
                    }
            }
            else
            {
                if (XCodeProjectSettings.Instance.EntitlementsFile != null)
                {
                    var entitlementsContentPath = AssetDatabaseUtility.GetAssetAbsolutePath(XCodeProjectSettings.Instance.EntitlementsFile);
                    var contents = File.ReadAllText(entitlementsContentPath);
                    File.WriteAllText(entitlementsPath, contents);
                }
                else
                {
                    Debug.LogWarning("ISD: EntitlementsMode set to Manual but no file provided");
                }
            }
        }

        static void AddPlistVariables(string projectPath)
        {
            var infoPlist = new PlistDocument();
            var infoPlistPath = projectPath + "/Info.plist";
            infoPlist.ReadFromFile(infoPlistPath);

            foreach (var variable in XCodeProjectSettings.Instance.PlistVariables)
            {
                PlistElement plistVariable = null;
                switch (variable.Type)
                {
                    case InfoPlistKeyType.String:
                        plistVariable = new PlistElementString(variable.StringValue);
                        break;
                    case InfoPlistKeyType.Integer:
                        plistVariable = new PlistElementInteger(variable.IntegerValue);
                        break;
                    case InfoPlistKeyType.Boolean:
                        plistVariable = new PlistElementBoolean(variable.BooleanValue);
                        break;
                    case InfoPlistKeyType.Array:
                        plistVariable = CreatePlistArray(variable);
                        break;
                    case InfoPlistKeyType.Dictionary:
                        plistVariable = CreatePlistDict(variable);
                        break;
                }

                infoPlist.root[variable.Name] = plistVariable;
            }

            // Get root
            var rootDict = infoPlist.root;

            // remove exit on suspend if it exists.
            var exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if (rootDict.values.ContainsKey(exitsOnSuspendKey)) rootDict.values.Remove(exitsOnSuspendKey);

            infoPlist.WriteToFile(infoPlistPath);
        }

        static PlistElementArray CreatePlistArray(InfoPlistKey variable, PlistElementArray array = null)
        {
            if (array == null) array = new PlistElementArray();

            foreach (var variableUniqueIdKey in variable.ChildrenIds)
            {
                var v = XCodeProjectSettings.Instance.GetVariableById(variableUniqueIdKey);

                switch (v.Type)
                {
                    case InfoPlistKeyType.String:
                        array.AddString(v.StringValue);
                        break;
                    case InfoPlistKeyType.Boolean:
                        array.AddBoolean(v.BooleanValue);
                        break;
                    case InfoPlistKeyType.Integer:
                        array.AddInteger(v.IntegerValue);
                        break;
                    case InfoPlistKeyType.Array:
                        CreatePlistArray(v, array.AddArray());
                        break;
                    case InfoPlistKeyType.Dictionary:
                        CreatePlistDict(v, array.AddDict());
                        break;
                }
            }

            return array;
        }

        static PlistElementDict CreatePlistDict(InfoPlistKey variable, PlistElementDict dict = null)
        {
            if (dict == null) dict = new PlistElementDict();

            foreach (var variableUniqueIdKey in variable.ChildrenIds)
            {
                var v = XCodeProjectSettings.Instance.GetVariableById(variableUniqueIdKey);

                switch (v.Type)
                {
                    case InfoPlistKeyType.String:
                        dict.SetString(v.Name, v.StringValue);
                        break;
                    case InfoPlistKeyType.Boolean:
                        dict.SetBoolean(v.Name, v.BooleanValue);
                        break;
                    case InfoPlistKeyType.Integer:
                        dict.SetInteger(v.Name, v.IntegerValue);
                        break;
                    case InfoPlistKeyType.Array:
                        var array = dict.CreateArray(v.Name);
                        CreatePlistArray(v, array);
                        break;
                    case InfoPlistKeyType.Dictionary:
                        var d = dict.CreateDict(v.Name);
                        CreatePlistDict(v, d);
                        break;
                }
            }

            return dict;
        }

        static void ApplyBuildSettings(PBXProject proj, string targetGUID)
        {
            foreach (var property in XCodeProjectSettings.Instance.BuildProperties)
            {
                proj.SetBuildProperty(targetGUID, property.Name, property.Value);
            }
        }

        static void AddFlags(PBXProject proj, string targetGuid)
        {
            foreach (var flag in XCodeProjectSettings.Instance.Flags)
            {
                if (flag.Type == XCodeFlagType.LinkerFlag) proj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", flag.Name);

                if (flag.Type == XCodeFlagType.LinkerFlag) proj.AddBuildProperty(targetGuid, "OTHER_CFLAGS", flag.Name);
            }
        }

        static void RegisterAppLanguages()
        {
            //We have nothing to register, no point to add en empty CFBundleLocalizations key.
            if (XCodeProjectSettings.Instance.Languages.Count == 0) return;

            var cfBundleLocalizations = new InfoPlistKey();
            cfBundleLocalizations.Name = XCodeProjectSettings.CfLocalizationsPlistKey;
            cfBundleLocalizations.Type = InfoPlistKeyType.Array;

            foreach (var lang in XCodeProjectSettings.Instance.Languages)
            {
                var langItem = new InfoPlistKey();
                langItem.Type = InfoPlistKeyType.String;
                langItem.StringValue = lang.Name;
                cfBundleLocalizations.AddChild(langItem);
            }

            XCodeProject.SetInfoPlistKey(cfBundleLocalizations);
        }

        static void AddCapabilities(ProjectCapabilityManager capManager)
        {
            var capability = XCodeProjectSettings.Instance.Capability;
            if (capability.iCloud.Enabled)
            {
                if (capability.iCloud.iCloudDocument || capability.iCloud.CustomContainers.Count > 0)
                    capManager.AddiCloud(capability.iCloud.KeyValueStorage, capability.iCloud.iCloudDocument, capability.iCloud.CustomContainers.ToArray());
                else
                    capManager.AddiCloud(capability.iCloud.KeyValueStorage, false, null);
            }

            if (capability.PushNotifications.Enabled) capManager.AddPushNotifications(capability.PushNotifications.Development);

            if (capability.GameCenter.Enabled) capManager.AddGameCenter();

#if UNITY_2019_3_OR_NEWER
            if (capability.SignInWithApple.Enabled)
            {
                capManager.AddSignInWithApple();
            }
#endif

            if (capability.Wallet.Enabled) capManager.AddWallet(capability.Wallet.PassSubset.ToArray());

            if (capability.Siri.Enabled) capManager.AddSiri();

            if (capability.ApplePay.Enabled) capManager.AddApplePay(capability.ApplePay.Merchants.ToArray());

            if (capability.InAppPurchase.Enabled) capManager.AddInAppPurchase();

            if (capability.Maps.Enabled)
            {
                var options = MapsOptions.None;
                foreach (var opt in capability.Maps.Options)
                {
                    var opt2 = (MapsOptions)opt;
                    options |= opt2;
                }

                capManager.AddMaps(options);
            }

            if (capability.PersonalVPN.Enabled) capManager.AddPersonalVPN();

            if (capability.BackgroundModes.Enabled)
            {
                var options = BackgroundModesOptions.None;
                foreach (var opt in capability.BackgroundModes.Options)
                {
                    var opt2 = (BackgroundModesOptions)opt;
                    options |= opt2;
                }

                capManager.AddBackgroundModes(options);
            }

            if (capability.InterAppAudio.Enabled) capManager.AddInterAppAudio();

            if (capability.KeychainSharing.Enabled) capManager.AddKeychainSharing(capability.KeychainSharing.AccessGroups.ToArray());

            if (capability.AssociatedDomains.Enabled) capManager.AddAssociatedDomains(capability.AssociatedDomains.Domains.ToArray());

            if (capability.AppGroups.Enabled) capManager.AddAppGroups(capability.AppGroups.Groups.ToArray());

            if (capability.DataProtection.Enabled) capManager.AddDataProtection();

            if (capability.HomeKit.Enabled) capManager.AddHomeKit();

            if (capability.HealthKit.Enabled) capManager.AddHealthKit();

            if (capability.WirelessAccessoryConfiguration.Enabled) capManager.AddWirelessAccessoryConfiguration();
        }

        static void AddFrameworks(PBXProject proj, string targetGuid, BuildTarget target)
        {
            foreach (var framework in XCodeProjectSettings.Instance.Frameworks)
            {
                if (target == BuildTarget.tvOS)
                    if (framework.FrameworkName == XCodeFrameworkName.EventKit)
                        continue;

                if (IsAvailableOnPlatform(framework, target)) proj.AddFrameworkToProject(targetGuid, framework.FullName, framework.IsOptional);
            }
        }

        static bool IsAvailableOnPlatform(XCodeFramework framework, BuildTarget target)
        {
            if (target == BuildTarget.tvOS)
                switch (framework.FrameworkName)
                {
                    case XCodeFrameworkName.MessageUI:
                    case XCodeFrameworkName.Contacts:
                    case XCodeFrameworkName.ContactsUI:
                    case XCodeFrameworkName.Social:
                    case XCodeFrameworkName.Accounts:
                        return false;
                }

            return true;
        }

        static void AddEmbeddedFrameworks(PBXProject proj, string targetGuid)
        {
            foreach (var framework in XCodeProjectSettings.Instance.EmbededFrameworks)
            {
                var fileGuid = proj.AddFile(framework.AbsoluteFilePath, "Frameworks/" + framework.FileName, PBXSourceTree.Source);
                var embedPhase = proj.AddCopyFilesBuildPhase(targetGuid, "Embed Frameworks", "", "10");
                proj.AddFileToBuildSection(targetGuid, embedPhase, fileGuid);
#if UNITY_2017_4_OR_NEWER
                proj.AddFileToEmbedFrameworks(targetGuid, fileGuid);
#endif
                proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
            }
        }

        static void AddShellScriptBuildPhase(PBXProject proj, string targetGuid)
        {
            foreach (var script in XCodeProjectSettings.Instance.ShellScripts)
            {
#if UNITY_2018_3_OR_NEWER
                proj.AddShellScriptBuildPhase(targetGuid, script.Name, script.Shell, script.Script);
#else
                MethodInfo dynMethod = proj.GetType().GetMethod("AppendShellScriptBuildPhase",
                                                 BindingFlags.NonPublic | BindingFlags.Instance, //if static AND public
                                                 null,
                                                 new[] { typeof(string), typeof(string), typeof(string), typeof(string) },//specify arguments to tell reflection which variant to look for
                                                 null);


                dynMethod.Invoke(proj, new object[] { targetGuid, script.Name, script.Shell, script.Script });
#endif
            }
        }

        static void AddLibraries(PBXProject proj, string targetGuid)
        {
            foreach (var lib in XCodeProjectSettings.Instance.Libraries) proj.AddFrameworkToProject(targetGuid, lib.FullName, lib.IsOptional);
        }

        static void CopyAssetFiles(PBXProject proj, string pathToBuiltProject, string targetGUID)
        {
            foreach (var file in XCodeProjectSettings.Instance.Files)
                if (file.IsDirectory)
                    foreach (var assetPath in Directory.GetFiles(file.RelativeFilePath))
                    {
                        var fileName = Path.GetFileName(assetPath);
                        var xCodeRelativePath = file.XCodeRelativePath + "/" + fileName;
                        CopyFile(xCodeRelativePath, assetPath, pathToBuiltProject, proj, targetGUID);
                    }
                else
                    CopyFile(file.XCodeRelativePath, file.RelativeFilePath, pathToBuiltProject, proj, targetGUID);
        }

        static void CopyFile(string xCodeRelativePath, string sourcePath, string pathToBuiltProject, PBXProject proj, string targetGUID)
        {
            var dstPath = Path.Combine(pathToBuiltProject, xCodeRelativePath);
            var rootPath = Path.GetDirectoryName(dstPath);

            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

            File.Copy(sourcePath, dstPath);

            var name = proj.AddFile(xCodeRelativePath, xCodeRelativePath, PBXSourceTree.Source);
            proj.AddFileToBuild(targetGUID, name);
        }
    }
}

#endif
