using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialListUI : UIBase
{
    
    [SerializeField] private List<Button> _tutorials = new(9);
    [SerializeField] private Button _exitBtn;

    private DimmedUI _dimmed;
    
    private void Awake()
    {
        _tutorials[0].onClick.AddListener(()=>ShowTutorial((TutorialType)0));
        _tutorials[1].onClick.AddListener(()=>ShowTutorial((TutorialType)1));
        _tutorials[2].onClick.AddListener(()=>ShowTutorial((TutorialType)2));
        _tutorials[3].onClick.AddListener(()=>ShowTutorial((TutorialType)3));
        _tutorials[4].onClick.AddListener(()=>ShowTutorial((TutorialType)4));
        _tutorials[5].onClick.AddListener(()=>ShowTutorial((TutorialType)5));
        _tutorials[6].onClick.AddListener(()=>ShowTutorial((TutorialType)6));
        _tutorials[7].onClick.AddListener(()=>ShowTutorial((TutorialType)7));
        _tutorials[8].onClick.AddListener(()=>ShowTutorial((TutorialType)8));
        
        _exitBtn.onClick.AddListener(Deactivate);
    }
    
    public override void Activate()
    {
        base.Activate();
        _dimmed = UIManager.Instance.GetUI<DimmedUI>(UIName.DimmedUI);
        _dimmed.SetDimmed(this);
    }

    public override void Deactivate()
    {
        _dimmed.ReturnDimmed(this);
        base.Deactivate();
    }

    private void ShowTutorial(TutorialType type)
    {
        var ui = UIManager.Instance.GetUI<TutorialImageUI>(UIName.TutorialImageUI);
        ui.Init(type, false);
    }
}
