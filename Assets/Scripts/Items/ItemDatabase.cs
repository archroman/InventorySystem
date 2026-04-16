using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Database")]
    internal sealed class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ItemData> _items;

        private List<AmmoData> _ammo;
        private List<ItemData> _equipment;

        private void OnEnable()
        {
            _ammo = _items.OfType<AmmoData>().ToList();

            _equipment = _items
                .Where(i => i is WeaponData || i is HeadData || i is TorsoData)
                .ToList();
        }

        public ItemData GetRandomItem()
        {
            if (_items == null || _items.Count == 0)
            {
                Debug.LogWarning("Items list is empty");
                return null;
            }

            return _items[Random.Range(0, _items.Count)];
        }

        public ItemData GetRandomAmmo()
        {
            if (_ammo == null || _ammo.Count == 0)
            {
                Debug.LogWarning("Ammo list is empty");
                return null;
            }

            return _ammo[Random.Range(0, _ammo.Count)];
        }

        public ItemData GetRandomEquipment()
        {
            if (_equipment == null || _equipment.Count == 0)
            {
                Debug.LogWarning("Equipment list is empty");
                return null;
            }

            return _equipment[Random.Range(0, _equipment.Count)];
        }
    }
}