using System.Collections.Generic;
using UnityEngine;
using PlayerCurrency;

namespace Inventory.UI
{
    internal sealed class InventoryView : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventorySlotView _slotPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private CurrencyService _currency;

        private List<InventorySlotView> _views = new();

        private void Start()
        {
            CreateSlots();
            Refresh();
        }

        private void CreateSlots()
        {
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                int index = i;

                var view = Instantiate(_slotPrefab, _content);
                _views.Add(view);

                view.BindUnlock(() =>
                {
                    if (_inventory.TryUnlockSlot(index, _currency))
                        Refresh();
                });
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var slot = _inventory.Slots[i];
                var view = _views[i];

                if (!slot.IsUnlocked)
                {
                    view.ShowLocked(_inventory.GetSlotCost(i));
                    continue;
                }

                if (slot.IsEmpty)
                {
                    view.ShowEmpty();
                    continue;
                }

                view.ShowItem(slot.Stack.Item.Icon, slot.Stack.Count);
            }
        }
    }
}