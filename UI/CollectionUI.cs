using System.Collections.Generic;
using UnityEngine;

public abstract class CollectionUI : MenuBase
{
    private static readonly Vector2 _gridCellSize = new(900, 350);
    private static readonly TextAnchor _anchor = TextAnchor.UpperCenter;

    public enum CollectionTab
    {
        Forest, Sea
    }

    public static readonly Dictionary<CollectionTab, string> TabName = new()
    {
        { CollectionTab.Forest, "숲속" },
        { CollectionTab.Sea, "바다" }
    };

    protected MapController _mapController;
    private Player _player;

    private void Awake()
    {
        _mapController = _UIM._mapController;
        _player = _UIM._player;
    }

    public override void Init()
    {
        Tabs = new[] { TabName[CollectionTab.Forest], TabName[CollectionTab.Sea] };
        GridCellSize = _gridCellSize;
        Anchor = _anchor;
    }

    protected void SetSlot(CollectionSlotUI slot, IUpgradeable owner, Sprite sprite)
    {
        slot.SetTargetImage(sprite);
        slot.SetTarget(owner);
        var item = owner.CollectionUIData;

        if (owner.IsUpgradeable == true)
        {
            slot.SetUpgradeEvent(SetUpgradeAction);
        }
        else
        {
            slot.SetUpgradeEvent(null);
        }

        void SetUpgradeAction()
        {
            var data = owner.CollectionUIData;
            if (_player.Data.CanPaid(data.CoinType, data.LevelPrice))
            {
                owner.Upgrade();
                _player.Data.AddCoinAmount(data.CoinType, -data.LevelPrice);
            }
            else
            {
                Debug.Log("실패 UI");
            }
        }
    }
}
