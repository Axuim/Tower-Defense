using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreaTower : Tower
{
    #region Private Properties
    
    [SerializeField]
    private int _damage;

    #endregion

    #region Tower Overrides

    protected override bool Shoot()
    {
        bool result = base.Shoot();

        if (result)
        {
            foreach (var target in this.TargetQueue)
            {
                target.TakeDamage(_damage);
            }
        }

        return result;
    }

    #endregion
}
