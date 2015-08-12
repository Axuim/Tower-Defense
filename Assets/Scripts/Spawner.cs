using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour
{
    #region Public Methods

    public Enemy Spawn(Enemy prefab)
    {
        Enemy result = null;

        if (prefab != null)
        {
            result = GameObject.Instantiate<Enemy>(prefab);
            result.transform.parent = this.transform;
            result.transform.localPosition = Vector3.zero;
            result.transform.localRotation = Quaternion.identity;
        }

        return result;
    }

    #endregion
}