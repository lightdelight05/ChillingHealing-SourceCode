using UnityEngine;

public class FurnitureUniversalData
{
    public ItemType Type;
    public int[] Size;
    public bool IsBlocked;
    public bool CanRotation;
    public string FrontSpritePath => $"Furniture{(int)Type}_0";
    public string SideSpritePath => $"Furniture{(int)Type}_1";
}

public class FurnitureData
{
    public FurnitureUniversalData Universal;

    public Vector3 Position;
    public DirectionType Direction;

    public void Load(FurnitureSaveData save,FurnitureUniversalData universal)
    {
        Universal = universal;
        Position = new(save.PosX, save.PosY);
        Direction = save.Direction;
    }
}