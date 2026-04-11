namespace Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public bool IsUnlocked;
        public ItemStack Stack;

        public bool IsEmpty => Stack == null || Stack.Count == 0;
    }
}