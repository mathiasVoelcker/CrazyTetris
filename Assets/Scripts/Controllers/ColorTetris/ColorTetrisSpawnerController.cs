using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorTetrisSpawnerController : SpawnerController
{
    public static List<string> Colors = new List<string>() { "#E2AD52", "#E26E52", "#96CF52", "#52ADD0", "#E295D0", "#E2E252" };

    public static List<string> ActiveColors = new List<string>() { "#E2AD52", "#E26E52", "#96CF52" };

    public static readonly List<string> StartColors = new List<string>() { "#E2AD52", "#E26E52", "#96CF52" };

    public List<Piece> StaticPieces;

    public List<Piece> StartPieces;

    public Sprite BombSprite;

    public static float BombProb = 0f;

    public override void Run()
    {
        Pieces.Clear();
        Pieces.AddRange(StartPieces);
        base.Run();
    }

    public override Piece CreateNextPiece()
    {
        var rndPiece = GetRndPiece();
        var piece = Instantiate(rndPiece);
        piece.SetButtons();
        piece.transform.localPosition = new Vector3(0, 0, 0);
        piece.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        piece.transform.SetParent(nextPiecePanel.transform, false);
        SetPieceColors(piece);

        piece.SetSpawner(this);
        piece.SetGridController(gridController);

        //Destroy(transform.gameObject);

        return piece;
    }

    public static void AddNextColor()
    {
        foreach (var color in Colors)
        {
            if (ActiveColors.Any(x => x == color)) continue;
            else
            {
                ActiveColors.Add(color);
                break;
            }
        }
    }

    public void AddStaticPieceToPieces(string name)
    {
        var piece = StaticPieces.First(x => x.name.Contains(name));
        Pieces.Add(piece);
    }

    private void SetPieceColors(Piece piece)
    {
        var colorTemp = new List<string>();
        colorTemp.AddRange(ActiveColors);
        foreach (Transform child in piece.transform)
        {
            if (colorTemp.Count() == 0) colorTemp.AddRange(ActiveColors);
            var colorHex = GetRndColor(colorTemp);
            Color color;
            if (ColorUtility.TryParseHtmlString(colorHex, out color))
                child.transform.GetChild(0).transform.GetComponent<Image>().color = color;
            if (HasBomb())
                AddBomb(child.transform.GetChild(0));

            colorTemp.Remove(colorHex);
        }
    }


    private string GetRndColor(List<string> colorList) 
    {
        var colorHex = colorList[UnityEngine.Random.Range(0, colorList.Count)];
        return colorHex;
    }

    private void AddBomb(Transform block)
    {
        var bomb = new GameObject("bomb");
        bomb.AddComponent<SpriteRenderer>();
        bomb.GetComponent<SpriteRenderer>().sprite = BombSprite;
        bomb.transform.SetParent(block.transform);
        bomb.transform.localPosition = new Vector3(0, 0, 0);
        bomb.transform.localScale = new Vector3(10, 10, 10);
    }

    private bool HasBomb()
    {
        if (BombProb == 0f) return false;
        var rndInt = UnityEngine.Random.Range(0, 100);
        return rndInt < BombProb * 100;
    }

}
