using System.Collections;
using UnityEngine;

public class NPCSpriteAnimation : MonoBehaviour
{
    [SerializeField] public Sprite[] _sprites;
    [SerializeField] private NPCCharacter _character;
    [SerializeField] private SpriteRenderer _renderer;

    private int _startIdx;
    private int _endIdx;
    private int _curIdx;

    private bool _isPlaying;
    private bool _isDirectionChanged;

    private NPCMotion _curMotion;
    private Direction _curDirection = Direction.Down;

    private WaitForSeconds _waitForSeconds = new(MotionBitConst.FrameSecond);
    private WaitForSeconds _NPCWaitForSeconds = new(MotionBitConst.FrameSecond / 2);

    public void SetDirection(Direction direction)
    {
        _curDirection = direction;
        _isDirectionChanged = true;
    }

    public void Idle(NPCMotion motion)
    {
        var bitData = new MotionBitHandler((int)motion);
        int index = bitData.GetMotionIdxWithDirection(_curDirection, true);

        _startIdx = index;

        StopAllCoroutines();
        _isPlaying = false;
        
        if (_renderer.enabled)
            _renderer.sprite = _sprites[index];
    }
    
    public void Play(NPCMotion newMotion)
    {
        var bitData = new MotionBitHandler((int)newMotion);
        int index = bitData.GetMotionIdxWithDirection(_curDirection, true);

        _startIdx = index;
        _endIdx = index + bitData.FrameCount;
        _curIdx = _startIdx;

        if (_curMotion != NPCMotion.Walk || newMotion != NPCMotion.Walk)
        {
            StopAllCoroutines();
            _isPlaying = false;

            if (newMotion == NPCMotion.Walk)
                StartCoroutine(nameof(PlayWalkMotion));
            else
                StartCoroutine(nameof(PlayMotion));
        }
    }
    
    IEnumerator PlayMotion()
    {
        _isPlaying = true;

        for (int curIdx = _startIdx; curIdx < _endIdx; curIdx++)
        {
            if (_renderer.enabled)
                _renderer.sprite = _sprites[curIdx];
            yield return _waitForSeconds;
        }

        _character.SpriteAnimation.Idle(NPCMotion.Walk);
    }

    IEnumerator PlayWalkMotion()
    {
        _isPlaying = true;

        while (true)
        {
            if (_isDirectionChanged)
            {
                var bitData = new MotionBitHandler((int)NPCMotion.Walk);
                int index = bitData.GetMotionIdxWithDirection(_curDirection, true);
                
                int prevIdx = _curIdx;
                int prevStart = _startIdx;

                _startIdx = index;
                _endIdx = index + bitData.FrameCount;

                _curIdx = prevIdx - prevStart + _startIdx;
            }
            if (_renderer.enabled)
               _renderer.sprite = _sprites[_curIdx++];

            yield return _waitForSeconds;

            if (_curIdx >= _endIdx)
                _curIdx = _startIdx;
        }
    }
}
