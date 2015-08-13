using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Projectile : MonoBehaviour
{
    #region Private Properties

    private const float SAME_POSITION_EPSILON = 0.1f;

    [SerializeField]
    private float _speed = 3.0f;

    #endregion

    #region Public Properties

    public Enemy Target { get; set; }

    [SerializeField]
    private int _damage = 1;
    public int Damage
    {
        get
        {
            return _damage;
        }
    }

    #endregion

    #region MonoBehaviour

    void Update()
    {
        this.MoveTowardsTarget();
    }

    #endregion

    #region Private Methods

    private void MoveTowardsTarget()
    {
        if (this.Target != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.Target.transform.position, _speed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, this.Target.transform.position) < SAME_POSITION_EPSILON)
            {
                this.Target.TakeDamage(_damage);
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            //Target was destroyed.
            GameObject.Destroy(this.gameObject);
        }
    }

    #endregion
}
