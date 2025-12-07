using UnityEngine;
using System.Collections.Generic;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private List<ItemData> _itemDataList;

    public ItemData GetItemData(int index)
    {
        if (index < 0 || index >= _itemDataList.Count)
        {
            return null;
        }
        return _itemDataList[index];
    }

    public int GetItemIndex(ItemData itemData)
    {
        return _itemDataList.IndexOf(itemData);
    }

    // ----- テスト用 ランダム取得メソッド -----

    private List<EquipmentData> equipmentDataList = new List<EquipmentData>();
    private List<MaterialItemData> materialItemDataList = new List<MaterialItemData>();
    private List<BlockItemData> blockItemDataList = new List<BlockItemData>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var itemData in _itemDataList)
        {
            if (itemData is EquipmentData equipmentData)
            {
                equipmentDataList.Add(equipmentData);
            }
            else if (itemData is MaterialItemData materialItemData)
            {
                materialItemDataList.Add(materialItemData);
            }
            else if (itemData is BlockItemData blockItemData)
            {
                blockItemDataList.Add(blockItemData);
            }
        }
    }

    public int GetRandomEquipmentIndex()
    {
        if (equipmentDataList.Count == 0) return -1;
        var randomEquipment = equipmentDataList[Random.Range(0, equipmentDataList.Count)];
        return GetItemIndex(randomEquipment);
    }
    public int GetRandomMaterialItemIndex()
    {
        if (materialItemDataList.Count == 0) return -1;
        var randomMaterialItem = materialItemDataList[Random.Range(0, materialItemDataList.Count)];
        return GetItemIndex(randomMaterialItem);
    }
    public int GetRandomBlockItemIndex()
    {
        if (blockItemDataList.Count == 0) return -1;
        var randomBlockItem = blockItemDataList[Random.Range(0, blockItemDataList.Count)];
        return GetItemIndex(randomBlockItem);
    }

}

