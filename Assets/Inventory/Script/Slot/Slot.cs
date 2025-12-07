using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private SlotView slotView;

    private ItemObject _itemObject;
    public ItemObject ItemObject
    {
        get => _itemObject;
        set
        {
            _itemObject = value;
            slotView.SetSlotView(value);
        }
    }

    public int DistributedCount { get; set; }

    // ----- Public Methods -----

    public void SetItemCount(int count)
    {
        if (!HasItem()) return;

        ItemObject.StackCount = count;
        ItemObject = ItemObject.StackCount > 0 ? ItemObject : null;
    }

    public int TakeHalf()
    {
        if (!HasItem()) return 0;

        int halfCount = Mathf.FloorToInt((float)ItemObject.StackCount / 2);
        SetItemCount(ItemObject.StackCount - halfCount);
        return halfCount;
    }

    public bool HasItem()
    {
        return _itemObject != null;
    }


    // ----- Test Code -----

    [SerializeField] private int testItemIndex = -1;
    private void Start()
    {
        if (testItemIndex < 0)
        {
            ItemObject = null;
        }
        else
        {
            ItemObject = new ItemObject(testItemIndex, 64);
        }
    }
}
