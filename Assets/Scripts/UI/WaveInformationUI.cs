using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class WaveInformationUI : MonoBehaviour
{
    #region Private Properties

    [SerializeField]
    private Text _waveNumberText;
    [SerializeField]
    private Text _waveNameText;

    #endregion

    #region MonoBehaviour
    
    void OnEnable()
    {
        GameStateManager.GameStateChanged += this.GameStateChangedHandler;
        WaveController.Singleton.WaveStarted += this.WaveStartedHandler;
    }

    void OnDisable()
    {
        GameStateManager.GameStateChanged -= this.GameStateChangedHandler;
        WaveController.Singleton.WaveStarted -= this.WaveStartedHandler;
    }

    #endregion

    #region Event Handlers

    private void GameStateChangedHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Preparing)
        {
            _waveNumberText.text = string.Empty;
            _waveNameText.text = string.Empty;
        }
    }

    private void WaveStartedHandler(object sender, WaveEventArgs args)
    {
        _waveNumberText.text = args.Number.ToString();
        _waveNameText.text = args.Info.Name;
    }

    #endregion
}
