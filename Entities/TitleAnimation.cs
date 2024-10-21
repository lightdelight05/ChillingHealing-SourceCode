using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    private static readonly int[] _skyLayers = { 4, 3, 3, 4 };
    
    private static readonly float _touchToStartTime = 2f;
    private static readonly float _carSpeed = 2000;
    private static readonly float _stopSec = 0.7f;
    private static readonly float _skySpeed = 20;
    
    [SerializeField] private RectTransform _car;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _touchToStart;

    [SerializeField] private Image _skyBG;
    [SerializeField] private Image[] _firstSkyImages;
    [SerializeField] private Image[] _secondSkyImages;
    [SerializeField] private RectTransform _firstSky;
    [SerializeField] private RectTransform _secondSky;
    [SerializeField] private Image _fadeOut;
    
    private Vector2 _helper;
    private float _stopTime;
    private float _touchBtnWaitTime;

    private bool _isTouchTimeDone;
    private bool _isOnMid;

    private float _deltaTime;
    private bool _canLoadScene = false;
    private Vector2 _skyHelper;

    private Color _fadeOutHelper;

    private void Awake()
    {
        _helper = new(1200, 280);
        _stopTime = 0;
        _touchBtnWaitTime = 0;
        _button.enabled = false;
        _touchToStart.enabled = false;
        _button.onClick.AddListener(ChangeScene);

        _skyHelper.x = 0;
        _skyHelper.y = _firstSky.anchoredPosition.y;
    }

    private void Start()
    {
        SetSkyImage();
    }

    private void SetSkyImage()
    {
        AssetManager assetMgr = AssetManager.Instance;
        int skyIdx = new System.Random().Next(_skyLayers.Length);
        string str = $"Sky{skyIdx}_";
        string strWithNum = $"{str}0";
        _skyBG.sprite = assetMgr.LoadAssetSync<Sprite>(strWithNum);
        Sprite sprite;
        for (int i = 1; i <= _skyLayers[skyIdx]; i++)
        {
            strWithNum = $"{str}{i}";
            sprite = assetMgr.LoadAssetSync<Sprite>(strWithNum);
            _firstSkyImages[i - 1].sprite = sprite;
            _secondSkyImages[i - 1].sprite = sprite;
        }

        if (skyIdx == 1 || skyIdx == 2)
        {
            _firstSkyImages[3].enabled = false;
            _secondSkyImages[3].enabled = false;
        }
    }

    private void Update()
    {
        _deltaTime = Time.deltaTime;
        
        // Sky
        _skyHelper.x = _firstSky.anchoredPosition.x - _deltaTime * _skySpeed;
        _firstSky.anchoredPosition = _skyHelper;
        _skyHelper.x = _secondSky.anchoredPosition.x - _deltaTime * _skySpeed;
        _secondSky.anchoredPosition = _skyHelper;

        if (_firstSky.anchoredPosition.x < -1110)
        {
            _skyHelper.x = _secondSky.anchoredPosition.x + (float)2811.4;
            _firstSky.anchoredPosition = _skyHelper;
        }
        else if (_secondSky.anchoredPosition.x < -1110)
        {
            _skyHelper.x = _firstSky.anchoredPosition.x + (float)2811.4;
            _secondSky.anchoredPosition = _skyHelper;
        }
        
        // touch button set active
        _touchBtnWaitTime += _deltaTime;
        if (_touchBtnWaitTime > _touchToStartTime && _isTouchTimeDone == false)
        {
            _button.enabled = true;
            _touchToStart.enabled = true;
            _touchToStart.alpha = 0;
            _isTouchTimeDone = true;
        }
        else
            _touchToStart.alpha += _deltaTime;

        // stop at mid
        if (_car.anchoredPosition.x < 0)
            _isOnMid = true;
        if (_isOnMid)
            _stopTime += _deltaTime;
        if (_stopTime <= _stopSec && _isOnMid)
             return;
        
        // loop
        _helper.x -= _carSpeed * _deltaTime;
        if (_car.anchoredPosition.x < -1200)
        {
            _helper.x = 1200;
            _stopTime = 0;
            _isOnMid = false;
        }
        _car.anchoredPosition = _helper;
    }

    private void ChangeScene()
    {
        if (_canLoadScene)
            return;

        _canLoadScene = true;
        _touchToStart.text = "Loading~";
        _touchToStart.alpha = 1;
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;
            if (asyncLoad.progress >= 0.9f)
            {
                if (_fadeOut.color.a < 1)
                { 
                    _fadeOutHelper = _fadeOut.color;
                    _fadeOutHelper.a += Time.deltaTime;
                    _fadeOut.color = _fadeOutHelper;
                }
                else
                    asyncLoad.allowSceneActivation = true;
            }
        }
    }
}
