using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Database")]
    internal sealed class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ItemData> _items;

        public ItemData GetRandomItem()
        {
            return _items[Random.Range(0, _items.Count)];
        }

        public ItemData GetRandomAmmo()
        {
            var ammo = _items.FindAll(i => i is AmmoData);
            return ammo[Random.Range(0, ammo.Count)];
        }

        public ItemData GetRandomEquipment()
        {
            var list = _items.FindAll(i => i is WeaponData || i is HeadData || i is TorsoData);
            return list[Random.Range(0, list.Count)];
        }
    }
}