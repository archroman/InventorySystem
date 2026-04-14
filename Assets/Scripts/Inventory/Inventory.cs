using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _slots;

        public List<InventorySlot> Slots => _slots;

        public bool TryAddItem(ItemData item)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked) continue;
                if (!slot.IsEmpty) continue;

                slot.Stack = new ItemStack()
                {
                    Item = item,
                    Count = 1
                };

                return true;
            }

            Debug.Log("Инвентарь полон");
            return false;
        }

        public bool TryAddAmmo(ItemData ammo, int amount)
        {
            int remaining = amount;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked) continue;
                if (!slot.IsEmpty) continue;

                int canAdd = ammo.MaxStack - slot.Stack.Count;
                int toAdd = Mathf.Min(canAdd, remaining);

                slot.Stack.Count += toAdd;
                remaining -= toAdd;

                if (remaining <= 0)
                    return true;
            }

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked) continue;
                if (!slot.IsEmpty) continue;

                int toAdd = Mathf.Min(ammo.MaxStack, remaining);

                slot.Stack = new ItemStack()
                {
                    Item = ammo,
                    Count = toAdd
                };

                remaining -= toAdd;

                if (remaining <= 0)
                    return true;
            }

            Debug.Log("Инвентарь полон");
            return false;
        }

        public bool RemoveRandomItem()
        {
            var availableSlots = _slots.FindAll(s => s.IsUnlocked && !s.IsEmpty);

            if (availableSlots.Count == 0)
            {
                Debug.Log("Инвентарь пуст");
                return false;
            }

            var slot = availableSlots[Random.Range(0, availableSlots.Count)];

            slot.Stack = null;
            return true;
        }

        public float GetTotalWeight()
        {
            float total = 0f;

            foreach (var slot in _slots)
            {
                if (slot.IsEmpty) continue;

                total += slot.Stack.Item.Weight * slot.Stack.Count;
            }

            return total;
        }
    }
}