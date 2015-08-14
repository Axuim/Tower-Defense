using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainController : MonoBehaviour
{
    #region Private Properties

    private Wall _selection;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _wallLayerMask;

    #endregion

    #region MonoBehaviour

    void OnEnable()
    {
        Objective.Instantiated += this.ObjectiveInstantiatedHandler;
        Objective.Destroyed += this.ObjectiveDestroyedHandler;

        foreach (var objective in Objective.Instances)
        {
            objective.Killed += this.ObjectiveKilledHandler;
        }
    }

    void OnDisable()
    {
        Objective.Instantiated -= this.ObjectiveInstantiatedHandler;
        Objective.Destroyed -= this.ObjectiveDestroyedHandler;

        foreach (var objective in Objective.Instances)
        {
            objective.Killed -= this.ObjectiveKilledHandler;
        }
    }

    void Update()
    {
        if (GameStateManager.GameState == GameStates.Preparing)
        {
            if (_selection != null)
            {
                _selection.SetSelected(false);
                _selection = null;
            }

            if (Input.GetButtonDown("Submit"))
            {
                GameStateManager.ChangeState(GameStates.Playing);
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                GameStateManager.ChangeState(GameStates.Building);
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                _selection = this.SelectWall();
                if (_selection != null)
                {
                    _selection.SetSelected(true);
                    GameStateManager.ChangeState(GameStates.Upgrading);
                }
            }
        }
        else if (GameStateManager.GameState == GameStates.Playing)
        {
            if (_selection != null)
            {
                _selection.SetSelected(false);
                _selection = null;
            }

            if (Input.GetButtonDown("Fire3"))
            {
                GameStateManager.ChangeState(GameStates.Building);
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                _selection = this.SelectWall();
                if (_selection != null)
                {
                    _selection.SetSelected(true);
                    GameStateManager.ChangeState(GameStates.Upgrading);
                }
            }
        }
        else if (GameStateManager.GameState == GameStates.Building)
        {
            if (Input.GetButton("Fire3") == false)
            {
                GameStateManager.RevertState();
            }
        }
        else if (GameStateManager.GameState == GameStates.Upgrading)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                GameStateManager.RevertState();
            }
        }
        else if (GameStateManager.GameState == GameStates.Loss)
        {
            if (Input.GetButtonDown("Submit"))
            {
                GameStateManager.ChangeState(GameStates.Preparing);
            }
        }
    }

    #endregion

    #region Private Methods

    private Wall SelectWall()
    {
        Wall result = null;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _wallLayerMask))
        {
            result = hit.collider.GetComponent<Wall>();
        }

        return result;
    }
    #endregion

    #region Event Handlers

    private void ObjectiveInstantiatedHandler(object sender, EventArgs args)
    {
        if (sender is Objective)
        {
            var objective = sender as Objective;
            objective.Killed += this.ObjectiveKilledHandler;
        }
    }

    private void ObjectiveDestroyedHandler(object sender, EventArgs args)
    {
        if (sender is Objective)
        {
            var objective = sender as Objective;
            objective.Killed -= this.ObjectiveKilledHandler;
        }
    }

    private void ObjectiveKilledHandler(object sender, EventArgs args)
    {
        if (Objective.Instances.All(o => o.IsDestroyed))
        {
            //If all objectives are destroyed trigger a loss.
            GameStateManager.ChangeState(GameStates.Loss);
        }
    }

    #endregion
}
