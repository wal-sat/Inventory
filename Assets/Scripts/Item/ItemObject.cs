
public class ItemObject
{
    public int ItemIndex;
    public int StackCount;

    public ItemObject(int itemIndex, int stackCount)
    {
        ItemIndex = itemIndex;
        StackCount = stackCount;
    }

    // ----- Public Methods -----

    public bool CanAddStackCount()
    {
        ItemData itemData = ItemManager.Instance.GetItemData(ItemIndex);
        if (itemData == null) return false;
        return StackCount < (int)itemData.StackSize;
    }

    public bool IsEquipment()
    {
        ItemData itemData = ItemManager.Instance.GetItemData(ItemIndex);
        return itemData is EquipmentData;
    }

    public EquipmentKindType GetEquipmentType()
    {
        ItemData itemData = ItemManager.Instance.GetItemData(ItemIndex);
        if (itemData is EquipmentData equipmentData)
        {
            return equipmentData.EquipmentKind;
        }
        return EquipmentKindType.None;
    }
}
