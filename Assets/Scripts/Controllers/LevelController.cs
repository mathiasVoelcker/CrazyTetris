using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject LevelText;

    public Dictionary<int, int> ScoreToLevel = new Dictionary<int, int>();

    public int ScoreToNextLevel { get; private set; } = 25;

    public int AddLevel()
    {
        var currentLevel = LevelText.gameObject.GetComponent<Text>().text;
        var nextLevel = int.Parse(currentLevel) + 1;
        LevelText.gameObject.GetComponent<Text>().text = "" + nextLevel;
        ScoreToNextLevel += 25;
        return nextLevel;
    }

    public void Reset()
    {
        LevelText.gameObject.GetComponent<Text>().text = "1";
        ScoreToNextLevel = 25;

    }

}
