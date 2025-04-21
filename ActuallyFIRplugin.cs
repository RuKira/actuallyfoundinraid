using BepInEx;
using PrivateRyan.ActuallyFoundInRaid.Helpers;
using PrivateRyan.ActuallyFoundInRaid.Patches;

namespace PrivateRyan.ActuallyFoundInRaid
{
    [BepInPlugin("privateryan.actuallyfir", "ActuallyFoundInRaid", Version)]
    [BepInDependency("com.SPT.core", "3.11")]
    public class ActuallyFIRPlugin : BaseUnityPlugin
    {
        public const string Version = "1.0.2";
        private void Awake()
        {
            Settings.Init(Config);
            
            new BotBrainActivatePatch().Enable();
        }
    }
}
