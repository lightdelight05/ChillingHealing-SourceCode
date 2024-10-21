using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CharacterCustomUI : UIBase
{
    enum MannequinSlot
    {
        Hair,
        Glasses,
        BeardRing,
        Hat,
        TopOverall,
        Bottom,
        Shoes
    }
    
    private static readonly int TabCount = 4;
    private static readonly string[] TabName = { "머리", "옷", "신발", "장신구" };

    private static readonly Dictionary<Part, MannequinSlot> _partToMannequin = new()
    {
        { Part.Top, MannequinSlot.TopOverall },
        { Part.Bottom, MannequinSlot.Bottom },
        { Part.Overalls, MannequinSlot.TopOverall },
        { Part.Shoes, MannequinSlot.Shoes },
        { Part.Hair, MannequinSlot.Hair },
        { Part.Beard, MannequinSlot.BeardRing },
        { Part.Earring, MannequinSlot.BeardRing },
        { Part.Glasses, MannequinSlot.Glasses },
        { Part.Hat, MannequinSlot.Hat }
    };

    private Player _player;
    private Character _character;
    private UIManager _UIM;

    private Dictionary<MannequinSlot, ItemType> WearingData = new();
    [SerializeField] public SpriteBtn[] WearingSlots = new SpriteBtn[7];
    [SerializeField] public Image[] WearingMannequin = new Image[7];
    
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Button _saveBtn;
    
    [SerializeField] private TextBtn[] _tabs;
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private SpriteBtn _original;
    
    private List<SpriteBtn> _itemSlots;
    private List<List<ItemType>> _tabItemList;
    private int _currentTab;

    private Action<int> OnMannequinSlotClicked;
    private Action<int> OnSlotClicked;
    private Action OnSaveClicked;

    private void Awake()
    {
        _itemSlots = new();
        _tabItemList = new();
        for (int i = 0; i < TabCount; i++)
        {
            _tabItemList.Add(new());
            _tabs[i].Init(i, TabName[i], ChangeTab);
        }

        OnMannequinSlotClicked += TakeOffSlot;
        OnSlotClicked += TryPutOn;
        OnSaveClicked += PutOn;
        _saveBtn.onClick.AddListener(() => OnSaveClicked?.Invoke());
        _cancelBtn.onClick.AddListener(() => _UIM.ChangeHUDUIMode(HUDChanger.Mode.Inside));
    }

    public void Init()
    {
        _UIM = UIManager.Instance;
        _player = _UIM._player;
        _character = _player._character;

        InitTabItemList();
        InitCustomView();
        
        _currentTab = 0;
        ChangeTab(_currentTab);
    }

    private void InitTabItemList()
    {
        for (int i = 0; i < 4; i++)
        {
            _tabItemList[i].Clear();
        }
        foreach (var item in _player.Inventory.Space.ItemList[ItemCategory.Equipment])
        {
            var type = item.UniversalStatus.Type;
            var part = GetPartOfItem(type);
            if (part == Part.Hair)
                _tabItemList[0].Add(type);
            else if (part == Part.Shoes)
                _tabItemList[2].Add(type);
            else if (part == Part.Top || part == Part.Bottom || part == Part.Overalls)
                _tabItemList[1].Add(type);
            else
                _tabItemList[3].Add(type);
        }
    }

    private void InitCustomView()
    {
        for (int i = 0; i < 7; i++)
        {
            WearingSlots[i].SetSlotData(i, null);
            WearingMannequin[i].color = new Color(1, 1, 1, 0);
        }
        
        foreach (var clothes in _character._wearingsData)
        {
            if (clothes.Key != Part.Body)
            {
                TryPutOn((int)clothes.Value);
            }

        }
    }
    
    private void TryPutOn(int typeInt)
    {
        Stopwatch TEST = Stopwatch.StartNew();
        
        ItemType type = (ItemType)typeInt;
        var part = GetPartOfItem(type);
        
        var idx = (int)_partToMannequin[part];
        TakeOff((int)part);
        
        
        WearingMannequin[idx].sprite = PartSpritesList.Instance.GetFirstFrame(part, type);
        
        
        TEST.Stop();
        Debug.Log(TEST.ElapsedMilliseconds);
        
        
        WearingMannequin[idx].color = new Color(1, 1, 1, 1);
        WearingSlots[idx].SetSlotData(idx, AssetManager.GetItemIconSprite(type));
        
        
        WearingSlots[idx].Init(OnMannequinSlotClicked);

        WearingData.Add((MannequinSlot)idx, type);
        
        if (part == Part.Overalls && WearingData.ContainsKey(MannequinSlot.Bottom))
            TakeOff((int)Part.Bottom);
        if (part == Part.Bottom && WearingData.ContainsKey(MannequinSlot.TopOverall))
        {
            if (GetPartOfItem(WearingData[MannequinSlot.TopOverall]) == Part.Overalls)
                TakeOff((int)Part.Overalls);
        }

        SoundManager.Instance.PlaySE(SoundType.CharacterOutfitSave);
        
    }
    
    private void TakeOff(int partNum)
    {
        Part part = (Part)partNum;
        var manIdx = _partToMannequin[part];
        if (WearingData.ContainsKey(manIdx))
            WearingData.Remove(manIdx);
        WearingSlots[(int)manIdx].SetSlotData((int)manIdx, null);
        WearingMannequin[(int)manIdx].color = new Color(1, 1, 1, 0);
        SoundManager.Instance.PlaySE(SoundType.InteriorTrashcan);
    }

    private void TakeOffSlot(int slotNum)
    {
        if (WearingData.ContainsKey((MannequinSlot)slotNum))
        {
            WearingData.Remove((MannequinSlot)slotNum);
            WearingSlots[slotNum].SetSlotData(slotNum, null);
            WearingMannequin[slotNum].color = new Color(1, 1, 1, 0);
        }
    }

    private void PutOn()
    {
        _character._wearing = Part.Body;
        var newData = new Dictionary<Part, ItemType>();
        newData.Add(Part.Body, ItemType.Skin1);
        foreach (var newClothes in WearingData)
        {
            newData.Add(GetPartOfItem(newClothes.Value), newClothes.Value);
        }

        _character.SetCharacter(newData);
    }

    public Part GetPartOfItem(ItemType type)
    {
        var dict = PartSpritesList.CustomDB;
        foreach (var partItemList in dict)
        {
            foreach (var item in partItemList.Value.ItemTypeList)
            {
                if (type == item)
                    return partItemList.Key;
            }
        }

        return Part.Body;
    }

    private void ChangeTab(int index)
    {
        _currentTab = index;
        RefreshSlots(_tabItemList[index]);
    }

    private void RefreshSlots(List<ItemType> itemList)
    {
        while (_itemSlots.Count <= itemList.Count)
        {
            _itemSlots.Add(GenerateSlot());
        }

        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (i >= itemList.Count)
            {
                _itemSlots[i].gameObject.SetActive(false);
            }
            else
            {
                _itemSlots[i].gameObject.SetActive(true);
                var type = itemList[i];
                _itemSlots[i].SetSlotData(i, AssetManager.GetItemIconSprite(type));
            }
        }
    }
    
    private SpriteBtn GenerateSlot()
    {
        var slot = Instantiate(_original, _slotRoot);
        slot.Init(GetClickedSlotNumber);
        slot.gameObject.SetActive(true);
        return slot;

        void GetClickedSlotNumber(int num)
        {
            OnSlotClicked?.Invoke((int)_tabItemList[_currentTab][num]);
        }
    }
}
