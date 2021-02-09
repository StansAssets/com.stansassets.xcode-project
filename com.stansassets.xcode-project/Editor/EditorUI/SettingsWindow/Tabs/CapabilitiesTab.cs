using System.Collections.Generic;
using UnityEngine;
using Rotorz.ReorderableList;
using StansAssets.Plugins.Editor;
using UnityEditor;

namespace StansAssets.IOS.XCode
{
    class CapabilitiesTab : IMGUILayoutElement
    {
        List<CapabilityLayout> m_CapabilitiesLayout;

        public override void OnLayoutEnable()
        {
            base.OnLayoutEnable();

            m_CapabilitiesLayout = new List<CapabilityLayout>();

            //iCloud
            var layout = new CapabilityLayout("iCloud", "cloud.png", () => { return XCodeProjectSettings.Instance.Capability.iCloud; }, () =>
            {
                using (new IMGUIHorizontalSpace(16))
                {
                    var cloud = XCodeProjectSettings.Instance.Capability.iCloud;
                    cloud.KeyValueStorage = IMGUILayout.ToggleFiled("Key Value Storage", cloud.KeyValueStorage, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                    cloud.iCloudDocument = IMGUILayout.ToggleFiled("iCloud Document", cloud.iCloudDocument, IMGUIToggleStyle.ToggleType.EnabledDisabled);

                    ReorderableListGUI.Title("Custom Containers");
                    ReorderableListGUI.ListField(cloud.CustomContainers, (Rect position, string itemValue) =>
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

            layout = new CapabilityLayout("Push Notifications", "push.png", () => { return XCodeProjectSettings.Instance.Capability.PushNotifications; }, () =>
            {
                using (new IMGUIHorizontalSpace(16))
                {
                    var pushNotifications = XCodeProjectSettings.Instance.Capability.PushNotifications;
                    pushNotifications.Development = IMGUILayout.ToggleFiled("Development", pushNotifications.Development, IMGUIToggleStyle.ToggleType.YesNo);
                }
            });

            m_CapabilitiesLayout.Add(layout);

            //Game Center
            layout = new CapabilityLayout("Game Center", "game.png", () => { return XCodeProjectSettings.Instance.Capability.GameCenter; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Game Center
            layout = new CapabilityLayout("Sign In With Apple", "keychaine.png", () => { return XCodeProjectSettings.Instance.Capability.SignInWithApple; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Wallet

            layout = new CapabilityLayout("Wallet", "wallet.png", () => { return XCodeProjectSettings.Instance.Capability.Wallet; }, () =>
            {
                var wallet = XCodeProjectSettings.Instance.Capability.Wallet;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Pass Subset");
                        ReorderableListGUI.ListField(wallet.PassSubset, EditorGUI.TextField, () =>
                        {
                            GUILayout.Label("You haven't added any pass subset.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //Siri
            layout = new CapabilityLayout("Siri", "siri.png", () => { return XCodeProjectSettings.Instance.Capability.Siri; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //ApplePay

            layout = new CapabilityLayout("Apple Pay", "pay.png", () => { return XCodeProjectSettings.Instance.Capability.ApplePay; }, () =>
            {
                var applePay = XCodeProjectSettings.Instance.Capability.ApplePay;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Merchants");
                        ReorderableListGUI.ListField(applePay.Merchants, (Rect position, string itemValue) =>
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
            layout = new CapabilityLayout("In-App Purchase", "purchase.png", () => { return XCodeProjectSettings.Instance.Capability.InAppPurchase; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //Maps
            layout = new CapabilityLayout("Maps", "maps.png", () => { return XCodeProjectSettings.Instance.Capability.Maps; }, () =>
            {
                var maps = XCodeProjectSettings.Instance.Capability.Maps;
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Options");
                        ReorderableListGUI.ListField(maps.Options, (Rect position, XCodeCapabilitySettings.MapsCapability.MapsOptions itemValue) =>
                        {
                            return (XCodeCapabilitySettings.MapsCapability.MapsOptions)EditorGUI.EnumPopup(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("Set maps capability options.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //PersonalVPN
            layout = new CapabilityLayout("Personal VPN", "vpn.png", () => { return XCodeProjectSettings.Instance.Capability.PersonalVPN; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //BackgroundModes
            layout = new CapabilityLayout("Background Modes", "back.png", () => { return XCodeProjectSettings.Instance.Capability.BackgroundModes; }, () =>
            {
                var backgroundModes = XCodeProjectSettings.Instance.Capability.BackgroundModes;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Options");
                        ReorderableListGUI.ListField(backgroundModes.Options, (Rect position, XCodeCapabilitySettings.BackgroundModesCapability.BackgroundModesOptions itemValue) =>
                        {
                            return (XCodeCapabilitySettings.BackgroundModesCapability.BackgroundModesOptions)EditorGUI.EnumPopup(position, itemValue);
                        }, () =>
                        {
                            GUILayout.Label("Set background modes capability options.");
                        });
                    }
                }
            });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new CapabilityLayout("Inter-App Audio", "inter.png", () => { return XCodeProjectSettings.Instance.Capability.InterAppAudio; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //KeychainSharing
            layout = new CapabilityLayout("Keychain Sharing", "keychaine.png", () => { return XCodeProjectSettings.Instance.Capability.KeychainSharing; }, () =>
            {
                var keychainSharing = XCodeProjectSettings.Instance.Capability.KeychainSharing;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Access Groups");
                        ReorderableListGUI.ListField(keychainSharing.AccessGroups, (Rect position, string itemValue) =>
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
            layout = new CapabilityLayout("Associated Domains", "associated.png", () => { return XCodeProjectSettings.Instance.Capability.AssociatedDomains; }, () =>
            {
                var associatedDomains = XCodeProjectSettings.Instance.Capability.AssociatedDomains;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Associated Domains");
                        ReorderableListGUI.ListField(associatedDomains.Domains, (Rect position, string itemValue) =>
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
            layout = new CapabilityLayout("App Groups", "app.png", () => { return XCodeProjectSettings.Instance.Capability.AppGroups; }, () =>
            {
                var appGroups = XCodeProjectSettings.Instance.Capability.AppGroups;

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(16);
                    using (new IMGUIBeginVertical())
                    {
                        ReorderableListGUI.Title("Groups");
                        ReorderableListGUI.ListField(appGroups.Groups, (Rect position, string itemValue) =>
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
            layout = new CapabilityLayout("Data Protection", "data.png", () => { return XCodeProjectSettings.Instance.Capability.DataProtection; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new CapabilityLayout("HomeKit", "homekit.png", () => { return XCodeProjectSettings.Instance.Capability.HomeKit; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new CapabilityLayout("HealthKit", "healhtkit.png", () => { return XCodeProjectSettings.Instance.Capability.HealthKit; }, () => { });
            m_CapabilitiesLayout.Add(layout);

            //InterAppAudio
            layout = new CapabilityLayout("Wireless Accessory Configuration", "wirelless.png", () => { return XCodeProjectSettings.Instance.Capability.WirelessAccessoryConfiguration; }, () => { });
            m_CapabilitiesLayout.Add(layout);
        }

        public override void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            foreach (var layout in m_CapabilitiesLayout) layout.OnGUI();

            if (EditorGUI.EndChangeCheck()) XCodeProjectSettings.Save();
        }
    }
}
