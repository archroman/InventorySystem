using Items;

namespace Inventory
{
    [System.Serializable]
    public class ItemStack
    {
        public ItemData Item;
        public int Count;

        public bool IsFull => Count >= Item.MaxStack;
    }
}