using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Wall : MonoBehaviour
{
    #region Events

    public static EventHandler Instantiated;
    public static EventHandler Destroyed;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        if (AstarPath.active != null)
        {
            AstarPath.active.UpdateGraphs(this.GetComponent<Collider>().bounds);
        }

        if (Instantiated != null)
        {
            Instantiated(this, null);
        }
    }

    void OnDestroy()
    {
        if (AstarPath.active != null)
        {
            AstarPath.active.UpdateGraphs(this.GetComponent<Collider>().bounds);
        }

        if (Destroyed != null)
        {
            Destroyed(this, null);
        }
    }

    #endregion
}