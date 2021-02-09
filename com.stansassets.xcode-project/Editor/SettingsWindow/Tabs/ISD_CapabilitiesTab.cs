using System.Collections.Generic;
using UnityEngine;
using Rotorz.ReorderableList;
using SA.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;

namespace SA.iOS.XCode
{
    public class ISD_CapabilitiesTab : IMGUILayoutElement
    {
        List<ISD_CapabilityLayout> m_CapabilitiesLayout;

        public override void OnLayoutEnable()
        {
            base.OnLayoutEnable();

            ISD_CapabilityLayout layout;
            m_CapabilitiesLayout = new List<ISD_CapabilityLayout>();

            //iCloud

            layout = new ISD_CapabilityLayout("iCloud", "cloud.png", () => { return ISD_Settings.Instance.Capability.iCloud; }, () =>
            {
                using (new SA_GuiHorizontalSpace(16))
                {
                    var cloud = ISD_Settings.Instance.Capability.iCloud;
                    cloud.keyValueStorage = IMGUILayout.ToggleFiled("Key Value Storage", cloud.keyValueStorage, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                    cloud.iCloudDocument = IMGUILayout.ToggleFiled("iCloud Document", cloud.iCloudDocument, IMGUIToggleStyle.ToggleType.EnabledDisabled);

                    ReorderableListGUI.Title("Custom Containers");
                    ReorderableListGUI.ListField(cloud.customContainers, (Rect position, string itemValue) =>
                    {
                        return EditorGUI.TextField(position, itemValue);
                    }, () =>
                    {
                        GUILayout.Label("You haven't added any custom containers yet.");
                    });
                }
            });

            m_CapabilitiesLayout.Add(layout);

            //Push Notifications

            layout = new ISD_CapabilityLayout("Push Notifications", "push.png", () => { return ISD_Settings.Instance.Capability.PushNotifications; }, () =>
            {
                using (new SA_GuiHorizontalSpace(16))
                {
                    var pushNotifications = ISD_Settings.Instance.Capability.PushNotifications;
                    pushNotifications.development = IMGUILayout.ToggleFiled("Development", pushNotifications.development, IMGUIToggleStyle.ToggleType.YesNo);
                }
            });

            m_CapabilitiesLayout.Add(layout);

            //Game Center
            layout = new ISD_CapabilityLayout("Game Center", "game.png", () => { return ISD_Settings.Instance.Capability.GameCenter; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Game Center
            layout = new ISD_CapabilityLayout("Sign In With Apple", "keychaine.png", () => { return ISD_Settings.Instance.Capability.SignInWithApple; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Wallet

            layout = new ISD_CapabilityLayout("Wallet", "wallet.png", () => { return ISD_Settings.Instance.Capability.Wallet; }, () =>
            {
                var wallet = ISD_Settings.Instance.Capability.Wallet;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Pass Subset");
                        ReorderableListGUI.ListField(wallet.passSubset, (Rect position, string itemValue) =>
                        {
                            return EditorGUI.TextField(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("You haven't added any pass subset.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //Siri
            layout = new ISD_CapabilityLayout("Siri", "siri.png", () => { return ISD_Settings.Instance.Capability.Siri; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //ApplePay

            layout = new ISD_CapabilityLayout("Apple Pay", "pay.png", () => { return ISD_Settings.Instance.Capability.ApplePay; }, () =>
            {
                var applePay = ISD_Settings.Instance.Capability.ApplePay;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Merchants");
                        ReorderableListGUI.ListField(applePay.merchants, (Rect position, string itemValue) =>
                        {
                            return EditorGUI.TextField(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("You haven't added any merchants yet.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //InAppPurchase
            layout = new ISD_CapabilityLayout("In-App Purchase", "purchase.png", () => { return ISD_Settings.Instance.Capability.InAppPurchase; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Maps
            layout = new ISD_CapabilityLayout("Maps", "maps.png", () => { return ISD_Settings.Instance.Capability.Maps; }, () =>
            {
                var maps = ISD_Settings.Instance.Capability.Maps;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Options");
                        ReorderableListGUI.ListField(maps.options, (Rect position, ISD_CapabilitySettings.MapsCapability.MapsOptions itemValue) =>
                        {
                            return (ISD_CapabilitySettings.MapsCapability.MapsOptions)EditorGUI.EnumPopup(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("Set maps capability options.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //PersonalVPN
            layout = new ISD_CapabilityLayout("Personal VPN", "vpn.png", () => { return ISD_Settings.Instance.Capability.PersonalVPN; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //BackgroundModes
            layout = new ISD_CapabilityLayout("Background Modes", "back.png", () => { return ISD_Settings.Instance.Capability.BackgroundModes; }, () =>
            {
                var backgroundModes = ISD_Settings.Instance.Capability.BackgroundModes;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Options");
                        ReorderableListGUI.ListField(backgroundModes.options, (Rect position, ISD_CapabilitySettings.BackgroundModesCapability.BackgroundModesOptions itemValue) =>
                        {
                            return (ISD_CapabilitySettings.BackgroundModesCapability.BackgroundModesOptions)EditorGUI.EnumPopup(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("Set background modes capability options.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new ISD_CapabilityLayout("Inter-App Audio", "inter.png", () => { return ISD_Settings.Instance.Capability.InterAppAudio; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //KeychainSharing
            layout = new ISD_CapabilityLayout("Keychain Sharing", "keychaine.png", () => { return ISD_Settings.Instance.Capability.KeychainSharing; }, () =>
            {
                var keychainSharing = ISD_Settings.Instance.Capability.KeychainSharing;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Access Groups");
                        ReorderableListGUI.ListField(keychainSharing.accessGroups, (Rect position, string itemValue) =>
                        {
                            return EditorGUI.TextField(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("You haven't added any access groups yet.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //AssociatedDomains
            layout = new ISD_CapabilityLayout("Associated Domains", "associated.png", () => { return ISD_Settings.Instance.Capability.AssociatedDomains; }, () =>
            {
                var associatedDomains = ISD_Settings.Instance.Capability.AssociatedDomains;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Associated Domains");
                        ReorderableListGUI.ListField(associatedDomains.domains, (Rect position, string itemValue) =>
                        {
                            return EditorGUI.TextField(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("You haven't added any domains yet.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //AssociatedDomains
            layout = new ISD_CapabilityLayout("App Groups", "app.png", () => { return ISD_Settings.Instance.Capability.AppGroups; }, () =>
            {
                var appGroups = ISD_Settings.Instance.Capability.AppGroups;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Groups");
                        ReorderableListGUI.ListField(appGroups.groups, (Rect position, string itemValue) =>
                        {
                            return EditorGUI.TextField(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("You haven't added any groups yet.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new ISD_CapabilityLayout("Data Protection", "data.png", () => { return ISD_Settings.Instance.Capability.DataProtection; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new ISD_CapabilityLayout("HomeKit", "homekit.png", () => { return ISD_Settings.Instance.Capability.HomeKit; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new ISD_CapabilityLayout("HealthKit", "healhtkit.png", () => { return ISD_Settings.Instance.Capability.HealthKit; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new ISD_CapabilityLayout("Wireless Accessory Configuration", "wirelless.png", () => { return ISD_Settings.Instance.Capability.WirelessAccessoryConfiguration; }, () => { });
            m_CapabilitiesLayout.Add(layout);
        }

        public override void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            foreach (var layout in m_CapabilitiesLayout) layout.OnGUI();

            if (EditorGUI.EndChangeCheck()) ISD_Settings.Save();
        }
    }
}
