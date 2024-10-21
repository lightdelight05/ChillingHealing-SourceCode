using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GambleSlotUI : UIBase
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemName;

    public void Init(ItemType itemType)
    {
        _itemName.text = ItemGenerator.Instance.GetDB(itemType).Name;
        _itemImage.sprite = AssetManager.GetItemIconSprite(itemType);
    }
}
