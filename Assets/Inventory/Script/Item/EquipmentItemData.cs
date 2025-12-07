using UnityEngine;

public enum EquipmentKindType { Helmet, ChestPlate, Leggings, Boots, Shield, Tool, None }

[CreateAssetMenu(menuName = "ScriptableObject/EquipmentItem", fileName = "EI_")]
public class EquipmentData : ItemData
{
    [SerializeField] public EquipmentKindType EquipmentKind;
    [SerializeField] public float Durability;

    private void OnEnable()
    {
        StackSize = StackSizeType._1;
    }
}
