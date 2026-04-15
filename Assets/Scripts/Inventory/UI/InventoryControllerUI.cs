using System;
using Items;
using PlayerCurrency;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.UI
{
    internal sealed class InventoryControllerUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventoryView _view;
        [SerializeField] private CurrencyService _currency;
        [SerializeField] private ItemDatabase _database;

        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _weightText;

        private void OnEnable()
        {
            _currency.OnCoinsChanged += UpdateCoins;
        }

        private void OnDisable()
        {
            _currency.OnCoinsChanged -= UpdateCoins;
        }

        private void Start()
        {
            UpdateCoins(_currency.Coins);
            UpdateWeight();
        }

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

            UpdateWeight();
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

            UpdateWeight();
            _view.Refresh();
        }

        public void RemoveItem()
        {
            if (_inventory.RemoveRandomItem())
            {
                Debug.Log("Удалён предмет");
            }

            UpdateWeight();
            _view.Refresh();
        }

        public void Shoot()
        {
            _inventory.TryShoot();

            UpdateWeight();
            _view.Refresh();
        }

        private void UpdateCoins(int coins)
        {
            _coinsText.text = $"Монеты: {coins}";
        }

        private void UpdateWeight()
        {
            float weight = _inventory.GetTotalWeight();
            _weightText.text = $"Вес: {weight:0.000}";
        }
    }
}