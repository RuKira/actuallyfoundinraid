using BepInEx;
using RuKira.ActuallyFoundInRaid.Helpers;
using RuKira.ActuallyFoundInRaid.Patches;

namespace RuKira.ActuallyFoundInRaid
{
    [BepInPlugin("com.rukiragaming.actuallyfir", "ActuallyFoundInRaid", Version)]
    [BepInDependency("com.SPT.core", "4.0.0")]
    public class ActuallyFIRPlugin : BaseUnityPlugin
    {
        public const string Version = "1.0.4";

        private void Awake()
        {
            Settings.Init(Config);
            
            new BotBrainActivatePatch().Enable();
        }
    }
}