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
        public static void PatchPostfix(WildSpawnType __state, StandartBotBrain __instance, BotOwner ___BotOwner_0)
        {
            __state = ___BotOwner_0.Profile.Info.Settings.Role; // Store original type in state param to allow access in PatchPostFix()
            
            if (!Settings.ActuallyFIREnabled.Value)
                return;
            
            try
            {
                var isSptPmc = AIExtensions.IsPMC(___BotOwner_0);
                if (isSptPmc)
                {
                    var botProfile = ___BotOwner_0.Profile;
                    botProfile.SetSpawnedInSession(true);
                    
                    foreach (var slotType in Utils.SlotsToProcess)
                    {
                        Utils.ProcessSlot(botProfile.Inventory.Equipment.GetSlot(slotType));
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