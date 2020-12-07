using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathTetrisSpawner : SpawnerController
{
    public static List<char> EqOperators = new List<char>() { '<', '>' };

    public static List<int> Numbers = new List<int> { -3, -2, -1, 0, 1, 2, 3 };

    //public decimal MultiplierChances { get; set; } = 0;

    public override Piece CreateNextPiece()
    {
        var rndPiece = GetRndPiece();
        var piece = Instantiate(rndPiece);
        piece.SetButtons();
        piece.transform.localPosition = new Vector3(0, 0, 0);
        piece.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        piece.transform.SetParent(nextPiecePanel.transform, false);
        if (piece.name.Contains("T"))
            SetTValues(piece);
        else if (piece.name.Contains("I"))
            SetIValues(piece);
        else if (piece.name.Contains("L"))
            SetLJValues(piece);
        else if (piece.name.Contains("J"))
            SetLJValues(piece);
        else
            SetPieceValues(piece);

        piece.SetSpawner(this);
        piece.SetGridController(gridController);
        //_nextPiecePanel.transform.DetachChildren();
        return piece;
    }

    private void SetPieceValues(Piece piece)
    {
        CharType charType = GetRndChartype();
        
        foreach (Transform child in piece.transform)
        {
            SetRndValueToBlock(child, charType);
            if (charType == CharType.Operator) charType = CharType.Number;
            else charType = GetRndChartype();
        }
    }

    private void SetTValues(Piece piece)
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);
            }
        }
    }

    private void SetIValues(Piece piece)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i == 1 || i == 2)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);

            }
        }
    }

    private void SetLJValues(Piece piece)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
                SetRndValueToBlock(piece.transform.GetChild(i), CharType.Number);
            else
            {
                CharType charType = GetRndChartype();
                SetRndValueToBlock(piece.transform.GetChild(i), charType);

            }
        }
    }

    private void SetRndValueToBlock(Transform block, CharType charType)
    {
        var textComponent = block.GetTextTransform();
        string value;

        if (charType == CharType.Operator)
        {
            value = EqOperators[UnityEngine.Random.Range(0, EqOperators.Count)].ToString();
        }
        else
        {
            var number = Numbers[UnityEngine.Random.Range(0, Numbers.Count)];
            value = number > 0 ? $"+{number}" : $"{number}";
            //if (MultiplierChances != 0)
            //{
            //    if (MultiplierChances * 10 <= UnityEngine.Random.Range(0, 10))
            //        value = $"*({value})";
            //}
        }

        textComponent.gameObject.GetComponent<Text>().text = value;
    }

    private CharType GetRndChartype()
    {
        return UnityEngine.Random.Range(1, 4) > 2 ? CharType.Operator : CharType.Number;
    }

    public void AddOperator(char op)
    {
        EqOperators.Add(op);
    }

    public void AddNumber(int i)
    {
        Numbers.Add(i);
        Numbers.Add(-i);
    }

}

public enum CharType
{
    Number = 1,
    Operator = 2,
}
