using UnityEngine;
using UnityEngine.UI;

public class WarningModalUI : UIBase
{
    private static readonly float _buttonOnTime = 2;
    
    [SerializeField] private Button _btn;
    private float _time;
    private bool _isButtonOn;

    private void Awake()
    {
        _btn.onClick.AddListener(Deactivate);
        _btn.gameObject.SetActive(false);
        _time = 0;
        _isButtonOn = false;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_isButtonOn == false)
        {
            if (_time > _buttonOnTime)
            {
                _btn.gameObject.SetActive(true);
                _isButtonOn = true;
            }
        }
    }
}
