namespace Inventory
{
    [System.Serializable]
    internal sealed class InventorySlot
    {
        public bool IsUnlocked;
        public ItemStack Stack;

        public bool IsEmpty => Stack == null || Stack.Count == 0;
    }
}