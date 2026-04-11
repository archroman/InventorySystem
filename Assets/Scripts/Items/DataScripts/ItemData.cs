using UnityEngine;

namespace Items.DataScripts
{
    public class ItemData : ScriptableObject
    {
        public string Id;
        public ItemType Type;
        public float Weight;
        public int MaxStack;
    }
}
