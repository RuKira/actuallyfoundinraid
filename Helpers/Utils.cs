using EFT.InventoryLogic;

namespace PrivateRyan.ActuallyFIR.Helpers
{
    internal class Utils
    {
        public static EquipmentSlot[] SlotsToProcess { get; } = new[]
        {
            EquipmentSlot.Earpiece,
            EquipmentSlot.Headwear,
            EquipmentSlot.FaceCover,
            EquipmentSlot.Eyewear,
            EquipmentSlot.TacticalVest,
            EquipmentSlot.ArmorVest,
            EquipmentSlot.Backpack,
            EquipmentSlot.Holster,
            EquipmentSlot.FirstPrimaryWeapon,
            EquipmentSlot.SecondPrimaryWeapon,
            EquipmentSlot.Pockets
        };
        
        public static void ProcessSlot(Slot slot)
        {
            Item item = slot.ContainedItem;
            if (item != null)
            {
                item.SpawnedInSession = true;
                item.GetAllItems().ExecuteForEach(x => x.SpawnedInSession = true);
            }
        }
    }
}