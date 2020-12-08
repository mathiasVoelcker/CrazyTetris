using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DuelTetrisGameController : GameController
{
    [SerializeField]
    private GameObject ScorePanelPlayer1;

    [SerializeField]
    private GameObject ScorePanelPlayer2;

    [SerializeField]
    private GameObject NextPiecePanel;

    private DuelTetrisSpawner DsSpawner {
        get
        {
            return (DuelTetrisSpawner)SpawnerController;
        }
    }

    public static bool isPlayerOneTurn = true;

    public static int turnNumber = 0;

    public override void StartGame()
    {
        ScorePanelPlayer1.SetActive(true);
        ScorePanelPlayer2.SetActive(true);
        RectTransform myRectTransform = NextPiecePanel.GetComponent<RectTransform>();

        myRectTransform.offsetMin = new Vector2(580f /*left*/, 340f /*bottom*/);
        myRectTransform.offsetMax = new Vector2(230f /*right*/, -370f /*top*/);
        base.StartGame();
    }

    public override void ResetGame()
    {
        ScorePanelPlayer1.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "0";
        ScorePanelPlayer1.SetActive(false);
        ScorePanelPlayer2.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "0";
        ScorePanelPlayer2.SetActive(false);
        DsSpawner.FallDelayPlayer1 = 0.8f;
        DsSpawner.FallDelayPlayer2 = 0.8f;
        SpawnerController.Reset();

        RectTransform myRectTransform = NextPiecePanel.GetComponent<RectTransform>();
        myRectTransform.offsetMin = new Vector2(580 /*left*/, 700f /*bottom*/);
        myRectTransform.offsetMax = new Vector2(230f /*right*/, -10f /*top*/);
        IsPaused = false;
        turnNumber = 0;
        isPlayerOneTurn = true;
}

    public override void AddScore(List<Block> blocksToRemove, int seqMultiplier = 1)
    {
        var spawner = DsSpawner;
        var scorePlayer1 = int.Parse(GetScorePlayer1());
        var scorePlayer2 = int.Parse(GetScorePlayer2());
        if (isPlayerOneTurn)
        {
            var newScore = scorePlayer1 + blocksToRemove.Count;
            ScorePanelPlayer1.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "" + newScore;
            scorePlayer1 = newScore;
        }
        else
        {
            var newScore = scorePlayer2 + blocksToRemove.Count;
            ScorePanelPlayer2.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "" + newScore;
            scorePlayer2 = newScore;
        }
        
        spawner.FallDelayPlayer1 = GetNewFallDelay(scorePlayer1, scorePlayer2);
        spawner.FallDelayPlayer2 = GetNewFallDelay(scorePlayer2, scorePlayer1);
        //if (scorePlayer1 != 0 && scorePlayer2 != 0)
        //{
        //    if (scorePlayer1 / scorePlayer2 < 1.3f)
        //        spawner.FallDelayPlayer1 = 0.8f;
        //    if (scorePlayer2 / scorePlayer1 < 1.3f)
        //        spawner.FallDelayPlayer2 = 0.8f;
        //    if (scorePlayer1 / scorePlayer2 > 1.3f)
        //        spawner.FallDelayPlayer1 = 0.8f / (scorePlayer1 / scorePlayer2);
        //    if (scorePlayer2 / scorePlayer1 > 1.3f)
        //        spawner.FallDelayPlayer2 = 0.8f / (scorePlayer2 / scorePlayer1);
        //}

    }

    public override void GameOver()
    {
        var spawner = (DuelTetrisSpawner)SpawnerController;
        if (isPlayerOneTurn)
        {
            var text = GetScorePlayer1();
            var newScore = int.Parse(text) - 50;
            ScorePanelPlayer1.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "" + newScore;
        }
        else
        {
            var text = GetScorePlayer2();
            var newScore = int.Parse(text) - 50;
            ScorePanelPlayer2.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text = "" + newScore;
        }
        base.GameOver();
    }

    private float GetNewFallDelay(int higherScore, int lowerScore)
    {
        if (higherScore <= lowerScore) return 0.8f;
        var diff = higherScore - lowerScore;
        var quo = (100 - diff) / 100.0;
        quo = Math.Pow(quo, 2);
        var fallDelay = quo * 0.8f;
        if (fallDelay < 0.3f)
            return 0.3f;
        return (float)fallDelay;
    }

    private string GetScorePlayer1()
    {
        return ScorePanelPlayer1.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text;
    }

    private string GetScorePlayer2()
    {
        return ScorePanelPlayer2.gameObject.GetComponentsInChildren<Text>().First(x => x.name == "Score").text;
    }

    public override void AddScore(List<Block> blocksToRemove, List<Block> blocksBombed, int seqMultiplier = 1)
    {
        throw new NotImplementedException();
    }
}
