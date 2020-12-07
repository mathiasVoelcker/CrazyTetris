using System;
using UnityEngine;

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
    private MenuController MenuController;

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        GameController.IsPaused = true;
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        GameController.IsPaused = false;
    }

    public void BackToMenu()
    {
        MenuPanel.SetActive(true);
        MenuController.EndGame();
        GridPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        GameController.IsPaused = true;
        GameOverPanel.SetActive(true);
        //MenuController.EndGame();
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
        //var child = GridPanel.transform.FindChild("Block");
    }
}
