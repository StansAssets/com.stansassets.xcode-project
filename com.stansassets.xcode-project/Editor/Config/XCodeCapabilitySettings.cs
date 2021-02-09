using System;
using System.Collections.Generic;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// Defines XCode project capability settings.
    /// </summary>
    [Serializable]
    public class XCodeCapabilitySettings
    {
        [Serializable]
        public class Capability
        {
            public bool Enabled = false;
        }

        [Serializable]
        // ReSharper disable once InconsistentNaming
        public class iCloudCapability : Capability
        {
            public bool KeyValueStorage = false;
            // ReSharper disable once InconsistentNaming
            public bool iCloudDocument = false;
            public List<string> CustomContainers = new List<string>();
        }

        [Serializable]
        public class PushNotificationsCapability : Capability
        {
            public bool Development = true;
        }

        [Serializable]
        public class WalletCapability : Capability
        {
            public List<string> PassSubset = new List<string>();
        }

        [Serializable]
        public class ApplePayCapability : Capability
        {
            public List<string> Merchants = new List<string>();
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

            public List<MapsOptions> Options = new List<MapsOptions>();
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
                // ReSharper disable once InconsistentNaming
                UsesBluetoothLEAccessory = 32,
                // ReSharper disable once InconsistentNaming
                ActsAsABluetoothLEAccessory = 64,
                BackgroundFetch = 128,
                RemoteNotifications = 256
            }

            public List<BackgroundModesOptions> Options = new List<BackgroundModesOptions>();
        }

        [Serializable]
        public class KeychainSharingCapability : Capability
        {
            public List<string> AccessGroups = new List<string>();
        }

        [Serializable]
        public class AssociatedDomainsCapability : Capability
        {
            public List<string> Domains = new List<string>();
        }

        [Serializable]
        public class AppGroupsCapability : Capability
        {
            public List<string> Groups = new List<string>();
        }

        // ReSharper disable once InconsistentNaming
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
