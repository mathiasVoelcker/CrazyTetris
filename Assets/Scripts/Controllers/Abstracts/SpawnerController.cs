using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerController : MonoBehaviour, ISpawner
{
    [SerializeField]
    protected GameObject nextPiecePanel;

    [SerializeField]
    protected GameObject gamePanel;

    [SerializeField]
    protected GridController gridController;

    protected Piece nextPiece;

    protected Piece fallingPiece;

    public List<Piece> Pieces;


    public GameObject GetGamePanel()
    {
        return gamePanel;
    }

    public virtual void Run()
    {
        nextPiece = CreateNextPiece();
        SummonNextPiece();
    }

    public void Reset()
    {
        if (nextPiece != null)
        {
            Destroy(nextPiece.gameObject);
            nextPiece = null;
        }
        if (fallingPiece != null)
        {
            Destroy(fallingPiece.gameObject);
            fallingPiece = null;
        }
        gridController.Reset();
    }

    public virtual void SummonNextPiece()
    {
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
            nextPiece.StartFalling();
            fallingPiece = nextPiece;
            nextPiece = CreateNextPiece();
        }
        else
        {
            Destroy(nextPiece.gameObject);
            nextPiece.transform.parent = null;
        }
    }

    public abstract Piece CreateNextPiece();

    protected Piece GetRndPiece()
    {
        var rndPiece = Pieces[Random.Range(0, Pieces.Count)];
        var spawnPosition = transform.position;
        if (rndPiece.name == "O")
        {
            spawnPosition.x += 0.5f;
            spawnPosition.y += 0.5f;
        }
        return rndPiece;
    }
}
