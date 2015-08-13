using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameStateController : MonoBehaviour
{
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
            if (Input.GetButtonDown("Submit"))
            {
                GameStateManager.ChangeState(GameStates.Playing);
            }
            if (Input.GetButtonDown("Fire3"))
            {
                GameStateManager.ChangeState(GameStates.Building);
            }
        }
        else if (GameStateManager.GameState == GameStates.Playing)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                GameStateManager.ChangeState(GameStates.Building);
            }
        }
        else if (GameStateManager.GameState == GameStates.Building)
        {
            if (Input.GetButton("Fire3") == false)
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
