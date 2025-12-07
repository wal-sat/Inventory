using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorView : MonoBehaviour
{
    [SerializeField] private GameObject _itemNameArea;
    [SerializeField] private TextMeshProUGUI _itemName;

    public void DisplayItemName(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            _itemNameArea.SetActive(false);
            _itemName.gameObject.SetActive(false);
        }
        else
        {
            _itemNameArea.SetActive(true);
            _itemName.gameObject.SetActive(true);

            ItemData itemData = ItemManager.Instance.GetItemData(itemObject.ItemIndex);
            _itemName.text = itemData.Name;
        }
    }
}
