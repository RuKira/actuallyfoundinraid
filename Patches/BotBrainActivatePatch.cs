using System;
using SPT.Reflection.Patching;
using HarmonyLib;
using System.Reflection;
using EFT;
using RuKira.ActuallyFoundInRaid.Helpers;
using SPT.Custom.CustomAI;

namespace RuKira.ActuallyFoundInRaid.Patches
{
    public class BotBrainActivatePatch : ModulePatch
    {
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(StandartBotBrain), nameof(StandartBotBrain.Activate));
        }

        [PatchPostfix]
        public static void PatchPostfix(StandartBotBrain __instance)
        {
            if (!Settings.ActuallyFIREnabled.Value)
                return;
            
            var owner = __instance._owner;
            try
            {
                var isSptPmc = AIExtensions.IsPMC(owner);
                if (isSptPmc)
                {
                    var botProfile = owner.Profile;
                    botProfile.SetSpawnedInSession(true);
                    
                    foreach (var slotType in Helpers.Utils.SlotsToProcess)
                    {
                        Helpers.Utils.ProcessSlot(botProfile.Inventory.Equipment.GetSlot(slotType));
                    }

                    Logger.LogInfo($"CoopBot {botProfile.Info.Nickname} was successfully created and patched.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error running CustomAiPatch PatchPostfix(): {ex.Message}");
                Logger.LogError(ex.StackTrace);
            }

            return; // Do original 
        }
    }
}