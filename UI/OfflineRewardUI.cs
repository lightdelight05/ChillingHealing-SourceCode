using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineRewardUI : UIBase
{
    [SerializeField] private TMP_Text _rewardCoinsText;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _doubleWithAdBtn;

    private int _rewardCoins;

    private void Awake()
    {
        _confirmBtn.onClick.AddListener(() => Confirm(_rewardCoins));
        _doubleWithAdBtn.onClick.AddListener(() => AdMobManager.Instance.ShowRewardedAd(WatchAd, () => { Confirm(_rewardCoins); }));
    }

    public void Init(int rewardCoins)
    {
        _rewardCoins = rewardCoins;
        _rewardCoinsText.text = $"{Const.TextEmoji_HealingCoin} {rewardCoins}";
    }

    private void Confirm(int rewardCoins)
    {
        UIManager.Instance._player.Data.AddCoinAmount(CoinType.Healing, rewardCoins);
        Deactivate();
    }

    private void WatchAd()
    {
        Confirm(_rewardCoins * 2);
    }
}
