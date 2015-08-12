﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyTarget : MonoBehaviour
{
    #region Public Properties

    private static List<EnemyTarget> _instances = new List<EnemyTarget>();
    public static IEnumerable<EnemyTarget> Instances
    {
        get
        {
            return _instances.ToArray();
        }
    }
    
    public static Func<EnemyTarget, bool> NotDestroyedPredicate = new Func<EnemyTarget, bool>((EnemyTarget et) => { return et.IsDestroyed == false; });

    public bool IsDestroyed
    {
        get
        {
            return this.Health == 0;
        }
    }

    [SerializeField]
    private int _maxHealth = 10;
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

                if (this.Health <= 0 && this.Destroyed != null)
                {
                    this.Destroyed(this, null);
                }
            }
        }
    }

    #endregion

    #region Events

    public event EventHandler Destroyed;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _instances.Add(this);

        this.Health = this.MaxHealth;
    }

    void OnDestroy()
    {
        _instances.Remove(this);
    }

    #endregion

    #region Public Methods
    
    public static EnemyTarget ClosestInstanceTo(Vector3 worldPosition)
    {
        EnemyTarget result = null;

        float leastDistance = float.MaxValue;
        float distance;
        foreach (var target in EnemyTarget.Instances)
        {
            distance = Vector3.Distance(worldPosition, target.transform.position);
            if (distance < leastDistance)
            {
                result = target;
                leastDistance = distance;
            }
        }

        return result;
    }

    public static EnemyTarget ClosestInstanceTo(Vector3 worldPosition, Func<EnemyTarget, bool> predicate)
    {
        EnemyTarget result = null;

        if (predicate != null)
        {
            float leastDistance = float.MaxValue;
            float distance;
            foreach (var target in EnemyTarget.Instances.Where(et => predicate(et)))
            {
                distance = Vector3.Distance(worldPosition, target.transform.position);
                if (distance < leastDistance)
                {
                    result = target;
                    leastDistance = distance;
                }
            }
        }

        return result;
    }

    public void TakeDamage(int damage)
    {
        this.Health -= damage;
    }

    #endregion

    #region Private Methods

    #endregion
}
