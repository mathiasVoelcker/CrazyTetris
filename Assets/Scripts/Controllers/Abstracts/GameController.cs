using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameController : MonoBehaviour
{
    [SerializeField]
    protected ScoreController ScoreController;

    [SerializeField]
    protected LevelController LevelController;

    [SerializeField]
    protected SpawnerController SpawnerController;

    //[SerializeField]
    //protected GameObject GridPanel;

    //[SerializeField]
    //protected GameObject PausePanel;

    [SerializeField]
    protected GamePanelController GamePanelController;

    public static bool IsPaused;

    public virtual void StartGame()
    {
        SpawnerController.Run();
    }

    public virtual void ResetGame()
    {
        ScoreController.Reset();
        LevelController.Reset();
        SpawnerController.Reset();
        GamePanelController.ClearPanel();
        IsPaused = false;
    }

    public abstract void AddScore(List<Block> blocksToRemove, int seqMultiplier = 1);

    public abstract void AddScore(List<Block> blocksToRemove, List<Block> blocksBombed, int seqMultiplier = 1);

    public virtual void GameOver()
    {
        GamePanelController.GameOver();
    }
}
