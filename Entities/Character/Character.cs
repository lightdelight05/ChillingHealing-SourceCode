using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Character : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] public SpriteAnimation[] SpriteAnimations = new SpriteAnimation[14];

    public Dictionary<Part, ItemType> _wearingsData;
    public Part _wearing;

    public void Awake()
    {
        _player.OnMovingChanged += SetMove;
        _player.OnDirectionChanged += SetDirection;
    }

    public void Init()
    {
        SaveManager saveManager = SaveManager.Instance;
        var wearings = saveManager.IsSaveFileLoaded
            ? saveManager._saveData.PlayerSaveData.WearingsData
            : GetRandomBasicClothes();
        SetCharacter(wearings);
    }

    private Dictionary<Part, ItemType> GetRandomBasicClothes()
    {
        ItemType hair = _randomHair[new Random().Next(_randomHair.Length)];
        ItemType top = _randomTop[new Random().Next(_randomTop.Length)];
        ItemType bottom = _randomBottom[new Random().Next(_randomBottom.Length)];
        ItemType shoes = _randomShoes[new Random().Next(_randomShoes.Length)];

        UIManager.Instance._player.AddItem(hair, 1);
        UIManager.Instance._player.AddItem(top, 1);
        UIManager.Instance._player.AddItem(bottom, 1);
        UIManager.Instance._player.AddItem(shoes, 1);

        return new Dictionary<Part, ItemType>
        {
            { Part.Body, ItemType.Skin1 }, { Part.Hair, hair }, { Part.Top, top },
            { Part.Bottom, bottom }, { Part.Shoes, shoes }
        };
    }

    public void SetCharacter(Dictionary<Part, ItemType> wearingsData)
    {
        _wearingsData = wearingsData;
        _wearing = Part.Body;
        foreach (KeyValuePair<Part, ItemType> part in _wearingsData)
        {
            PutOn(part.Key, part.Value);
        }
        RefreshWearing();
        Idle();
    }

    public void RefreshWearing()
    {
        foreach (Part part in Enum.GetValues(typeof(Part)))
        {
            SpriteAnimations[(int)Math.Log((int)part, 2)].SetRenderer(isWearing(part));
        }

        Idle();
    }

    public void PutOn(Part part, ItemType type)
    {
        _wearing |= part;
        if (part == Part.Overalls)
            TakeOff(Part.Top ^ Part.Bottom);
        else if (part == Part.Beard)
            TakeOff(Part.Earring);
        else if (part == Part.Earring)
            TakeOff(Part.Beard);
        if (_wearingsData.ContainsKey(part))
        {
            if (_wearingsData[part] != type)
                _wearingsData[part] = type;
        }
        else
            _wearingsData.Add(part, type);
        SetPartSprites(part, PartSpritesList.Instance.GetSpritesFromSingleItem(part, type));
        RefreshWearing();
        SoundManager.Instance.PlaySE(SoundType.CharacterOutfitEquip);
    }

    public void TakeOff(Part part)
    {
        _wearing ^= part;
        if (_wearingsData.ContainsKey(part))
        {
            _wearingsData.Remove(part);
        }
        RefreshWearing();
    }

    public bool isWearing(Part part)
    {
        return (_wearing & part) == part;
    }
    
    public void SetPartSprites(Part part, PartSprites partSprites)
    {
        SpriteAnimations[(int)Math.Log((int)part, 2)].SetSkinSprites(partSprites);
    }
    
    public void SetMove(bool isMoving)
    {
        if (isMoving)
            Play(Motion.Walk);
        else
            Idle();
    }

    public void SetDirection(Direction direction)
    {
        for (int i = 0; i < SpriteAnimations.Length; i++)
        {
            if (SpriteAnimations[i] != null)
                SpriteAnimations[i].SetDirection(direction);
        }
    }

    public void Play(Motion motion)
    {
        for (int i = 0; i < SpriteAnimations.Length; i++)
        {
            if (SpriteAnimations[i] != null)
            {
                SpriteAnimations[i].Play(motion);
            }
        }
    }

    public void Idle()
    {
        for (int i = 0; i < SpriteAnimations.Length; i++)
        {
            if (SpriteAnimations[i] != null)
                SpriteAnimations[i].Idle(Motion.Walk);
        }
    }
}
