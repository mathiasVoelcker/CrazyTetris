using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelTetrisSpawner : SpawnerController
{
    public float FallDelayPlayer1 { get; set; } = 0.8f;

    public float FallDelayPlayer2 { get; set; } = 0.8f;

    public override void SummonNextPiece()
    {
        if (DuelTetrisGameController.turnNumber == 2)
        {
            DuelTetrisGameController.isPlayerOneTurn = !DuelTetrisGameController.isPlayerOneTurn;
            DuelTetrisGameController.turnNumber = 1;
        }
        else DuelTetrisGameController.turnNumber++;
        if (nextPiece != null)
            nextPiece._fallDelay = DuelTetrisGameController.isPlayerOneTurn ? FallDelayPlayer1 : FallDelayPlayer2;

        nextPiecePanel.transform.DetachChildren();
        var spawnPosition = transform.localPosition;
        if (nextPiece.transform.name.Contains("O") || nextPiece.transform.name.Contains("I"))
        {
            spawnPosition.x += 30f;
            spawnPosition.y += 30f;
        }
        nextPiece.transform.SetParent(gamePanel.transform, false);
        nextPiece.transform.localPosition = spawnPosition;
        nextPiece.transform.localScale = new Vector3(1, 1, 1);
        if (nextPiece.CheckInitialPosition())
        {
            nextPiece.SetButtons(DuelTetrisGameController.isPlayerOneTurn);
            nextPiece.StartFalling();
            fallingPiece = nextPiece;
            nextPiece = CreateNextPiece();
        }
    }

    public override Piece CreateNextPiece()
    {
        var rndPiece = GetRndPiece();
        var piece = Instantiate(rndPiece);
        piece.transform.localPosition = new Vector3(0, 0, 0);
        piece.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        piece.transform.SetParent(nextPiecePanel.transform, false);

        piece.SetSpawner(this);
        piece.SetGridController(gridController);

        return piece;
    }
}
