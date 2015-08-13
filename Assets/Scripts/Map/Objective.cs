using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Objective : MonoBehaviour, IKillable
{
    #region Public Properties

    private static List<Objective> _instances = new List<Objective>();
    public static IEnumerable<Objective> Instances
    {
        get
        {
            return _instances;
        }
    }
    
    public static Func<Objective, bool> NotDestroyedPredicate = new Func<Objective, bool>((Objective et) => { return et.IsDestroyed == false; });

    public bool IsDestroyed
    {
        get
        {
            return this.Health == 0;
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

        this.Health = this.MaxHealth;
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
    
    public static Objective ClosestInstanceTo(Vector3 worldPosition)
    {
        return Objective.ClosestInstanceTo(worldPosition, Objective.Instances);
    }

    public static Objective ClosestInstanceTo(Vector3 worldPosition, Func<Objective, bool> predicate)
    {
        return Objective.ClosestInstanceTo(worldPosition, Objective.Instances.Where(et => predicate(et)));
    }

    public void TakeDamage(int damage)
    {
        this.Health -= damage;
    }

    #endregion

    #region Private Methods

    private static Objective ClosestInstanceTo(Vector3 worldPosition, IEnumerable<Objective> instances)
    {
        Objective result = null;

        float leastDistance = float.MaxValue;
        float distance;
        foreach (var target in instances)
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

    #endregion

    #region IKillable

    public event EventHandler Killed;

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

    #endregion
}
