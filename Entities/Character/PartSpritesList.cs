using System.Collections.Generic;
using UnityEngine;

public class PartSpritesList : Singleton<PartSpritesList>
{
    private readonly int _characterFrames = 32;
    private readonly int _NPCFrames = 20;
    private PartSprites _helper = new();
    
    public Dictionary<UnitType, PartSprites> _NPCdictionary = new();

    public static Dictionary<Part, CustomItemTypeList> CustomDB = new();
    
    public void Init()
    {
        GetFirstFrame(Part.Hair, ItemType.BlackBombHair);
        
        List<ItemTypeToPartData> json =
            AssetManager.Instance.DeserializeJsonSync<List<ItemTypeToPartData>>(Const.Json_CharacterCustomDB);

        foreach (var data in json)
        {
            if (CustomDB.ContainsKey(data.Part) == false)
            {
                CustomDB.Add(data.Part, new CustomItemTypeList());
            }

            CustomDB[data.Part].ItemTypeList.Add(data.ItemType);
        }

        InitNPC();
    }

    private void InitNPC()
    {
        for (int i = (int)UnitType.FireCow; i <= (int)UnitType.GoodNight; i++)
        {
            _NPCdictionary.Add((UnitType)i, GetNPCSprites((UnitType)i));
        }
    }

    public Sprite GetFirstFrame(Part part, ItemType type)
    {
        string partNum = $"{part.ToString()}{(int)type}";
        return AssetManager.Instance.LoadAssetSync<Sprite>($"{partNum}[{partNum}_0]");
    }

    public PartSprites GetSpritesFromSingleItem(Part part, ItemType type)
    {
        _helper.Sprites.Clear();
        string partNum = $"{part.ToString()}{(int)type}";
        for (int i = 0; i < _characterFrames; i++)
        {
            _helper.Sprites.Add(AssetManager.Instance.LoadAssetSync<Sprite>($"{partNum}[{partNum}_{i}]"));
        }

        _helper.ItemType = type;

        return _helper;
    }

    public PartSprites GetNPCSprites(UnitType type)
    {
        PartSprites newSprites = new();
        for (int i = 0; i < _NPCFrames; i++)
        {
            string partNum = $"{type.ToString()}";
            Sprite single = AssetManager.Instance.LoadAssetSync<Sprite>($"{partNum}[{partNum}_{i}]");
            newSprites.Sprites.Add(single);
        }

        return newSprites;
    }
}

public class PartSprites
{
    public List<Sprite> Sprites = new(32);
    public ItemType ItemType;
}

public class CustomItemTypeList
{
    public List<ItemType> ItemTypeList = new();
}

public class ItemTypeToPartData
{
    public ItemType ItemType;
    public Part Part;
}
