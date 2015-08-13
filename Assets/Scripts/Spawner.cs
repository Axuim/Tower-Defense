using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour
{
    #region Public Properties

    private static List<Spawner> _instances = new List<Spawner>();
    public static IEnumerable<Spawner> Instances
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

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _instances.Add(this);

        if (Instantiated != null)
        {
            Instantiated(this, null);
        }
    }

    void OnDestroy()
    {
        _instances.Remove(this);

        if (Destroyed != null)
        {
            Destroyed(this, null);
        }
    }

    #endregion

    #region Public Methods

    public Enemy Spawn(Enemy prefab)
    {
        Enemy result = null;

        if (prefab != null)
        {
            result = GameObject.Instantiate<Enemy>(prefab);
            result.transform.parent = this.transform;
            result.transform.localPosition = Vector3.zero;
            result.transform.localRotation = Quaternion.identity;
        }

        return result;
    }

    #endregion
}