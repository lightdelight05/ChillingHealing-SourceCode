using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventMissionCompleteUI : UIBase
{
    [SerializeField] private Button _panel;
    [SerializeField] private Image _NPCLookingRight;
    [SerializeField] private TMP_Text _dialog;

    private DimmedUI _dimmedUI;
    
    private void Awake()
    {
        _panel.onClick.AddListener(Deactivate);
    }

    public void Init(UnitType npc, string text)
    {
        _dimmedUI = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmedUI.SetDimmed(this);
        string npcName = $"{npc.ToString()}";
        _NPCLookingRight.sprite =
            AssetManager.Instance.LoadAssetSync<Sprite>($"{npcName}[{npcName}_{4 * (int)Direction.Right}]");
        _dialog.text = text;
    }

    public override void Deactivate()
    {
        _dimmedUI.ReturnDimmed(this);
        base.Deactivate();
    }
}
