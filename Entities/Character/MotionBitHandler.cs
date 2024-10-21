public class MotionBitConst
{
    public const int FrameBitSize = 5;
    public const int FrameBitLayer = (1 << FrameBitSize) - 1; // 2^5 - 1
    public const float FrameSecond = 0.1f;

    public const int PlayerMaxFrame = 8;
    public const int NPCMaxFrame = 4;
}

public class MotionBitHandler
{
    private int _startIdx;
    private int _frameCount;

    public int FrameCount
    {
        get => _frameCount;
        private set => _frameCount = value;
    }

    public MotionBitHandler(int motionInt)
    {
        _startIdx = motionInt >> MotionBitConst.FrameBitSize;
        _frameCount = motionInt & MotionBitConst.FrameBitLayer;
    }

    public int GetMotionIdxWithDirection(Direction direction, bool isNPC)
    {
        if (_startIdx == (int)Motion.Die)
            return _startIdx;
        int frame = isNPC ? MotionBitConst.NPCMaxFrame : MotionBitConst.PlayerMaxFrame;
        return _startIdx + frame * (int)direction;
    }
}
