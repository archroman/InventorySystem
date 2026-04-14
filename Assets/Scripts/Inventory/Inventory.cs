using System.Collections.Generic;
using Items;
using PlayerCurrency;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
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
            {
                Debug.LogError("Неверный индекс слота");
                return false;
            }

            var slot = _slots[index];

            if (slot.IsUnlocked)
            {
                Debug.Log("Слот уже открыт");
                return false;
            }

            if (index > 0 && !_slots[index - 1].IsUnlocked)
            {
                Debug.Log("Сначала открой предыдущий слот");
                return false;
            }

            if (index >= _slotCosts.Length)
            {
                Debug.LogError("Нет стоимости для этого слота");
                return false;
            }

            int cost = _slotCosts[index];

            if (!currency.TrySpend(cost))
            {
                Debug.Log("Недостаточно монет");
                return false;
            }

            slot.IsUnlocked = true;

            Debug.Log($"Слот {index} открыт за {cost} монет");
            return true;
        }

        public bool TryAddItem(ItemData item)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || !slot.IsEmpty) continue;

                slot.Stack = new ItemStack()
                {
                    Item = item,
                    Count = 1
                };

                Debug.Log($"Добавлено {item.name} в слот");
                return true;
            }

            Debug.Log("Инвентарь полон");
            return false;
        }

        public bool TryAddAmmo(ItemData ammo, int amount)
        {
            if (!(ammo is AmmoData))
            {
                Debug.Log("Ошибка! Это не патроны");
                return false;
            }

            int remaining = amount;

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || slot.IsEmpty) continue;
                if (slot.Stack.Item != ammo) continue;

                int canAdd = ammo.MaxStack - slot.Stack.Count;
                int toAdd = Mathf.Min(canAdd, remaining);

                slot.Stack.Count += toAdd;
                remaining -= toAdd;

                Debug.Log($"Добавлено ({toAdd}) {ammo.name} в слот");

                if (remaining <= 0)
                    return true;
            }

            foreach (var slot in _slots)
            {
                if (!slot.IsUnlocked || !slot.IsEmpty) continue;

                int toAdd = Mathf.Min(ammo.MaxStack, remaining);

                slot.Stack = new ItemStack()
                {
                    Item = ammo,
                    Count = toAdd
                };

                remaining -= toAdd;

                Debug.Log($"Добавлено ({toAdd}) {ammo.name} в слот");

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

            Debug.Log($"Удалено ({slot.Stack.Count}) {slot.Stack.Item.name}");

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