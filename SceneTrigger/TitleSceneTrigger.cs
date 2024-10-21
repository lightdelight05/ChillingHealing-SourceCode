using UnityEngine;

public class TitleSceneTrigger : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<AssetManager>();
        
        var obj = new GameObject("SoundManager").AddComponent<SoundManager>();
        obj.transform.SetParent(transform);
        obj.Init();
        obj.IsOn = true;
        
        SoundManager.Instance.PlayBGM(SoundType.SunnyDay);
    }
}
