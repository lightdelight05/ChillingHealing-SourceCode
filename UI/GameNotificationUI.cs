﻿using UnityEngine;
using UnityEngine.UI;

public class GameNotificationUI : UIBase
{
    private DimmedUI _dimmed;
    [SerializeField]
    private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(Deactivate);
    }

    public override void Activate()
    {
        base.Activate();
        _dimmed = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmed.SetDimmed(this);
    }

    public override void Deactivate()
    {
        _dimmed.ReturnDimmed(this);
        base.Deactivate();
    }
}
