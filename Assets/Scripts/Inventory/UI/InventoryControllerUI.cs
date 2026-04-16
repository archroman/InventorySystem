using Items;
using PlayerCurrency;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Inventory.UI.Utils;

namespace Inventory.UI
{
    internal sealed class InventoryControllerUI : MonoBehaviour
    {
        private const int MinCoinsAddAmount = 9;
        private const int MaxCoinsAddAmount = 100;
        
        private const int MinAmmoAddAmount = 10;
        private const int MaxAmmoAddAmount = 31;
        
        [SerializeField] private Inventory _inventory;
        [SerializeField] private CurrencyService _currency;
        [SerializeField] private ItemDatabase _database;

        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _weightText;

        private void OnEnable()
        {
            _currency.OnCoinsChanged += UpdateCoins;
            _inventory.OnInventoryChanged += UpdateWeight;
        }

        private void OnDisable()
        {
            _currency.OnCoinsChanged -= UpdateCoins;
            _inventory.OnInventoryChanged -= UpdateWeight;
        }

        private void Start()
        {
            UpdateCoins(_currency.Coins);
            UpdateWeight();
        }

        public void AddCoins()
        {
            int amount = Random.Range(MinCoinsAddAmount, MaxCoinsAddAmount);
            _currency.AddCoins(amount);
        }

        public void AddItem()
        {
            var item = _database.GetRandomEquipment();
            _inventory.TryAddItem(item);
        }

        public void AddAmmo()
        {
            var ammo = _database.GetRandomAmmo();
            int amount = Random.Range(MinAmmoAddAmount, MaxAmmoAddAmount);
            _inventory.TryAddAmmo(ammo, amount);
        }

        public void RemoveItem()
        {
            _inventory.RemoveRandomItem();
        }

        public void Shoot()
        {
            _inventory.TryShoot();
        }

        private void UpdateCoins(int coins)
        {
            _coinsText.text = $"Монеты: {NumberFormatter.Format(coins)}";
        }

        private void UpdateWeight()
        {
            float weight = _inventory.GetTotalWeight();
            _weightText.text = $"Вес: {weight:0.000}";
        }
    }
}