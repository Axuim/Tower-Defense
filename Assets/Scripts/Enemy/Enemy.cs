﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
public class Enemy : MonoBehaviour, IAmKillable
{
    #region Private Properties

    private float SAME_POSITION_EPSILON = 0.1f;

    private Seeker _seeker;

    private Objective _target;
    private Path _path;
    private int _pathIndex;

    [SerializeField]
    private float _speed = 1.0f;

    #endregion

    #region Public Properties

    [SerializeField]
    private int _damage = 1;
    public int Damage
    {
        get
        {
            return _damage;
        }
    }

    public float DistanceToObjective { get; private set; }

    #endregion

    #region Events

    public static EventHandler Instantiated;
    public static EventHandler Destroyed;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _seeker = this.GetComponent<Seeker>();

        if (Instantiated != null)
        {
            Instantiated(this, null);
        }

        this.Health = this.MaxHealth;
        this.DistanceToObjective = float.MaxValue;
    }

    void OnDestroy()
    {
        if (Destroyed != null)
        {
            Destroyed(this, null);
        }
    }

    void Start()
    {
        //Find target and add events.
        this.SetupTarget();
    }

    void OnEnable()
    {
        GameStateManager.GameStateChanging += this.GameStateChangingHandler;
        Wall.PathUpdated += this.WallsChangedHandler;
    }

    void OnDisable()
    {
        //Cleanup and remove events
        if (_target != null)
        {
            _target.Killed -= this.TargetDestroyedHandler;
            _target = null;
        }

        GameStateManager.GameStateChanging -= this.GameStateChangingHandler;
        Wall.PathUpdated -= this.WallsChangedHandler;
        
        this.CleanupPath();
    }

    void Update()
    {
        if (_target == null)
        {
            this.SetupTarget();
        }
        else
        {
            this.MoveTowardsTarget();
        }
    }

    #endregion
    
    #region Private Methods
    
    private void SetupTarget()
    {
        if (_target != null)
        {
            _target.Killed -= this.TargetDestroyedHandler;
        }

        //Find the closest objective "As the bird flies"
        _target = Objective.ClosestInstanceTo(this.transform.position, Objective.NotDestroyedPredicate);
        if (_target != null)
        {
            _target.Killed += this.TargetDestroyedHandler;
            this.PathTo(_target.transform.position);
        }
    }

    private void OnPathingComplete(Path path)
    {
        if (path.error == false)
        {
            if (_path != null)
            {
                _path.Release(this);
            }
            
            _path = path;
            _path.Claim(this);
            
            _pathIndex = 0;
            //Update distanec to objective.
            this.DistanceToObjective = _path.GetTotalLength();
        }
    }

    private void PathTo(Vector3 position)
    {
        this.CleanupPath();
        this.DistanceToObjective = float.MaxValue;
        _seeker.StartPath(this.transform.position, position, this.OnPathingComplete);
    }

    private void CleanupPath()
    {
        if (_path != null)
        {
            _path.Release(this);
            _path = null;
        }
    }

    private void MoveTowardsTarget()
    {
        if (_path != null)
        {
            if (_pathIndex >= _path.vectorPath.Count)
            {
                this.TargetReached();
            }
            else
            {
                var targetPosition = _path.vectorPath[_pathIndex];

                if (Vector3.Distance(this.transform.position, targetPosition) < SAME_POSITION_EPSILON)
                {
                    _pathIndex++;
                }

                Vector3 direction = (targetPosition - this.transform.position).normalized;
                direction *= _speed * Time.deltaTime;

                //Update distance to objective.
                this.DistanceToObjective -= direction.magnitude;

                //No collision checking.
                //Local avoidance and collision later maybe.
                this.transform.position += direction;
            }
        }
    }

    private void TargetReached()
    {
        if (_target != null)
        {
            _target.TakeDamage(this.Damage);
            GameObject.Destroy(this.gameObject);
        }
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

    private void TargetDestroyedHandler(object sender, EventArgs args)
    {
        this.SetupTarget();
    }

    private void WallsChangedHandler(object sender, EventArgs args)
    {
        if (_target != null)
        {
            this.PathTo(_target.transform.position);
        }
    }

    #endregion

    #region IAmKillable

    public event EventHandler Killed;

    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    private int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            if (value != _health)
            {
                if (value < 0)
                {
                    _health = 0;
                }
                else if (value > this.MaxHealth)
                {
                    _health = this.MaxHealth;
                }
                else
                {
                    _health = value;
                }

                if (this.Health <= 0)
                {
                    if (this.Killed != null)
                    {
                        this.Killed(this, null);
                    }

                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        this.Health -= amount;
    }

    #endregion
}
