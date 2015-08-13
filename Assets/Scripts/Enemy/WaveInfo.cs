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
    private float _spawnPeriod;
    public float SpawnPeriod
    {
        get
        {
            return _spawnPeriod;
        }
    }

    [SerializeField]
    private Enemy _enemyPrefab;
    public Enemy EnemyPrefab
    {
        get
        {
            return _enemyPrefab;
        }
    }

    [SerializeField]
    private float _baseSpawnCount;
    public float BaseSpawnCount
    {
        get
        {
            return _baseSpawnCount;
        }
    }

    [SerializeField]
    private float _waveNumberSpawnCountMultiplier;
    public float WaveNumberSpawnCountMultiplier
    {
        get
        {
            return _waveNumberSpawnCountMultiplier;
        }
    }

    #endregion
}