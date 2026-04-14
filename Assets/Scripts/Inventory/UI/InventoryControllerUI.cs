using Items;
using PlayerCurrency;
using UnityEngine;

namespace Inventory.UI
{
    internal sealed class InventoryControllerUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventoryView _view;
        [SerializeField] private CurrencyService _currency;
        [SerializeField] private ItemDatabase _database;

        public void AddCoins()
        {
            int amount = Random.Range(9, 100);
            _currency.AddCoins(amount);

            Debug.Log($"Добавлено ({amount}) монет");
        }

        public void AddItem()
        {
            var item = _database.GetRandomEquipment();

            if (_inventory.TryAddItem(item))
            {
                Debug.Log($"Добавлено {item.name}");
            }

            _view.Refresh();
        }

        public void AddAmmo()
        {
            var ammo = _database.GetRandomAmmo();
            int amount = Random.Range(10, 31);

            if (_inventory.TryAddAmmo(ammo, amount))
            {
                Debug.Log($"Добавлено ({amount}) {ammo.name}");
            }

            _view.Refresh();
        }

        public void RemoveItem()
        {
            if (_inventory.RemoveRandomItem())
            {
                Debug.Log("Удалён предмет");
            }

            _view.Refresh();
        }
    }
}