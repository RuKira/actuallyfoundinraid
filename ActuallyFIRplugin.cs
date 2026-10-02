using BepInEx;
using RuKira.ActuallyFoundInRaid.Helpers;
using RuKira.ActuallyFoundInRaid.Patches;

namespace RuKira.ActuallyFoundInRaid
{
    [BepInPlugin("com.rukiragaming.actuallyfir", "ActuallyFoundInRaid", Version)]
    [BepInDependency("com.SPT.core", "4.1.1")]
    public class ActuallyFIRPlugin : BaseUnityPlugin
    {
        public const string Version = "1.1.0";

        private void Awake()
        {
            Settings.Init(Config);

            // Patches Ammo.SpawnedInSession property to allow setting it to true or false instead of always returning false.
            new AmmoSpawnedInSessionGetterPatch().Enable();
            new AmmoSpawnedInSessionSetterPatch().Enable();

            new BotBrainActivatePatch().Enable();
        }
    }
}