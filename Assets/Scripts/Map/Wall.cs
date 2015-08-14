using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Collider))]
public class Wall : MonoBehaviour, IAmSelectable
{
    #region Private Properties

    private Collider _collider;

    [SerializeField]
    private Tower[] _emptyBuildables;

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

    public Tower Tower { get; private set; }
    
    public IEnumerable<Tower> Buildables
    {
        get
        {
            return this.Tower != null ? this.Tower.Buildables : _emptyBuildables;
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

    void OnEnable()
    {
        GameStateManager.GameStateChanging += this.GameStateChangingHandler;
    }

    void OnDestroy()
    {
        GameStateManager.GameStateChanging -= this.GameStateChangingHandler;

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

    #region Public Methods

    public bool BuildTower(Tower prefab)
    {
        bool result = false;

        if (this.Tower != null)
        {
            GameObject.Destroy(this.Tower.gameObject);
            result = true;
        }

        if (prefab != null)
        {
            var tower = GameObject.Instantiate<Tower>(prefab);
            tower.transform.parent = this.transform;
            tower.transform.localPosition = Vector3.zero;
            tower.transform.localRotation = Quaternion.identity;
            this.Tower = tower;

            result = true;
        }

        return result;
    }

    #endregion

    #region Event Handlers

    private void GameStateChangingHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Loss)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    #endregion

    #region IAmSelectable

    private bool _selected = false;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        private set
        {
            if (value != _selected)
            {
                _selected = value;

                if (_selected)
                {
                    //TODO: Selection Logic
                }
                else
                {
                    //TODO: Deselection Logic
                }
            }
        }
    }

    public bool SetSelected(bool value)
    {
        bool result = false;

        if (value != this.Selected)
        {
            this.Selected = value;
            result = true;
        }

        return result;
    }

    #endregion
}