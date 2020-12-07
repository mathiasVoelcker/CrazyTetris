using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//TODO: IMPLEMENT LOGIC FOR ADDING 0 IN ROW -1
public class MathTetrisGridController : GridController
{

    public override void CheckBlocks(bool isSequence = false)
    {
        var blocksToRemove = MathGridDomain.CheckEquationsHorizontal(Grid);

        if (blocksToRemove.Any())
        {
            GameController.AddScore(blocksToRemove);
            StartCoroutine(RemoveBlocks(blocksToRemove));
        }
    }
}
