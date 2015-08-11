using UnityEngine;
using System;

[Serializable]
public class WaveInfo
{
    #region Public Properties

    [SerializeField]
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
    }

    [SerializeField]
    private float _duration;
    public float Duration
    {
        get
        {
            return _duration;
        }
    }

    [SerializeField]
    private GameObject _enemyPrefab;
    public GameObject EnemyPrefab
    {
        get
        {
            return _enemyPrefab;
        }
    }

    [SerializeField]
    private float _spawnCount;
    public float SpawnCount
    {
        get
        {
            return _spawnCount;
        }
    }

    [SerializeField]
    private float _spawnInterval;
    public float SpawnInterval
    {
        get
        {
            return _spawnInterval;
        }
    }

    #endregion
}