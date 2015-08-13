using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : MonoBehaviour
{
    #region Singleton

    public static Map Singleton { get; private set; }

    #endregion

    #region Public Properties

    [SerializeField]
    private Transform _innerWallsParent;
    public Transform InnerWallsParent
    {
        get
        {
            return _innerWallsParent;
        }
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Singleton = this;
    }

    #endregion
}
