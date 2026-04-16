using System.Collections.Generic;
using Items;
using PlayerCurrency;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Inventory
{
    internal sealed class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _slots;
        [SerializeField] private int _startUnlockedSlots = 15;
        [SerializeField] private int[] _slotCosts;

        public List<InventorySlot> Slots => _slots;

        public event Action OnInventoryChanged;

        private void Awake()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].IsUnlocked = i < _startUnlockedSlots;
            }
        }

        private bool Finish(bool changed)
        {
            if (changed)
                OnInventoryChanged?.Invoke();

            return changed;
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

            if (!currency.TrySpend(_slotCosts[index]))
                return false;

            slot.IsUnlocked = true;

            Debug.Log($"Слот {index} разблокирован");
            return Finish(true);
        }

        public bool TryAddItem(ItemData item)
        {
            bool added = false;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;
                if (slot.Stack.Item.Id != item.Id) continue;
                if (slot.Stack.IsFull) continue;

                slot.Stack.Count++;
                added = true;
                break;
            }

            if (!added)
            {
                foreach (var slot in _slots)
                {
                    if (!slot.IsUnlocked || !slot.IsEmpty) continue;

                    slot.Stack = new ItemStack
                    {
                        Item = item,
                        Count = 1
                    };

                    added = true;
                    break;
                }
            }

            return Finish(added);
        }

        public bool TryAddAmmo(ItemData ammo, int amount)
        {
            if (ammo is not AmmoData)
                return false;

            int remaining = amount;
            bool added = false;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;
                if (slot.Stack.Item.Id != ammo.Id) continue;
                if (slot.Stack.IsFull) continue;

                int toAdd = Mathf.Min(ammo.MaxStack - slot.Stack.Count, remaining);

                slot.Stack.Count += toAdd;
                remaining -= toAdd;
                added = true;

                if (remaining <= 0)
                    break;
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
                added = true;

                if (remaining <= 0)
                    break;
            }

            return Finish(added);
        }

        public bool TryShoot()
        {
            bool shot = false;

            foreach (var weaponSlot in _slots)
            {
                if (!weaponSlot.IsUnlocked || weaponSlot.IsEmpty) continue;
                if (weaponSlot.Stack.Item is not WeaponData weapon) continue;

                foreach (var ammoSlot in _slots)
                {
                    if (!ammoSlot.IsUnlocked || ammoSlot.IsEmpty) continue;

                    if (ammoSlot.Stack.Item is AmmoData ammo && ammo.Id == weapon.AmmoId)
                    {
                        ammoSlot.Stack.Count--;

                        if (ammoSlot.Stack.Count <= 0)
                            ammoSlot.Stack.Clear();

                        shot = true;
                        break;
                    }
                }

                if (shot)
                    break;
            }

            if (!shot)
            {
                Debug.Log("Нет оружия с подходящими патронами");
                return false;
            }

            return Finish(true);
        }

        public bool RemoveRandomItem()
        {
            var available = _slots.FindAll(s => s.IsUnlocked && !s.IsEmpty);

            if (available.Count == 0)
                return false;

            var slot = available[Random.Range(0, available.Count)];
            slot.Stack.Clear();
            
            return Finish(true);
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