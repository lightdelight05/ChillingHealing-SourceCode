using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : UIBase
{
    [SerializeField] public Image HealthPointGauge;
    [SerializeField] public Image CleanlinessGauge;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        player.Data.OnStatusChanged += SetFillAmount;
        _player.Data.HP = PlayerData.MaxHP;
        _player.Data.Cleanliness = PlayerData.MaxCleanliness;
        SetFillAmount(StatusType.HP, _player.Data.HP);
        SetFillAmount(StatusType.Cleanliness, _player.Data.Cleanliness);
    }

    public void SetFillAmount(StatusType type, float change)
    {
        switch (type)
        {
            case StatusType.HP:
                HealthPointGauge.fillAmount = change / PlayerData.MaxHP;
                break;
            case StatusType.Cleanliness:
                CleanlinessGauge.fillAmount = change / PlayerData.MaxCleanliness;
                break;
        }
    }

}
