using System;

public enum Motion
{
    // start index, required frames
    Walk = 8,           // 0  , 8
    Jump = 1029,        // 32 , 5
    PickUp = 2053,      // 64 , 5
    Carry = 3080,       // 96 , 8
    Sword = 4100,       // 128, 4
    Block = 5121,       // 160, 1
    Hurt = 6145,        // 192, 1
    Die = 7170,         // 224, 2
    Pickaxe = 8197,     // 232, 5
    Axe = 9221,         // 264, 5
    Water = 10242,      // 296, 2
    Hoe = 11266,        // 328, 5
    Fishing = 12293     // 360, 5
}

public enum NPCMotion
{
    Walk = 4,           // 0  , 4
    Sleep = 516,        // 16 , 4
}

public enum Direction
{
    Down,
    Up,
    Right,
    Left
}

[Flags]
public enum Part
{
    Body = 1,
    Eyes = 2,
    Blush = 4,
    Lipstick = 8,
    Top = 16,
    Bottom = 32,
    Overalls = 64,
    Shoes = 128,
    Hair = 256,
    Beard = 512,
    Earring = 1024,
    Mask = 2048,
    Glasses = 4096,
    Hat = 8192
}
