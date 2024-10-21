using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionAcceptDialogueUI : UIBase
{
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public Button Button;
    [SerializeField] private Image _NPCLookingLeft;

    private DimmedUI _dimmedUI;

    public void Init(string dialog, UnitType npcType)
    {
        Text.text = dialog;
        string npcName = $"{npcType.ToString()}";
        _NPCLookingLeft.sprite =
            AssetManager.Instance.LoadAssetSync<Sprite>($"{npcName}[{npcName}_{4 * (int)Direction.Left}]");
        _dimmedUI = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmedUI.SetDimmed(this);
    }

    public override void Deactivate()
    {
        _dimmedUI.ReturnDimmed(this);
        base.Deactivate();
        Button.onClick.RemoveAllListeners();
    }
}
