using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighestScoreController : MonoBehaviour 
{
    
    [SerializeField]
    private GameObject HighestScorePannel;

    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField]
    private GameObject TextPlaceholder;

    [SerializeField]
    private List<GameObject> ScoreObjList;

    public GameObject RestartButton;

    private UserScoreService _service;

    private bool isLoading = false;

    public void BackToMenu()
    {
        HighestScorePannel.SetActive(false);
        MenuPanel.SetActive(true);
    }
    
    public void PopulateHighestScore(int userId = -1)
    {
        _service = new UserScoreService();
        List<UserScore> scoreList = new List<UserScore>();
        if (userId == -1)
            scoreList = _service.GetHighestScore();
        else 
            scoreList = _service.GetScores(userId);
        for (int i = 0; i < 6; i++) 
        {
            string user = ""; string score = "";
            if (i < scoreList.Count)
            {
                var userScore = scoreList[i];
                user = string.Format("{0}. {1}", userScore.Position, userScore.Username);
                score = userScore.Score.ToString();
            }
            var userScoreGameObj = ScoreObjList[i].transform;
            userScoreGameObj.Find("user").GetComponent<Text>().text = user;
            userScoreGameObj.Find("score").GetComponent<Text>().text = score;
        }
    }
    
}
