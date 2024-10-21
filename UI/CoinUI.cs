using TMPro;
using UnityEngine;

public class CoinUI : UIBase
{
    [SerializeField] public TextMeshProUGUI HealingCoinText;
    [SerializeField] public TextMeshProUGUI MissionCoinText;

    public void Init(Player player)
    {
        player.Data.OnStatusChanged += SetCoinText;
        SetCoinText(StatusType.HealingCoin, player.Data.HealingCoin);
        SetCoinText(StatusType.MissionCoin, player.Data.MissionCoin);
    }

    public void SetCoinText(StatusType type, float value)
    {
        if (type == StatusType.HealingCoin)
        {
            HealingCoinText.text = ((int)value).ToString();
        }
        else if (type == StatusType.MissionCoin)
        {
            MissionCoinText.text = ((int)value).ToString();
        }
    }
}
