using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

[RequireComponent(typeof(Collider))]
public class BuildingWall : MonoBehaviour
{
    #region Private Properties

    private const float SAME_POSITION_EPSILON = 0.01f;

    private Collider _collider;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _collider = this.GetComponent<Collider>();
        _collider.enabled = false;
    }

    #endregion

    #region Public Methods

    public bool CanBuildAtPosition()
    {
        return this.IsColliding() == false && this.AllObjectivesReachable();
    }

    #endregion

    #region Private Methods

    private bool IsColliding()
    {
        return Wall.Instances.Any(w => Vector3.Distance(this.transform.position, w.transform.position) < Wall.SIZE) && Objective.Instances.Any(o => Vector3.Distance(this.transform.position, o.transform.position) < Wall.SIZE);
    }

    private bool AllObjectivesReachable()
    {
        bool result = false;

        if (AstarPath.active != null)
        {
            //Add our test to the graph
            _collider.enabled = true;
            AstarPath.active.UpdateGraphs(_collider.bounds);

            //Check if we can still reach each objective
            Path path = null;
            bool allPathsSuccessful = true;
            foreach (var spawner in Spawner.Instances)
            {
                foreach (var objective in Objective.Instances)
                {
                    path = ABPath.Construct(spawner.transform.position, objective.transform.position);
                    AstarPath.StartPath(path, true);
                    AstarPath.WaitForPath(path);
                    
                    allPathsSuccessful = path.error == false && Vector3.Distance(objective.transform.position, path.vectorPath.Last()) < SAME_POSITION_EPSILON;

                    if (allPathsSuccessful == false)
                    {
                        break;
                    }
                }

                if (allPathsSuccessful == false)
                {
                    break;
                }
            }

            //Remove our test from the graph
            _collider.enabled = false;
            AstarPath.active.UpdateGraphs(_collider.bounds);

            result = allPathsSuccessful;
        }

        return result;
    }

    #endregion
}