using System;
using UnityEngine;

public static class ColorExtension
{
    public static string GetHexString(this Color color)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", color.r.ToByte(), color.g.ToByte(), color.b.ToByte());
    }
}
