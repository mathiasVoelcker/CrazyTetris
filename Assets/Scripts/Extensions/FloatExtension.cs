using System;
using UnityEngine;

public static class FloatExtension
{

    public static byte ToByte(this float val)
    {
        val = Mathf.Clamp01(val);
        return (byte)(val * 255);
    }

}
