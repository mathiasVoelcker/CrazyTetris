using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathTetris.Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.UI;

public class ColorTetrisGridController : GridController
{
    [SerializeField]
    private GameObject MultiplierText;

    [SerializeField]
    private Animator MultiplierAnimator;


    public override void CheckBlocks(bool isSequence = false)
    {
        var gridDomain = new ColorGridDomain(Grid, LINES, COLUMNS);
        var colorTetrisGameController = (ColorTetrisGameController) GameController;
        var blocksToRemove = gridDomain.CheckColors(colorTetrisGameController.pieceSize);

        if (isSequence)
        {
            if (blocksSequence < 3) blocksSequence++;
        }
        else blocksSequence = 1;

        if (blocksToRemove.Any())
        {
            if (blocksSequence > 1)
            {
                MultiplierText.SetActive(true);
                MultiplierText.GetComponent<Text>().text = "x" + blocksSequence;
                MultiplierText.transform.position = blocksToRemove.First().BlockObject.position;
                MultiplierAnimator.Play("MultiplierAnimation", -1, 0f);
                StartCoroutine(MultiplierTextFloat());
            }
            StartCoroutine(RemoveBlocks(blocksToRemove));
        }
    }

    public IEnumerator MultiplierTextFloat()
    {
        int i = 0;
        while (i < 10)
        {
            MultiplierText.transform.position = new Vector3(MultiplierText.transform.position.x, MultiplierText.transform.position.y + 0.1f, MultiplierText.transform.position.z);
            yield return new WaitForSeconds(0.1f);
            i++;
        }
    }
}
