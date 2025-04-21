using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace PrivateRyan.ActuallyFoundInRaid.Helpers
{
    internal class Settings
    {
        public const string GeneralSectionTitle = "1. General";

        public static ConfigFile Config;

        public static ConfigEntry<bool> ActuallyFIREnabled;

        public static List<ConfigEntryBase> ConfigEntries = new List<ConfigEntryBase>();

        public static void Init(ConfigFile Config)
        {
            Settings.Config = Config;

            ConfigEntries.Add(ActuallyFIREnabled = Config.Bind(
                GeneralSectionTitle,
                "ActuallyFIR Enabled",
                true,  // Default value
                new ConfigDescription(
                    "Is Actually Found In Raid enabled?", 
                    null,
                    new ConfigurationManagerAttributes { Order = 0 }
                )));

            RecalcOrder();
        }

        private static void RecalcOrder()
        {
            // Set the Order field for all settings, to avoid unnecessary changes when adding new settings
            var settingOrder = ConfigEntries.Count;
            foreach (var attributes in ConfigEntries.Select(entry => entry.Description.Tags[0] as ConfigurationManagerAttributes))
            {
                if (attributes != null)
                {
                    attributes.Order = settingOrder;
                }

                settingOrder--;
            }
        }
    }
}