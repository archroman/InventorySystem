using UnityEngine;

namespace Items.DataScripts
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    internal sealed class WeaponData : ItemData
    {
        public int Damage;
        public string AmmoId;
    }
}