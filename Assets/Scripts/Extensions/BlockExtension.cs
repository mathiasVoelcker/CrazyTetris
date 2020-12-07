using System;
using UnityEngine;
using UnityEngine.UI;

public static class BlockExtension
{
    public static Transform GetTextTransform(this Transform piece)
    {
        return piece.transform.GetChild(0).GetChild(0);
    }

    public static string GetText(this Transform piece)
    {
        return piece.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text;
    }

    public static Color GetColor(this Transform piece)
    {
        return piece.transform.GetChild(0).transform.GetComponent<Image>().color;
    }

    public static void SetColor(this Transform piece, Color color)
    {
        piece.transform.GetChild(0).transform.GetComponent<Image>().color = color;
    }
}
