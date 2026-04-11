using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _slots;

        public List<InventorySlot> Slots => _slots;
    }
}