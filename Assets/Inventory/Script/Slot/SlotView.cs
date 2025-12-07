using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI stackCount;
    [SerializeField] private Image durabilityArea;
    [SerializeField] private Image durabilityBar;

    public void SetSlotView(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            image.gameObject.SetActive(false);
            stackCount.gameObject.SetActive(false);
            durabilityArea.gameObject.SetActive(false);
            durabilityBar.gameObject.SetActive(false);
        }
        else
        {
            ItemData itemData = ItemManager.Instance.GetItemData(itemObject.ItemIndex);
            if (itemData == null) return;
            
            image.gameObject.SetActive(true);
            image.sprite = itemData.Icon;
            if (itemData is EquipmentData equipmentData)
            {
                stackCount.gameObject.SetActive(false);

                durabilityArea.gameObject.SetActive(true);
                durabilityBar.gameObject.SetActive(true);
                durabilityBar.fillAmount = (float)itemObject.StackCount / (float)equipmentData.Durability;
            }
            else
            {
                stackCount.gameObject.SetActive(true);
                stackCount.text = itemObject.StackCount.ToString();

                durabilityArea.gameObject.SetActive(false);
                durabilityBar.gameObject.SetActive(false);
            }
        }
    }
}
