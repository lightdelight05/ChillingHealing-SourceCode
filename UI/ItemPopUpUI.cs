using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemPopUpType
{
    InventoryConsumables, // 사용하기, 되팔기
    Inventory, // 되팔기
    Shop // 구매하기
}

public class ItemPopUpUI : UIBase
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDesc;
    [SerializeField] private Button _leftBtn;
    [SerializeField] private TMP_Text _leftBtnText;
    [SerializeField] private Button _rightBtn;
    [SerializeField] private TMP_Text _rightBtnText;
    [SerializeField] private Button _closeBtn;

    private ItemType _type;
    private ItemPopUpType _uiType;
    private int _maxValue;

    private DimmedUI _dimmedUI;
    private UIManager _UIM = UIManager.Instance;

    public Action<int> OnValueChanged;
    public Action<int> OnConsumableUsed;

    private void Awake()
    {
        _leftBtn.onClick.AddListener(LeftButton);
        _rightBtn.onClick.AddListener(RightButton);
        _closeBtn.onClick.AddListener(CloseBtn);
    }

    public void Init(ItemType type, ItemPopUpType uiType, int maxValue = -1)
    {
        _type = type;
        _uiType = uiType;
        _maxValue = maxValue;
        ItemUniversalStatus data = ItemGenerator.Instance.GetDB(_type);
        _itemSprite.sprite = AssetManager.GetItemIconSprite(_type);
        _itemName.text = data.Name;
        _itemDesc.text = data.Desc;

        _dimmedUI = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmedUI.SetDimmed(this);

        InitButtons();
    }

    private void InitButtons()
    {
        _leftBtnText.text = _uiType == ItemPopUpType.Shop ? "구입하기" : "되팔기";
        _rightBtn.gameObject.SetActive(_uiType == ItemPopUpType.InventoryConsumables);
        if (_uiType == ItemPopUpType.InventoryConsumables)
            _rightBtnText.text = "사용하기";
    }

    private void LeftButton()
    {
        if (_uiType == ItemPopUpType.Shop && _maxValue <= 0)
            UIManager.ShowNotificationUI("구매할 수 없습니다.");
        else
        {
            var popUp = _UIM.GetUI<CountPopUpUI>(UIName.CountPopUpUI);
            popUp.Init(_maxValue, _leftBtnText.text);
            popUp.OnConfirmAction = ConfirmLeftButton;
        }
    }

    private void ConfirmLeftButton(int value)
    {
        Deactivate();
        if (value != 0)
            OnValueChanged?.Invoke(value);
        OnValueChanged = null;
    }

    private void RightButton()
    {
        // only consumables
        var popUp = _UIM.GetUI<CountPopUpUI>(UIName.CountPopUpUI);
        popUp.Init(_maxValue, _rightBtnText.text);
        popUp.OnConfirmAction = ConfirmRightButton;
    }

    private void ConfirmRightButton(int value)
    {
        Deactivate();
        if (value != 0)
            OnConsumableUsed?.Invoke(value);
        OnConsumableUsed = null;
    }

    private void CloseBtn()
    {
        Deactivate();
        OnConsumableUsed = null;
    }

    public override void Deactivate()
    {
        _dimmedUI.ReturnDimmed(this);
        base.Deactivate();
    }
}
