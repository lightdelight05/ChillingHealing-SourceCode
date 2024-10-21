using UnityEngine;

public class NPCCharacter : MonoBehaviour
{
    [SerializeField] private NPC _NPC;
    [SerializeField] public NPCSpriteAnimation SpriteAnimation;

    public void Awake()
    {
        _NPC.OnMovingChanged += SetMove;
        _NPC.OnDirectionChanged += SetDirection;
        _NPC.OnDragChanged += SetDrag;
    }

    public void Init()
    {
        PartSpritesList _list = PartSpritesList.Instance;
        SpriteAnimation._sprites = _list._NPCdictionary[_NPC.Data.Type].Sprites.ToArray();
        SpriteAnimation.Idle(NPCMotion.Walk);
    }

    public void SetMove(bool isMoving)
    {
        if (isMoving)
            SpriteAnimation.Play(NPCMotion.Walk);
        else
            SpriteAnimation.Idle(NPCMotion.Walk);
    }

    public void SetDirection(Direction direction)
    {
        SpriteAnimation.SetDirection(direction);
    }
    
    public void SetDrag(bool isMoving)
    {
        if (isMoving)
        {
            SpriteAnimation.Play(NPCMotion.Walk);
        }
        else
        {
            SpriteAnimation.Idle(NPCMotion.Walk);
        }
    }
}
