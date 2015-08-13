using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Collider))]
public class Wall : MonoBehaviour
{
    #region Private Properties

    private Collider _collider;

    #endregion

    #region Public Properties

    public const float SIZE = 1.0f;
    public const float HALF_SIZE = SIZE * 0.5f;

    private static List<Wall> _instances = new List<Wall>();
    public static IEnumerable<Wall> Instances
    {
        get
        {
            return _instances;
        }
    }

    #endregion

    #region Events

    public static EventHandler Instantiated;
    public static EventHandler Destroyed;
    public static EventHandler PathUpdated;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _instances.Add(this);
        
        if (Instantiated != null)
        {
            Instantiated(this, null);
        }

        _collider = this.GetComponent<Collider>();
    }

    void Start()
    {
        if (AstarPath.active != null)
        {
            AstarPath.active.UpdateGraphs(_collider.bounds);
            
            if (PathUpdated != null)
            {
                PathUpdated(this, null);
            }
        }
    }

    void OnDestroy()
    {
        _instances.Remove(this);

        if (AstarPath.active != null)
        {
            AstarPath.active.UpdateGraphs(_collider.bounds);

            if (PathUpdated != null)
            {
                PathUpdated(this, null);
            }
        }

        if (Destroyed != null)
        {
            Destroyed(this, null);
        }
    }

    #endregion
}