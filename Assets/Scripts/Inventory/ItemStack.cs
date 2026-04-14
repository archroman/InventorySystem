using Items;

namespace Inventory
{
    [System.Serializable]
    internal sealed class ItemStack
    {
        public ItemData Item;
        public int Count;

        public bool IsFull => Count >= Item.MaxStack;
    }
}