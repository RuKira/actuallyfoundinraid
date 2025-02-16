using System;
using SPT.Reflection.Patching;
using HarmonyLib;
using System.Reflection;
using EFT;
using PrivateRyan.ActuallyFIR.Helpers;
using SPT.Custom.CustomAI;

namespace PrivateRyan.ActuallyFIR.Patches
{
    public class BotBrainActivatePatch : ModulePatch
    {
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(StandartBotBrain), nameof(StandartBotBrain.Activate));
        }

        [PatchPrefix]
        public static bool PatchPrefix(out WildSpawnType __state, StandartBotBrain __instance, BotOwner ___botOwner_0)
        {
            __state = ___botOwner_0.Profile.Info.Settings.Role; // Store original type in state param to allow access in PatchPostFix()
            
            if (!Settings.ActuallyFIREnabled.Value)
                return true;
            
            try
            {
                var isSptPmc = AiHelpers.BotIsSptPmc(__state, ___botOwner_0);
                if (isSptPmc)
                {
                    Profile botProfile = ___botOwner_0.Profile;
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
                Logger.LogError($"Error running CustomAiPatch PatchPrefix(): {ex.Message}");
                Logger.LogError(ex.StackTrace);
            }

            return true; // Do original 
        }
    }
}