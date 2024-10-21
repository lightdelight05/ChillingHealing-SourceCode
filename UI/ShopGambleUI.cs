using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGambleUI : UIBase
{
    [SerializeField] private GridLayoutGroup _slotParent;
    [SerializeField] private Button _confirmButton;

    private List<GambleSlotUI> _slots = new();
    
    private UIManager _UIM = UIManager.Instance;
    private DimmedUI _dimmedUI;

    private void Awake()
    {
        _confirmButton.onClick.AddListener(Deactivate);
    }

    public void Init(List<ItemType> gambleRewards)
    {
        _dimmedUI = _UIM.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmedUI.SetDimmed(this);

        foreach (var item in gambleRewards)
        {
            var slot = _UIM.GetPooledUI<GambleSlotUI>(PooledUIName.GambleSlotUI);
            slot.Init(item);
            slot.transform.SetParent(_slotParent.gameObject.transform, false);
            _slots.Add(slot);
        }
    }

    public override void Deactivate()
    {
        _dimmedUI.ReturnDimmed(this);
        base.Deactivate();
        foreach (var slot in _slots)
        {
            slot.Deactivate();
        }
    }
}
