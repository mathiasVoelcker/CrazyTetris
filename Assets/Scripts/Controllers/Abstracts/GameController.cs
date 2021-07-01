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

    public static bool IsSoundActive = true;

    public static bool IsPaused;

    public int Score;

    public virtual void StartGame()
    {
        Score = 0;
        SpawnerController.Run();
    }

    public virtual void ResetGame()
    {
        Score = 0;
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
