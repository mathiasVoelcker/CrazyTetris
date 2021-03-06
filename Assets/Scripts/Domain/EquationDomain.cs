﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EquationDomain
{
    public static bool EquationIsValid(List<Block> equation, bool isHorizontal)
    {
        //var leftSide = new List<string>();
        //var rightSide = new List<string>();

        string eqOperators = "";
        var equationValues = isHorizontal
            ? equation.OrderBy(x => x.Column).Select(x => x.BlockObject).ToList()
            : equation.OrderByDescending(x => x.Line).Select(x => x.BlockObject).ToList();
        var counter = 0;
        decimal leftSideValue = 0, rightSideValue = 0;

        //add values to leftSideEquation until find an operator
        while (!MathTetrisSpawner.EqOperators.Any(x => x.ToString() == equationValues[counter].GetText()))
        {
            AddBlockValue(equationValues[counter], ref leftSideValue);
            counter++;
        }
        //add values to eqOperators until find something that is not an operator
        while (MathTetrisSpawner.EqOperators.Any(x => x.ToString() == equationValues[counter].GetText()))
        {
            var text = equationValues[counter].GetText();
            eqOperators += text;
            counter++;
        }
        //add values to leftSideEquation until the equations end
        while (counter < equationValues.Count)
        {
            AddBlockValue(equationValues[counter], ref rightSideValue);
            counter++;
        }

        return CheckEquation(leftSideValue, eqOperators, rightSideValue);
    }


    private static void AddBlockValue(Transform block, ref decimal equationValue)
    {
        
        var text = block.GetText();
        var value = text.ExtractNumber();
        if (text.Contains('+'))
            equationValue += value;
        else if (text.Contains('-'))
            equationValue -=  value;
    }

    private static bool CheckEquation(decimal leftSideValue, string eqOperators, decimal rightSideValue)
    {
        if (eqOperators.All(x => x == '<'))
            return leftSideValue < rightSideValue;
        if (eqOperators.All(x => x == '>'))
            return leftSideValue > rightSideValue;
        if (eqOperators == "><" || eqOperators == "<>")
            return leftSideValue != rightSideValue;
        if (eqOperators.All(x => x == '='))
            return leftSideValue == rightSideValue;
        if (eqOperators == ">=")
            return leftSideValue >= rightSideValue;
        if (eqOperators == "<=")
            return leftSideValue <= rightSideValue;
        return false;
    }
}
