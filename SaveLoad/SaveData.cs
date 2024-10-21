using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public PlayerSaveData PlayerSaveData;
    public Dictionary<MapType, MapSaveData> MapDictionary;
    public Dictionary<int, MissionSaveData> MissionDictionary;
    public DateTimeData OfflineStartTime;
}

public class PlayerSaveData
{
    public string Name;
    public float HP;
    public float Cleanliness;
    public int HealingCoin;
    public int MissionCoin;
    public int CampingCarLevel;
    public int BonusSpace;

    public Dictionary<ItemCategory, List<ItemBaseData>> ItemList;

    public Dictionary<Part, ItemType> WearingsData;

    public PlayerSaveData(string Name, float HP, float Cleanliness, int HealingCoin, int MissionCoin,
        int CampingCarLevel,int bonusSpace)
    {
        this.Name = Name;
        this.HP = HP;
        this.Cleanliness = Cleanliness;
        this.HealingCoin = HealingCoin;
        this.MissionCoin = MissionCoin;
        this.CampingCarLevel = CampingCarLevel;
        BonusSpace = bonusSpace;
    }
}

public class ItemBaseData
{
    public ItemType Type;
    public int Index;
    public int Stack;

    public ItemBaseData(ItemType Type, int Index, int Stack)
    {
        this.Type = Type;
        this.Index = Index;
        this.Stack = Stack;
    }
}

public class MapSaveData
{
    public bool IsUnlocked;
    public Dictionary<CampingType, int> CampingLevelList;
    public Dictionary<UnitType, int> NPCLevelList;
    public List<FurnitureSaveData> FurnitureList;

    public MapSaveData(Dictionary<CampingType, int> CampingLevelList, Dictionary<UnitType, int> NPCLevelList, List<FurnitureSaveData> FurnitureList,bool isUnlocked)
    {
        this.CampingLevelList = CampingLevelList;
        this.NPCLevelList = NPCLevelList;
        this.FurnitureList = FurnitureList;
        IsUnlocked = isUnlocked;
    }
}

public class FurnitureSaveData
{
    public ItemType Type;
    public int PosX;
    public int PosY;
    public DirectionType Direction;

    public FurnitureSaveData(ItemType Type, Vector3 Position, DirectionType Direction)
    {
        this.Type = Type;
        this.PosX = (int)Position.x;
        this.PosY = (int)Position.y;
        this.Direction = Direction;
    }
}

public class MissionSaveData
{
    public MissionState MissionState;
    public int Score;
    public int UnlockScore;

    public MissionSaveData(MissionState MissionState, int Score, int UnlockScore)
    {
        this.MissionState = MissionState;
        this.Score = Score;
        this.UnlockScore = UnlockScore;
    }
}

public class DateTimeData
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    public DateTimeData(){}

    public DateTimeData(int Year, int Month, int Day, int Hour, int Minute, int Second)
    {
        this.Year = Year;
        this.Month = Month;
        this.Day = Day;
        this.Hour = Hour;
        this.Minute = Minute;
        this.Second = Second;
    }
}
