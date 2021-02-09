using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.XCode
{
    [Serializable]
    public class ISD_CapabilitySettings
    {
        [Serializable]
        public class Capability
        {
            public bool Enabled = false;
        }

        [Serializable]
        public class iCloudCapability : Capability
        {
            public bool keyValueStorage = false;
            public bool iCloudDocument = false;
            public List<string> customContainers = new List<string>();
        }

        [Serializable]
        public class PushNotificationsCapability : Capability
        {
            public bool development = true;
        }

        [Serializable]
        public class WalletCapability : Capability
        {
            public List<string> passSubset = new List<string>();
        }

        [Serializable]
        public class ApplePayCapability : Capability
        {
            public List<string> merchants = new List<string>();
        }

        [Serializable]
        public class MapsCapability : Capability
        {
            [Flags]
            [Serializable]
            public enum MapsOptions
            {
                None = 0,
                Airplane = 1,
                Bike = 2,
                Bus = 4,
                Car = 8,
                Ferry = 16,
                Pedestrian = 32,
                RideSharing = 64,
                StreetCar = 128,
                Subway = 256,
                Taxi = 512,
                Train = 1024,
                Other = 2048
            }

            public List<MapsOptions> options = new List<MapsOptions>();
        }

        [Serializable]
        public class BackgroundModesCapability : Capability
        {
            [Flags]
            [Serializable]
            public enum BackgroundModesOptions
            {
                None = 0,
                AudioAirplayPiP = 1,
                LocationUpdates = 2,
                VoiceOverIP = 4,
                NewsstandDownloads = 8,
                ExternalAccessoryCommunication = 16,
                UsesBluetoothLEAccessory = 32,
                ActsAsABluetoothLEAccessory = 64,
                BackgroundFetch = 128,
                RemoteNotifications = 256
            }

            public List<BackgroundModesOptions> options = new List<BackgroundModesOptions>();
        }

        [Serializable]
        public class KeychainSharingCapability : Capability
        {
            public List<string> accessGroups = new List<string>();
        }

        [Serializable]
        public class AssociatedDomainsCapability : Capability
        {
            public List<string> domains = new List<string>();
        }

        [Serializable]
        public class AppGroupsCapability : Capability
        {
            public List<string> groups = new List<string>();
        }

        public iCloudCapability iCloud = new iCloudCapability();
        public PushNotificationsCapability PushNotifications = new PushNotificationsCapability();
        public Capability GameCenter = new Capability();
        public Capability SignInWithApple = new Capability();
        public WalletCapability Wallet = new WalletCapability();
        public Capability Siri = new Capability();
        public ApplePayCapability ApplePay = new ApplePayCapability();
        public Capability InAppPurchase = new Capability();
        public MapsCapability Maps = new MapsCapability();
        public Capability PersonalVPN = new Capability();
        public BackgroundModesCapability BackgroundModes = new BackgroundModesCapability();
        public Capability InterAppAudio = new Capability();
        public KeychainSharingCapability KeychainSharing = new KeychainSharingCapability();
        public AssociatedDomainsCapability AssociatedDomains = new AssociatedDomainsCapability();
        public AppGroupsCapability AppGroups = new AppGroupsCapability();
        public Capability DataProtection = new Capability();
        public Capability HomeKit = new Capability();
        public Capability HealthKit = new Capability();
        public Capability WirelessAccessoryConfiguration = new Capability();
    }
}
