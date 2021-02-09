using UnityEngine;
using UnityEditor;
using SA.Foundation.Localization;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;

namespace StansAssets.IOS.XCode
{
    class GeneralWindowTab : IMGUILayoutElement
    {
        [SerializeField]
        bool m_IsDefFrameworksOpen = false;
        [SerializeField]
        bool m_IsDefLibrariesOpen = false;

        IMGUIPluginActiveTextLink m_EntitlementsLink;

        public override void OnAwake()
        {
            base.OnAwake();
            m_EntitlementsLink = new IMGUIPluginActiveTextLink("[?] Read More");
        }

        public override void OnGUI()
        {
#if (UNITY_IOS || UNITY_TVOS)
            Settings();
#else
            EditorGUILayout.HelpBox("XCode Build Post-Process isn't avaliable on current platfrom", MessageType.Error);
#endif

            var buildSettingsIcon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "BuildSettings.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Build Properties", buildSettingsIcon)))
            {
                foreach (var property in XCodeProjectSettings.Instance.BuildProperties) property.Value = IMGUILayout.StringValuePopup(property.Name, property.Value, property.Options);
                EditorGUILayout.Space();
            }

            Frameworks();
            DrawEmbeddedBlock();

            BuildFlags();
            Languages();

            CopyFilestoXCodeBuildPhasee();
            ShellScriptsBuildPhasee();

            Entitlements();
        }

        void Settings()
        {
            var settingsIcon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "isdSettings.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Settings", settingsIcon)))
            {
                var isEnabled = XCodeProjectSettings.PostProcessEnabled;
                EditorGUI.BeginChangeCheck();
                isEnabled = IMGUILayout.ToggleFiled("Build PostProcess", isEnabled, IMGUIToggleStyle.ToggleType.YesNo);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.DisplayDialog("Configuration", "Changes will be applide after scripts re-recompilation is completed", "Okay");
                    if (isEnabled)
                        EditorDefines.RemoveCompileDefine("ISD_DISABLED", BuildTarget.iOS);
                    else
                        EditorDefines.AddCompileDefine("ISD_DISABLED", BuildTarget.iOS);
                }

                if (!XCodeProjectSettings.PostProcessEnabled)
                    EditorGUILayout.HelpBox("Build PostProcess is curreently disabled. " +
                        "The XCode project will not be modifayed.", MessageType.Error);
            }
        }

        void Entitlements()
        {
            var entitlements = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "entitlements.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Entitlements", entitlements)))
            {
                EditorGUILayout.HelpBox("Entitlements confer specific capabilities or security permissions to your iOS or macOS app.\n" +
                    "By default Entitlements file is generated based on your deploy settings, " +
                    "but you may alos provide own version of the entitlements file by using manual mode", MessageType.Info);
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.FlexibleSpace();
                    var click = m_EntitlementsLink.DrawWithCalcSize();
                    if (click) Application.OpenURL("https://developer.apple.com/documentation/uikit/core_app/protecting_the_user_s_privacy?language=objc");
                }

                EditorGUILayout.Space();

                XCodeProjectSettings.Instance.EntitlementsMode = (EntitlementsGenerationMode)IMGUILayout.EnumPopup("Generation Mode", XCodeProjectSettings.Instance.EntitlementsMode);
                if (XCodeProjectSettings.Instance.EntitlementsMode == EntitlementsGenerationMode.Manual)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Entitlements File:");
                    XCodeProjectSettings.Instance.EntitlementsFile = EditorGUILayout.ObjectField(XCodeProjectSettings.Instance.EntitlementsFile, typeof(Object), false);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        void CopyFilestoXCodeBuildPhasee()
        {
            var folder = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "folder.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Copy Files to XCode Build Phasee", folder)))
            {
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.Files,
                    (XCodeAsset file) =>
                    {
                        return file.XCodeRelativePath;
                    },
                    (XCodeAsset file) =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Asset: ");
                        file.Asset = EditorGUILayout.ObjectField(file.Asset, typeof(UnityEngine.Object), false);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("XCode Path:");
                        file.XCodePath = EditorGUILayout.TextField(file.XCodePath);
                        EditorGUILayout.EndHorizontal();
                    },
                    () =>
                    {
                        XCodeProjectSettings.Instance.Files.Add(new XCodeAsset());
                    }
                );

                EditorGUILayout.Space();
            }
        }

        void ShellScriptsBuildPhasee()
        {
            var plistVariables_icon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "plistVariables.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Shell Scripts Build Phasee", plistVariables_icon)))
            {
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.ShellScripts,
                    (XCodeShellScript script) =>
                    {
                        return script.Name;
                    },
                    (XCodeShellScript script) =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Name: ");
                        script.Name = EditorGUILayout.TextField(script.Name);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Shell: ");
                        script.Shell = EditorGUILayout.TextField(script.Shell);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Script");
                        script.Script = EditorGUILayout.TextField(script.Script);
                        EditorGUILayout.EndHorizontal();
                    },
                    () =>
                    {
                        XCodeProjectSettings.Instance.ShellScripts.Add(new XCodeShellScript());
                    }
                );
                EditorGUILayout.Space();
            }
        }

        public void BuildFlags()
        {
            var linkerFlags = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "linkerFlags.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Linker & Compiler Flags", linkerFlags)))
            {
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.Flags,
                    (XCodeProjectFlag flag) =>
                    {
                        if (flag.Type.Equals(XCodeFlagType.CompilerFlag))
                            return flag.Name + "     (Compiler)";
                        else
                            return flag.Name + "     (Linker)";
                    },
                    (XCodeProjectFlag flag) =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Full Name:", GUILayout.Width(100));
                        flag.Name = EditorGUILayout.TextField(flag.Name, GUILayout.ExpandWidth(true));
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Type:", GUILayout.Width(100));

                        //flag.Type     = EditorGUILayout.TextField(flag.Type, GUILayout.ExpandWidth(true));
                        flag.Type = (XCodeFlagType)EditorGUILayout.EnumPopup(flag.Type);
                        EditorGUILayout.EndHorizontal();
                    },
                    () =>
                    {
                        var newFlag = new XCodeProjectFlag();
                        newFlag.Name = "New Flag";
                        XCodeProjectSettings.Instance.Flags.Add(newFlag);
                    }
                );
            }

            EditorGUILayout.Space();
        }

        int s_newLangindex = 0;

        public void Languages()
        {
            EditorGUI.BeginChangeCheck();

            var languages_icon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "languages.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Languages", languages_icon)))
            {
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.Languages,
                    (SA_ISOLanguage lang) =>
                    {
                        return lang.Code.ToUpper() + "     (" + lang.Name + ")";
                    }
                );

                EditorGUILayout.BeginHorizontal();
                s_newLangindex = EditorGUILayout.Popup(s_newLangindex, SA_LanguagesUtil.ISOLanguagesList.Names.ToArray());
                if (GUILayout.Button("Add Language", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var lang = SA_LanguagesUtil.ISOLanguagesList.Languages[s_newLangindex];
                    XCodeProjectSettings.Instance.Languages.Add(lang);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
                if (XCodeProjectSettings.Instance.Languages.Count == 0)
                    XCodeProject.RemoveInfoPlistKey(XCodeProjectSettings.CfLocalizationsPlistKey);
        }

        public int NewBaseFrameworkIndex = 0;
        public int NewLibraryIndex = 0;

        public void Frameworks()
        {
            var framework_icon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "frameworks.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Frameworks", framework_icon)))
            {
                m_IsDefFrameworksOpen = EditorGUILayout.Foldout(m_IsDefFrameworksOpen,
                    "Show Default Unity Frameworks (" + FrameworkHandler.DefaultFrameworks.Count + "Enabled)");
                if (m_IsDefFrameworksOpen)
                {
                    var indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Space(30);

                        using (new IMGUIBeginVertical(GUI.skin.box))
                        {
                            foreach (var framework in FrameworkHandler.DefaultFrameworks) IMGUILayout.SelectableLabel(framework.FrameworkName.ToString() + ".framework", "");
                        }
                    }

                    EditorGUI.indentLevel = indentLevel;
                    EditorGUILayout.Space();
                }

                m_IsDefLibrariesOpen = EditorGUILayout.Foldout(m_IsDefLibrariesOpen, "Default Unity Libraries (2 Enabled)");
                if (m_IsDefLibrariesOpen)
                {
                    var indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Space(30);

                        using (new IMGUIBeginVertical(GUI.skin.box))
                        {
                            IMGUILayout.SelectableLabel("libiPhone-lib.a", "");
                            IMGUILayout.SelectableLabel("libiconv.2.dylib", "");
                        }
                    }

                    EditorGUI.indentLevel = indentLevel;

                    EditorGUILayout.Space();
                }

                //Frameworks List
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.Frameworks,
                    (XCodeFramework framework) =>
                    {
                        if (framework.IsOptional && framework.IsEmbeded)
                            return framework.FullName + "       (Optional & Embeded)";
                        else if (framework.IsOptional)
                            return framework.FullName + "        (Optional)";
                        else if (framework.IsEmbeded)
                            return framework.FullName + "        (Embeded)";
                        else
                            return framework.FullName;
                    },
                    (XCodeFramework framework) =>
                    {
                        framework.IsOptional = IMGUILayout.ToggleFiled("Optional", framework.IsOptional, IMGUIToggleStyle.ToggleType.YesNo);
                        framework.IsEmbeded = IMGUILayout.ToggleFiled("Embeded", framework.IsEmbeded, IMGUIToggleStyle.ToggleType.YesNo);
                    }
                );

                //Libraries List
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.Libraries,
                    (XCodeLibrary lib) =>
                    {
                        if (lib.IsOptional)
                            return lib.FullName + "    (Optional)";
                        else
                            return lib.FullName;
                    },
                    (XCodeLibrary lib) =>
                    {
                        lib.IsOptional = IMGUILayout.ToggleFiled("Optional", lib.IsOptional, IMGUIToggleStyle.ToggleType.YesNo);
                    }
                );

                //Add New Framework
                EditorGUILayout.BeginHorizontal();
                NewBaseFrameworkIndex = EditorGUILayout.Popup(NewBaseFrameworkIndex, FrameworkHandler.BaseFrameworksArray());

                if (GUILayout.Button("Add Framework", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var type = FrameworkHandler.BaseFrameworksArray()[NewBaseFrameworkIndex];
                    NewBaseFrameworkIndex = 0;

                    var f = new XCodeFramework(type);
                    XCodeProject.AddFramework(f);
                }

                EditorGUILayout.EndHorizontal();

                //Add New Library
                EditorGUILayout.BeginHorizontal();
                NewLibraryIndex = EditorGUILayout.Popup(NewLibraryIndex, LibraryHandler.BaseLibrariesArray());

                if (GUILayout.Button("Add Library", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var type = (XCodeLibraryName)LibraryHandler.EnumValueOf(LibraryHandler.BaseLibrariesArray()[NewLibraryIndex]);
                    NewLibraryIndex = 0;
                    XCodeProject.AddLibrary(type);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        static void DrawEmbeddedBlock()
        {
            var libraries_icon = EditorAssetDatabase.GetTextureAtPath(XCodeWindowSkin.IconsPath + "libraries.png");
            using (new IMGUIWindowBlockWithIndent(new GUIContent("Embedded Binaries", libraries_icon)))
            {
                IMGUILayout.ReorderablList(XCodeProjectSettings.Instance.EmbededFrameworks,
                    (XCodeEmbedFramework framework) =>
                    {
                        return framework.FileName;
                    },
                    (XCodeEmbedFramework freamwork) =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Asset: ");
                        freamwork.Asset = EditorGUILayout.ObjectField(freamwork.Asset, typeof(UnityEngine.Object), false);
                        EditorGUILayout.EndHorizontal();
                    },
                    () =>
                    {
                        XCodeProjectSettings.Instance.EmbededFrameworks.Add(new XCodeEmbedFramework());
                    }
                );
            }
        }
    }
}
