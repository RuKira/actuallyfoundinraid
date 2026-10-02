using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace RuKira.ActuallyFoundInRaid.Patches 
{
    public class AmmoSpawnedInSessionGetterPatch : ModulePatch
    {
        internal sealed class SpawnedInSessionValue
        {
            public bool Value;
        }

        internal static readonly ConditionalWeakTable<Ammo, SpawnedInSessionValue> Values = new ConditionalWeakTable<Ammo, SpawnedInSessionValue>();

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(Ammo), nameof(Ammo.SpawnedInSession));
        }

        [PatchPrefix]
        public static bool Prefix(Ammo __instance, ref bool __result)
        {
            __result = Values.TryGetValue(__instance, out SpawnedInSessionValue value) && value.Value;
            return false;
        }
    }

    public class AmmoSpawnedInSessionSetterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertySetter(typeof(Ammo), nameof(Ammo.SpawnedInSession));
        }

        [PatchPrefix]
        public static bool Prefix(Ammo __instance, bool value)
        {
            var storedValue = AmmoSpawnedInSessionGetterPatch.Values.GetOrCreateValue(__instance);
            storedValue.Value = value;
            return false;
        }
    }
}
