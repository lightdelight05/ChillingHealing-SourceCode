using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CountPopUpUI : UIBase
{
    private static readonly int Min = 0;
    private static readonly int Max = 99;
    
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _minus;
    [SerializeField] private Button _plus;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _cancelBtn;

    private int _maxValue;
    private int _value;
    
    private DimmedUI _dimmedUI;

    public Action<int> OnConfirmAction;

    private void Awake()
    {
        _minus.onClick.AddListener(() => AddValue(-1));
        _plus.onClick.AddListener(() => AddValue(+1));
        _confirmBtn.onClick.AddListener(Confirm);
        _cancelBtn.onClick.AddListener(Cancel);
    }

    public void Init(int maxValue, string title)
    {
        _dimmedUI = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmedUI.SetDimmed(this);

        _title.text = title;
        ChangeValue(Min);
        _maxValue = maxValue == -1 ? Max : maxValue;
    }

    private void ChangeValue(int newValue)
    {
        _value = newValue;
        _number.text = _value.ToString();
    }
    
    private void AddValue(int diff)
    {
        ChangeValue(math.clamp(_value + diff, Min, _maxValue));
    }

    private void Confirm()
    {
        Deactivate();
        if (_value != 0)
            OnConfirmAction?.Invoke(_value);
        OnConfirmAction = null;
    }

    private void Cancel()
    {
        Deactivate();
        OnConfirmAction = null;
    }

    public override void Deactivate()
    {
        _dimmedUI.ReturnDimmed(this);
        base.Deactivate();
    }
}
