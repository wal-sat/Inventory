using UnityEngine;

public enum StackSizeType { _1 = 1, _16 = 16, _64 = 64 }

public class ItemData : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public Sprite Icon;
    [HideInInspector] public StackSizeType StackSize;
}
