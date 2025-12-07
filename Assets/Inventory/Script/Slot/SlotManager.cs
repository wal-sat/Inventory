using UnityEngine;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<Slot> _hotBarSlots;
    [SerializeField] private List<Slot> _inventorySlots;

    public List<Slot> HotBarSlots { get => _hotBarSlots; }
    public List<Slot> InventorySlots { get => _inventorySlots; }

    // ----- Public Methods -----

    public bool IsHotBarSlot(Slot slot)
    {
        return HotBarSlots.Contains(slot);
    }

    public bool IsInventorySlot(Slot slot)
    {
        return InventorySlots.Contains(slot);
    }
}
