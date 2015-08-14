using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Tower : MonoBehaviour
{
    #region Private Properties

    private List<Enemy> _targetQueue = new List<Enemy>();
    protected IEnumerable<Enemy> TargetQueue
    {
        get
        {
            return _targetQueue;
        }
    }

    private float _nextFireTime;

    [SerializeField]
    private float _fireDelay = 0.5f;

    #endregion

    #region Public Properties

    [SerializeField]
    private Tower[] _buildables;
    public IEnumerable<Tower> Buildables
    {
        get
        {
            return _buildables;
        }
    }

    #endregion

    #region MonoBehaviour

    void OnEnable()
    {
        Enemy.Destroyed += this.EnemyDestroyedHandler;
    }

    void OnDisable()
    {
        Enemy.Destroyed -= this.EnemyDestroyedHandler;
        _targetQueue.Clear();
    }

    void Update()
    {
        this.Shoot();
    }

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _targetQueue.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _targetQueue.Remove(enemy);
        }
    }

    #endregion

    #region Private Methods

    protected virtual bool Shoot()
    {
        bool result = false;
        
        if (_targetQueue.Any() && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + _fireDelay;

            result = true;
        }

        return result;
    }

    #endregion

    #region Event Handlers

    private void EnemyDestroyedHandler(object sender, EventArgs args)
    {
        if (sender is Enemy)
        {
            _targetQueue.Remove(sender as Enemy);
        }
    }

    #endregion
}
