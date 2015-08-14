using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProjectileTower : Tower
{
    #region Private Properties
    
    [SerializeField]
    private Projectile _projectilePrefab;
    [SerializeField]
    private Vector3 _localBulletSpawnPosition;

    #endregion

    #region MonoBehaviour
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position + _localBulletSpawnPosition, 0.05f);
    }

    #endregion

    #region Tower Overrides

    protected override bool Shoot()
    {
        bool result = base.Shoot();

        if (result)
        {
            var target = this.TargetQueue.OrderBy(e => e.DistanceToObjective).FirstOrDefault();
            if (target != null)
            {
                var bullet = GameObject.Instantiate<Projectile>(_projectilePrefab);
                bullet.transform.parent = this.transform;
                bullet.transform.localPosition = _localBulletSpawnPosition;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.Target = target;
            }
        }

        return result;
    }

    #endregion
}
