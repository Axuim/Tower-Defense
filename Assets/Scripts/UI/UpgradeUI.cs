using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpgradeUI : MonoBehaviour
{
    #region Private Properties

    private Wall _selection;

    [SerializeField]
    private TowerBuildUI _towerBuildUIPrefab;
    [SerializeField]
    private SetWidthBasedOnChildren _towerBuildUIParent;

    #endregion

    #region Construction

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        GameStateManager.GameStateChanging += this.GameStateChangingHandler;
        GameStateManager.GameStateChanged += this.GameStateChangedHandler;

        //Must be active at start to hook up to events.
        this.Close();
    }

    void OnDestroy()
    {
        GameStateManager.GameStateChanging -= this.GameStateChangingHandler;
        GameStateManager.GameStateChanged -= this.GameStateChangedHandler;
    }

    void OnDisable()
    {
        TowerBuildUI towerBuildUI = null;
        foreach (Transform child in _towerBuildUIParent.transform)
        {
            towerBuildUI = child.GetComponent<TowerBuildUI>();
            if (towerBuildUI != null)
            {
                towerBuildUI.Clicked -= this.TowerBuildUIClickedHandler;
            }
            
            GameObject.Destroy(child.gameObject);
        }
    }

    #endregion

    #region Public Methods

    public void Cancel()
    {
        if (GameStateManager.GameState == GameStates.Upgrading)
        {
            //State check just in case we somehow called from the wrong state since this is public.
            GameStateManager.RevertState();
        }
    }

    #endregion

    #region Private Methods
    
    private bool Show()
    {
        bool result = false;

        this.gameObject.SetActive(true);
        _selection = Wall.Instances.FirstOrDefault(w => w.Selected);

        if (_selection != null)
        {
            TowerBuildUI towerBuildUI = null;
            foreach (var prefab in _selection.Buildables)
            {
                towerBuildUI = GameObject.Instantiate<TowerBuildUI>(_towerBuildUIPrefab);
                towerBuildUI.transform.SetParent(_towerBuildUIParent.transform);
                towerBuildUI.transform.localScale = Vector3.one;
                towerBuildUI.Prefab = prefab;
                towerBuildUI.Clicked += this.TowerBuildUIClickedHandler;
            }

            _towerBuildUIParent.UpdateSize();
            result = _selection.Buildables.Any();
        }

        return result;
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
    }

    #endregion

    #region Event Handlers

    private void GameStateChangingHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Upgrading)
        {
            this.Close();
        }
    }

    private void GameStateChangedHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Upgrading)
        {
            if (this.Show() == false)
            {
                //If there is no selection or no buildables.
                GameStateManager.RevertState();
            }
        }
    }

    private void TowerBuildUIClickedHandler(object sender, EventArgs args)
    {
        if (sender is TowerBuildUI)
        {
            if (_selection != null)
            {
                _selection.BuildTower((sender as TowerBuildUI).Prefab);
            }

            this.Cancel();
        }
    }

    #endregion
}