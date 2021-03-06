﻿using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject GridPanel;

    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField]
    private GameObject SettingsPanel;

    [SerializeField]
    private MathTetrisGameController _mathTetrisGameController;

    [SerializeField]
    private ColorTetrisGameController _colorTetrisGameController;

    [SerializeField]
    private DuelTetrisGameController _duelTetrisGameController;

    [SerializeField]
    private GamePanelController _gamePanelController;

    [SerializeField]
    private GameObject BackButton;


    public void StartMathTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        BackButton.SetActive(true);
        _mathTetrisGameController.StartGame();
        _gamePanelController.SetGameController(_mathTetrisGameController);
    }

    public void StartColorTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        BackButton.SetActive(true);
        _colorTetrisGameController.StartGame();
        _gamePanelController.SetGameController(_colorTetrisGameController);
    }

    public void StartDuelTetris()
    {
        MenuPanel.SetActive(false);
        GridPanel.SetActive(true);
        BackButton.SetActive(true);
        _duelTetrisGameController.StartGame();
        _gamePanelController.SetGameController(_duelTetrisGameController);
    }

    public void OpenSettings()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void EndGame()
    {
        //_mathTetrisGameController.ResetGame();
        _colorTetrisGameController.ResetGame();
        //_duelTetrisGameController.ResetGame();
    }
}
