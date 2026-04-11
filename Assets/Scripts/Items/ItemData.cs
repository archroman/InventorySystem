using UnityEngine;

namespace Items
{
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private float _weight;
        [SerializeField] private int _maxStack;
        [SerializeField] private Sprite _icon;
        
        public string Id => _id;
        public float Weight => _weight;
        public int MaxStack => _maxStack;
        public Sprite Icon => _icon;
    }
}
