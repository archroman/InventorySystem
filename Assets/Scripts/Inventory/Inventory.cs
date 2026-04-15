using System.Collections.Generic;
using Items;
using PlayerCurrency;
using UnityEngine;

namespace Inventory
{
    internal sealed class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _slots;
        [SerializeField] private int _startUnlockedSlots = 15;
        [SerializeField] private int[] _slotCosts;

        public List<InventorySlot> Slots => _slots;

        private void Awake()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].IsUnlocked = i < _startUnlockedSlots;
            }
        }

        public bool TryUnlockSlot(int index, CurrencyService currency)
        {
            if (index < 0 || index >= _slots.Count)
                return false;

            var slot = _slots[index];

            if (slot.IsUnlocked)
                return false;

            if (index > 0 && !_slots[index - 1].IsUnlocked)
                return false;

            if (index >= _slotCosts.Length)
                return false;

            int cost = _slotCosts[index];

            if (!currency.TrySpend(cost))
                return false;

            slot.IsUnlocked = true;

            Debug.Log($"Slot {index} unlocked");
            return true;
        }

        public bool TryAddItem(ItemData item)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;
                if (slot.Stack.Item.Id != item.Id) continue;
                if (slot.Stack.IsFull) continue;

                slot.Stack.Count++;
                return true;
            }

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || !slot.IsEmpty) continue;

                slot.Stack = new ItemStack
                {
                    Item = item,
                    Count = 1
                };

                return true;
            }

            return false;
        }

        public bool TryAddAmmo(ItemData ammo, int amount)
        {
            if (ammo is not AmmoData)
                return false;

            int remaining = amount;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;
                if (slot.Stack.Item.Id != ammo.Id) continue;
                if (slot.Stack.IsFull) continue;

                int canAdd = ammo.MaxStack - slot.Stack.Count;
                int toAdd = Mathf.Min(canAdd, remaining);

                slot.Stack.Count += toAdd;
                remaining -= toAdd;

                if (remaining <= 0)
                    return true;
            }

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || !slot.IsEmpty) continue;

                int toAdd = Mathf.Min(ammo.MaxStack, remaining);

                slot.Stack = new ItemStack
                {
                    Item = ammo,
                    Count = toAdd
                };

                remaining -= toAdd;

                if (remaining <= 0)
                    return true;
            }

            return false;
        }

        public bool TryShoot()
        {
            WeaponData weapon = null;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;

                if (slot.Stack.Item is WeaponData w)
                {
                    weapon = w;
                    break;
                }
            }

            if (weapon == null)
            {
                Debug.Log("В инвентаре нет оружия");
                return false;
            }

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;

                if (slot.Stack.Item is AmmoData ammo && ammo.Id == weapon.AmmoId)
                {
                    slot.Stack.Count--;
                    Debug.Log(
                        $"Выстрел из {weapon.name}, урон: {weapon.Damage}, осталось патронов: {slot.Stack.Count}");

                    if (slot.Stack.Count <= 0)
                    {
                        slot.Stack = null;
                    }

                    return true;
                }
            }

            Debug.Log("В инвентаре нет подходящих патронов");
            return false;
        }

        public bool RemoveRandomItem()
        {
            var available = _slots.FindAll(s => s.IsUnlocked && !s.IsEmpty);

            if (available.Count == 0)
                return false;

            var slot = available[Random.Range(0, available.Count)];

            slot.Stack = null;
            return true;
        }

        public float GetTotalWeight()
        {
            float total = 0;

            foreach (var slot in _slots)
            {
                if (slot.IsEmpty) continue;

                total += slot.Stack.Item.Weight * slot.Stack.Count;
            }

            return total;
        }

        public int GetSlotCost(int index)
        {
            if (index < 0 || index >= _slotCosts.Length)
                return 0;

            return _slotCosts[index];
        }
    }
}