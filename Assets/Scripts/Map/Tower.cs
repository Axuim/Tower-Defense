using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Tower : MonoBehaviour, ISelectable
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

    private void EnemyDestroyedHandler(object sender, EventArgs args)
    {
        if (sender is Enemy)
        {
            _targetQueue.Remove(sender as Enemy);
        }
    }

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

    #region ISelectable

    public bool IsSelected { get; private set; }

    public bool Select()
    {
        bool result = false;

        if (this.IsSelected == false)
        {
            //TODO: Add selection logic

            this.IsSelected = true;
            result = true;
        }

        return result;
    }

    public bool Deselect()
    {
        bool result = false;

        if (this.IsSelected)
        {
            //TODO: Add deselection logic

            this.IsSelected = false;
            result = true;
        }

        return result;
    }

    #endregion
}
