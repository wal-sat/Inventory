using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BlockItem", fileName = "BI_")]
public class BlockItemData : ItemData
{
    private void OnEnable()
    {
        StackSize = StackSizeType._64;
    }
}
