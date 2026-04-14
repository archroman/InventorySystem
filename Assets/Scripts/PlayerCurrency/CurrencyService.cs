using UnityEngine;

namespace PlayerCurrency
{
    internal sealed class CurrencyService : MonoBehaviour
    {
        [SerializeField] private int _coins;

        public int Coins => _coins;

        public event System.Action<int> OnCoinsChanged;

        private void Awake()
        {
            OnCoinsChanged?.Invoke(_coins);
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0)
                return false;

            if (_coins < amount)
            {
                Debug.LogWarning("Недостаточно монет");
                return false;
            }

            _coins -= amount;
            OnCoinsChanged?.Invoke(_coins);

            Debug.Log($"Списано {amount} монет. Баланс: {_coins}");
            return true;
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0)
                return;

            _coins += amount;
            OnCoinsChanged?.Invoke(_coins);

            Debug.Log($"Добавлено {amount} монет. Баланс: {_coins}");
        }

        public void SetCoins(int amount)
        {
            _coins = Mathf.Max(0, amount);
            OnCoinsChanged?.Invoke(_coins);
        }
    }
}