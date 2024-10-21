using System.Collections;
using TMPro;
using UnityEngine;

public class FadeInUI : UIBase
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _loading;


    public void DoFadeIn()
    {
        if (SaveManager.Instance.IsSaveFileLoaded == false)
            SaveManager.Instance.SetNewGame();
        _loading.enabled = false;
        StartCoroutine(FadeIn());
    }

    public void ChangeText(string text)
    {
        _loading.text = text;
    }

    IEnumerator FadeIn()
    {
        while (true)
        {
            yield return null;
            if (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
