using System;
using System.Collections.Generic;
using UnityEngine;

public class MathTetrisGameController : GameController
{
    [SerializeField]
    private GameObject ScorePanel;

    public override void StartGame()
    {
        ScorePanel.SetActive(true);
        base.StartGame();
    }

    public override void ResetGame()
    {
        ScorePanel.SetActive(false);
        base.ResetGame();
    }

    public override void AddScore(List<Block> blocksToRemove, int seqMultiplier = 1)
    {
        var score = ScoreDomain.GetScore(blocksToRemove);
        var newScore = ScoreController.AddScore(score);

        MathTetrisSpawner spawner = (MathTetrisSpawner)SpawnerController;

        if (newScore >= LevelController.ScoreToNextLevel)
        {
            var newLevel = LevelController.AddLevel();
            if (newLevel == 2)
                spawner.AddNumber(4);
            if (newLevel == 3)
                spawner.AddOperator('=');
            if (newLevel == 4)
                spawner.AddNumber(5);
            //if (newLevel == 5)
                //spawner.MultiplierChances = 0.2m;
        }

    }

}
