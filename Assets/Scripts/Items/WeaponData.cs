using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    internal sealed class WeaponData : ItemData
    {
        public int Damage;
        public string AmmoId;
    }
}