using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Head")]
    internal sealed class HeadData : ItemData
    {
        [SerializeField] private int _protection;
        
        public int Protection => _protection;
    }
}