using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#region Enums

public enum GameStates { Preparing, Playing, Building, Upgrading, Loss }

#endregion

public static class GameStateManager
{
    #region Private Properties

    private const GameStates DEFAULT_STATE = GameStates.Preparing;

    private static Stack<GameStates> _previousStates;

    #endregion

    #region Public Properties

    private static GameStates _gameState;
    public static GameStates GameState
    {
        get
        {
            return _gameState;
        }
        private set
        {
            if (value != _gameState)
            {
                if (GameStateChanging != null)
                {
                    GameStateChanging(null, null);
                }

                _gameState = value;

                if (GameStateChanged != null)
                {
                    GameStateChanged(null, null);
                }
            }
        }
    }

    #endregion

    #region Events

    public static event EventHandler GameStateChanging;
    public static event EventHandler GameStateChanged;

    #endregion

    #region Construction

    static GameStateManager()
    {
        _previousStates = new Stack<GameStates>();
        _gameState = DEFAULT_STATE;
    }

    #endregion

    #region Public Methods

    public static void ChangeState(GameStates newState)
    {
        _previousStates.Push(GameState);
        GameState = newState;
    }

    public static bool RevertState()
    {
        bool result = _previousStates.Count > 0;

        if (result)
        {
            GameState = _previousStates.Pop();
        }

        return result;
    }

    #endregion
}