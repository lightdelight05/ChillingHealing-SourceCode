using System.Collections.Generic;

public class OfflineReward
{
    private readonly MapType[] _mapList = { MapType.Forest , MapType.Sea };
    
    private DateTimeData _savedTime;
    private Dictionary<MapType, List<CampingData>> _map = new();

    public OfflineReward(Dictionary<MapType, MapData> map)
    {
        foreach (var mapType in _mapList)
            _map.Add(mapType, map[mapType].Campings);
        _savedTime = SaveManager.Instance._saveData.OfflineStartTime;
        int inGameHour = GetInGameHour();
        if (inGameHour > 0)
        {
            var ui = UIManager.Instance.GetUI<OfflineRewardUI>(UIName.OfflineRewardUI);
            int result = CalculateCoinsPerHour() * inGameHour;
            if (result * 3 / 10 > 0)
                ui.Init(result * 3 / 10);
        }
    }

    private int CalculateCoinsPerHour()
    {
        int result = 0;

        foreach (var map in _map)
        {
            List<CampingData> campings = map.Value;
            foreach (var camp in campings)
            {
                if (camp.Level > 0)
                {
                    camp.Universal.TryGetLevelValue(camp.Level, out LevelData infoData);
                    result += infoData.Coin;
                }
            }
        }

        return result;
    }

    private int GetInGameHour()
    {
        DateTimeData s = _savedTime;
        var savedTime = new System.DateTime(s.Year, s.Month, s.Day, s.Hour, s.Minute, s.Second);
        var curTime = System.DateTime.Now;

        var timeDiff = curTime - savedTime;
        return (int)timeDiff.TotalMinutes;
    }
}
