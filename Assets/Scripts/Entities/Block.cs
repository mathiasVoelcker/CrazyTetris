using System;
using UnityEngine;

public class Block
{
    public Transform BlockObject { get; set; }

    public int Line { get; set; }

    public int Column { get; set; }

    public bool IsSpecial { get; set; }


    public Block(Transform blockObject, int line, int column)
    {
        BlockObject = blockObject;
        Line = line;
        Column = column;
    }

    public bool IsSpecialBlock()
    {
        return BlockObject.GetColor().GetHexString() == "#E295D0";
    }

    public bool IsBombBlock()
    {
        return BlockObject.transform.GetChild(0).Find("bomb") != null;
    }
}
