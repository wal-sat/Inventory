using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MaterialItem", fileName = "MI_")]
public class MaterialItemData : ItemData
{
    private void OnEnable()
    {
        StackSize = StackSizeType._64;
    }
}
