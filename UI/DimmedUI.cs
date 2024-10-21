using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimmedUI : UIBase
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _panel;

    private readonly Stack<UIBase> _originStack = new();
    private Action _panelClicked;

    private void Awake()
    {
        _panel.onClick.AddListener(() => _panelClicked?.Invoke());
    }

    public void SetDimmed(UIBase originUI)
    {
        var originSortingOrder = originUI.GetComponentInParent<Canvas>();
        if (originSortingOrder != null)
        {
            _canvas.sortingOrder = originUI.GetComponentInParent<Canvas>().sortingOrder - 1;
            _originStack.Push(originUI);
            _panelClicked = originUI.Deactivate;
        }
    }

    public void ReturnDimmed(UIBase tryingReturn)
    {
        if (_originStack.TryPeek(out UIBase origin))
        {
            if (origin == tryingReturn)
                _originStack.Pop();
            
            if (_originStack.TryPeek(out var prevUI))
            {
                _originStack.Pop();
                SetDimmed(prevUI);
                return;
            }
            
            // Dimmed 더 없을경우
            Deactivate();
        }
    }
}
