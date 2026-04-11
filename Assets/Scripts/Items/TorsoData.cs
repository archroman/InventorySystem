using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Torso")]
    internal sealed class TorsoData : ItemData
    {
        [SerializeField] private int _protection;
        
        public int Protection => _protection;
    }
}