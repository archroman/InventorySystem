using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    internal sealed class WeaponData : ItemData
    {
        [SerializeField] private int _damage;
        [SerializeField] private string _ammoId;
        
        public int Damage => _damage;
        public string AmmoId => _ammoId;
    }
}