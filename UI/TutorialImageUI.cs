using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialType
{
    UITutorial, // 0 UI설명
    PlayerMovement, // 1 캐릭터 이동
    Condition, // 2 캐릭터 컨디션
    NPCInteract, // 3 NPC 상호작용
    Mission, // 4 미션 수행
    StarterGuide, // 5 초보자 가이드
    Clover, // 6 행운 발견하기
    Customization, // 7 캐릭터 외형
    Interior // 8 캠핑카 인테리어
}

public class TutorialImageUI : UIBase
{
    [SerializeField] private Image _tutorialImage;
    [SerializeField] private Button _imageBtn;
    [SerializeField] private Button _prevBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _closeBtn;

    private TutorialType _currentType;
    private bool _isFirstGame;

    private void Awake()
    {
        _currentType = TutorialType.UITutorial;
        _imageBtn.onClick.AddListener(Deactivate);
        _prevBtn.onClick.AddListener(() => ChangeImage(_currentType, -1));
        _nextBtn.onClick.AddListener(() => ChangeImage(_currentType, 1));
        _closeBtn.onClick.AddListener(Deactivate);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (_isFirstGame)
        {
            var ui = UIManager.Instance.GetUI<TutorialImageUI>(UIName.TutorialImageUI);
            ui.Init(TutorialType.UITutorial, false);
        }
    }

    public void Init(TutorialType type, bool isFirstGame)
    {
        _isFirstGame = isFirstGame;
        ChangeImage(type, 0);
    }

    public void RandomImage()
    {
        _isFirstGame = false;
        int idx = new System.Random().Next(1, 9);
        _tutorialImage.sprite = AssetManager.Instance.LoadAssetSync<Sprite>($"Tutorial{idx}");
        _imageBtn.enabled = false;
        _prevBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(false);
        _closeBtn.gameObject.SetActive(false);
    }

    private void ChangeImage(TutorialType t, int prevOrNext)
    {
        var type = (TutorialType)((int)t + prevOrNext);
        _currentType = type;
        _imageBtn.enabled = type == TutorialType.UITutorial;
        _prevBtn.gameObject.SetActive(type != (TutorialType)1 && type != TutorialType.UITutorial);
        _nextBtn.gameObject.SetActive(type != (TutorialType)8 && type != TutorialType.UITutorial);
        _closeBtn.gameObject.SetActive(type != TutorialType.UITutorial && (_isFirstGame == false ||
                                       (_isFirstGame && type == (TutorialType)8)));
        _tutorialImage.sprite = AssetManager.Instance.LoadAssetSync<Sprite>($"Tutorial{(int)type}");
    }
}
