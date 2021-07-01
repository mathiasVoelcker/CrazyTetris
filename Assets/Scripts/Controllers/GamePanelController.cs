using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField]
    private GameObject GridPanel;

    [SerializeField]
    private GameObject GameOverPanel;

    [SerializeField]
    private GameObject HighestScorePanel;

    [SerializeField]
    private MenuController MenuController;

    [SerializeField]
    private HighestScoreController HighestScoreController;


    [SerializeField]
    private GameObject BackButton;

    private GameController _gameController;

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        GameController.IsPaused = true;
        BackButton.SetActive(false);
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        GameController.IsPaused = false;
        BackButton.SetActive(true);
    }

    public void BackToMenu()
    {
        MenuController.EndGame();
        GridPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        BackButton.SetActive(false);
        HighestScorePanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void GameOver()
    {
        GameController.IsPaused = true;
        GameOverPanel.SetActive(true);

        //MenuController.EndGame();
    }

    public void SetGameController(GameController gameController)
    {
        _gameController = gameController;
    }

    public void Restart()
    {
        GameOverPanel.SetActive(false);
        HighestScorePanel.SetActive(false);
        GridPanel.SetActive(true);
        BackButton.SetActive(true);
        _gameController.ResetGame();
        _gameController.StartGame();
    }

    public void ClearPanel()
    {
        foreach (Transform child in GridPanel.transform)
        {
            if (child.name.Contains("Clone"))
                Destroy(child.gameObject);
            else if (child.name.Contains("Block"))
                Destroy(child.gameObject);
        }
    }

    public void SaveScore()
    {
        var service = new UserScoreService();
        var username = GameOverPanel
            .transform
            .Find("Username Input")
            .Find("Text")
            .GetComponent<Text>().text;
        int id = service.SaveScore(username, _gameController.Score);
        
        MenuController.EndGame();
        GridPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        BackButton.SetActive(false);
        HighestScorePanel.SetActive(true);
        HighestScoreController.RestartButton.SetActive(true);
        HighestScoreController.PopulateHighestScore(id);
    }
}
