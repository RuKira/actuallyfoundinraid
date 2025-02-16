using BepInEx;
using PrivateRyan.ActuallyFIR.Helpers;
using PrivateRyan.ActuallyFIR.Patches;

namespace PrivateRyan.ActuallyFIR
{
    [BepInPlugin("privateryan.actuallyfir", "ActuallyFoundInRaid", "1.0.0")]
    [BepInDependency("com.SPT.core", "3.10.5")]
    public class ActuallyFIRPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Settings.Init(Config);
            
            new BotBrainActivatePatch().Enable();
        }
    }
}