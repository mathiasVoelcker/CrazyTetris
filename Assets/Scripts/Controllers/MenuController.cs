﻿using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject GridPanel;

    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField]
    private MathTetrisGameController _mathTetrisGameController;

    [SerializeField]
    private ColorTetrisGameController _colorTetrisGameController;

    [SerializeField]
    private DuelTetrisGameController _duelTetrisGameController;

    public void StartMathTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        _mathTetrisGameController.StartGame();

    }

    public void StartColorTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        _colorTetrisGameController.StartGame();
    }

    public void StartDuelTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        _duelTetrisGameController.StartGame();
    }

    public void EndGame()
    {
        //_mathTetrisGameController.ResetGame();
        _colorTetrisGameController.ResetGame();
        //_duelTetrisGameController.ResetGame();
    }
}