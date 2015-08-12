using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyTarget : MonoBehaviour
{
    #region Private Properties

    #endregion

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
            return false;
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

    #endregion

    #region Private Methods

    #endregion
}
