using Items;

namespace Inventory
{
    [System.Serializable]
    internal sealed class ItemStack
    {
        public ItemData Item;
        public int Count;

        public bool IsEmpty => Item == null || Count <= 0;

        public bool IsFull => Item != null && Count >= Item.MaxStack;

        public void Clear()
        {
            Item = null;
            Count = 0;
        }        
        
        
    }
}