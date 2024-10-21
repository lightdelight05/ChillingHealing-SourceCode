using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Android;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{
    private static readonly string _defaultFileName = "saveFile";
    private string _fileName;

    public SaveData _saveData;
    public bool IsSaveFileLoaded;

    public bool DEBUG_IsSaveFileRemoved;

    public Player _player;
    public MissionBoard _missionBoard;
    public MapController _mapController;

    public void Init()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        _fileName = _defaultFileName;
        IsSaveFileLoaded = false;
        ReadSaveFile();

        DEBUG_IsSaveFileRemoved = false;
    }

    public void ReadSaveFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, _fileName)))
        {

            IsSaveFileLoaded = true;
            string rawData = File.ReadAllText(Path.Combine(Application.persistentDataPath, _fileName));
            _saveData = JsonConvert.DeserializeObject<SaveData>(rawData);
        }
    }

    public void SetNewGame()
    {
        var ui = UIManager.Instance.GetUI<TutorialImageUI>(UIName.TutorialImageUI);
        ui.Init(TutorialType.PlayerMovement, true);
        UIManager.Instance.GetUI<WarningModalUI>(UIName.WarningModalUI);
    }

    public bool IsDayChanged()
    {
        DateTime now = DateTime.Now;
        bool yearDiff = _saveData.OfflineStartTime.Year != now.Year;
        bool monthDiff = _saveData.OfflineStartTime.Month != now.Month;
        bool dayDiff = _saveData.OfflineStartTime.Day != now.Day;

        return yearDiff || monthDiff || dayDiff;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (DEBUG_IsSaveFileRemoved)
            {
                File.Delete(Path.Combine(Application.persistentDataPath, _fileName));
            }
            else
            {
                if (_player == null || _player.Data == null)
                    return;
                SetSaveData();
                WriteSaveFile();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (DEBUG_IsSaveFileRemoved)
        {
            File.Delete(Path.Combine(Application.persistentDataPath, _fileName));
        }
        else
        {
            SetSaveData();
            WriteSaveFile();
        }
    }

    private void SetSaveData()
    {
        PlayerData playerData = _player.Data;
        SaveData newFile = new()
        {
            PlayerSaveData = new PlayerSaveData(playerData.Name, playerData.HP, playerData.Cleanliness, playerData.HealingCoin, playerData.MissionCoin, playerData.CampingCarLevel, _player.Inventory.Space.BonusSpace)
            {
                ItemList = SetInventoryData(),
                WearingsData = _player._character._wearingsData
            },
            MapDictionary = SetMapSaveData(),
            MissionDictionary = SetAllMissionSaveData(),
            OfflineStartTime = SetDateTime()
        };
        _saveData = newFile;
    }

    private Dictionary<ItemCategory, List<ItemBaseData>> SetInventoryData()
    {
        Dictionary<ItemCategory, List<ItemBaseData>> result = new();
        foreach (var itemCategory in _player.Inventory.Space.ItemList)
        {
            List<ItemBaseData> listResult = new();
            foreach (var item in itemCategory.Value)
            {
                listResult.Add(new ItemBaseData(item.UniversalStatus.Type, item.LocalStatus.Index,
                    item.LocalStatus.Stack));
            }

            result.Add(itemCategory.Key, listResult);
        }

        return result;
    }

    public DateTimeData SetDateTime()
    {
        System.DateTime now = System.DateTime.Now;
        return new DateTimeData(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
    }

    private Dictionary<int, MissionSaveData> SetAllMissionSaveData()
    {
        Dictionary<int, MissionSaveData> missionSaveData = new();
        foreach (var data in _missionBoard.Missions)
            missionSaveData.Add(data.ID, new MissionSaveData(data.State, data.Score, -1));
        foreach (var data in _missionBoard.EventMissions)
            missionSaveData.Add(data.ID, new MissionSaveData(data.State, data.Score, data.UnlockScore));
        foreach (var data in _missionBoard.LockEventMissions)
            missionSaveData.Add(data.ID, new MissionSaveData(data.State, data.Score, data.UnlockScore));

        return missionSaveData;
    }

    private Dictionary<MapType, MapSaveData> SetMapSaveData()
    {
        Dictionary<MapType, MapSaveData> mapSaveData = new();
        foreach (var data in _mapController.MapDatas)
        {
            Dictionary<CampingType, int> campingLevelList = new();
            Dictionary<UnitType, int> npcLevelList = new();
            List<FurnitureSaveData> furnitureList = new();
            bool isUnlock = data.Value.IsUnlock;

            foreach (var campingData in data.Value.Campings)
                campingLevelList.Add(campingData.Type, campingData.Level);
            foreach (var npcData in data.Value.Npcs)
                npcLevelList.Add(npcData.Type, npcData.Level);
            foreach (var furnitureData in data.Value.Furnitures)
            {
                furnitureList.Add(new FurnitureSaveData(furnitureData.Universal.Type, furnitureData.Position,
                    furnitureData.Direction));
            }

            mapSaveData.Add(data.Key, new MapSaveData(campingLevelList, npcLevelList, furnitureList, isUnlock));
        }

        return mapSaveData;
    }

    private void WriteSaveFile()
    {
        string rawData = JsonConvert.SerializeObject(_saveData);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, _fileName), rawData);
    }
}
