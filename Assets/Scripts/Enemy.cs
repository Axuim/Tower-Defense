using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
public class Enemy : MonoBehaviour
{
    #region Private Properties

    private float ADVANCE_WAYPOINT_DISTANCE = 0.1f;

    private Seeker _seeker;

    private EnemyTarget _target;
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

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _seeker = this.GetComponent<Seeker>();
    }

    void Start()
    {
        //Find target and add events.
        this.SetupTarget();
    }

    void OnEnable()
    {
        //Subscribe to events that will cause us to update our path
        Wall.Instantiated += this.WallsChangedHandler;
        Wall.Destroyed += this.WallsChangedHandler;
    }

    void OnDisable()
    {
        //Cleanup and remove events
        if (_target != null)
        {
            _target.Destroyed -= this.TargetDestroyedHandler;
            _target = null;
        }

        Wall.Instantiated -= this.WallsChangedHandler;
        Wall.Destroyed -= this.WallsChangedHandler;
        
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

    private void SetupTarget()
    {
        if (_target != null)
        {
            _target.Destroyed -= this.TargetDestroyedHandler;
        }

        _target = EnemyTarget.ClosestInstanceTo(this.transform.position, EnemyTarget.NotDestroyedPredicate);
        if (_target == null)
        {
            //No targets
            this.enabled = false;
        }
        else
        {
            _target.Destroyed += this.TargetDestroyedHandler;
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
        }
    }

    private void PathTo(Vector3 position)
    {
        this.CleanupPath();
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
                this.enabled = false;
            }
            else
            {
                var targetPosition = _path.vectorPath[_pathIndex];

                if (Vector3.Distance(this.transform.position, targetPosition) < ADVANCE_WAYPOINT_DISTANCE)
                {
                    _pathIndex++;
                }

                Vector3 direction = (targetPosition - this.transform.position).normalized;
                direction *= _speed * Time.deltaTime;

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
}
