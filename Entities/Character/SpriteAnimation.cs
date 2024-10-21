using System.Collections;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Character _character;
    [SerializeField] private SpriteRenderer _renderer;

    private int _startIdx;
    private int _endIdx;
    private int _curIdx;

    private bool _isPlaying;
    private bool _isDirectionChanged;

    private Motion _curMotion;
    private Direction _curDirection = Direction.Down;

    private WaitForSeconds _waitForSeconds = new(MotionBitConst.FrameSecond);

    public void SetRenderer(bool enabled)
    {
        _renderer.enabled = enabled;
    }

    public void SetDirection(Direction direction)
    {
        _curDirection = direction;
        _isDirectionChanged = true;
    }
    
    public void SetSkinSprites(PartSprites partSpritesSO)
    {
        _sprites = partSpritesSO.Sprites.ToArray();
    }

    public void Idle(Motion motion)
    {
        var bitData = new MotionBitHandler((int)motion);
        int index = bitData.GetMotionIdxWithDirection(_curDirection, false);

        _startIdx = index;

        StopAllCoroutines();
        _isPlaying = false;

        if (_renderer.enabled && index < _sprites.Length)
            _renderer.sprite = _sprites[index];
    }

    public void Play(Motion newMotion)
    {
        var bitData = new MotionBitHandler((int)newMotion);
        int index = bitData.GetMotionIdxWithDirection(_curDirection, false);

        _startIdx = index;
        _endIdx = index + bitData.FrameCount;
        _curIdx = _startIdx;

        if (_curMotion != Motion.Walk || newMotion != Motion.Walk)
        {
            StopAllCoroutines();
            _isPlaying = false;

            if (newMotion == Motion.Walk)
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

        _character.Idle();
    }

    IEnumerator PlayWalkMotion()
    {
        _isPlaying = true;

        while (true)
        {
            if (_isDirectionChanged)
            {
                var bitData = new MotionBitHandler((int)Motion.Walk);
                int index = bitData.GetMotionIdxWithDirection(_curDirection, false);
                
                int prevIdx = _curIdx;
                int prevStart = _startIdx;

                _startIdx = index;
                _endIdx = index + bitData.FrameCount;

                _curIdx = prevIdx - prevStart + _startIdx;
            }

            if (_renderer.enabled && _curIdx < _sprites.Length)
                _renderer.sprite = _sprites[_curIdx++];

            yield return _waitForSeconds;

            if (_curIdx >= _endIdx)
                _curIdx = _startIdx;
        }
    }
}
