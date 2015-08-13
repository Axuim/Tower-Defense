using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameStateController : MonoBehaviour
{
    #region MonoBehaviour

    void Update()
    {
        if (GameStateManager.GameState == GameStates.None)
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
    }

    #endregion
}
