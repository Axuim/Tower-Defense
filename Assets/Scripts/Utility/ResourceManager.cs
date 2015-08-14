using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;

//INotifyPropertyChanged
public static class ResourceManager
{
    #region Private Properties

    private const int INITIAL_MONEY = 10;

    #endregion

    #region Public Properties

    private static int _money;
    public static int Money
    {
        get
        {
            return _money;
        }
        private set
        {
            if (value != _money)
            {
                if (value < 0)
                {
                    _money = 0;
                }
                else
                {
                    _money = value;
                }

                NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region Construction

    static ResourceManager()
    {
        Initialize();

        GameStateManager.GameStateChanging += GameStateChangingHandler;
    }

    #endregion

    #region Public Methods

    public static void GainMoney(int amount)
    {
        Money += amount;
    }

    public static bool LoseMoney(int amount)
    {
        bool result = false;

        if (Money - amount >= 0)
        {
            Money -= amount;
            result = true;
        }

        return result;
    }

    #endregion

    #region Private Methods

    private static void Initialize()
    {
        _money = INITIAL_MONEY;
    }

    #endregion

    #region Event Handlers

    private static void GameStateChangingHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Loss)
        {
            Initialize();
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public static event PropertyChangedEventHandler PropertyChanged;

    private static void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }

    private static void NotifyPropertyChanged()
    {
        var stackTrace = new StackTrace();
        var propertyName = stackTrace.GetFrame(1).GetMethod().Name.Substring(4);

        if (PropertyChanged != null)
        {
            PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion
}