using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveController : MonoBehaviour
{
    #region Private Properties

    private bool _started = false;

    [SerializeField]
    private Spawner[] _spawners;
    [SerializeField]
    private WaveInfo[] _waves;

    #endregion

    #region Events

    public event EventHandler<WaveEventArgs> WaveStarted;

    #endregion

    void Start()
    {
        this.Begin();
    }

    #region Public Methods

    public bool Begin()
    {
        bool result = false;

        if (_started == false)
        {
            _started = true;
            StartCoroutine("AdvanceWaveCoroutine");
            result = true;
        }

        return result;
    }

    public bool Stop()
    {
        bool result = false;

        if (_started)
        {
            _started = false;
            StopCoroutine("AdvanceWaveCoroutine");
            StopCoroutine("SpawnWaveCoroutine");
            result = true;
        }

        return result;
    }
    
    #endregion

    #region Private Methods

    private WaveInfo StartRandomWave(int waveNumber)
    {
        return this.StartWave(UnityEngine.Random.Range(0, _waves.Length), waveNumber);
    }

    private WaveInfo StartWave(int waveIndex, int waveNumber)
    {
        WaveInfo result = null;

        if (waveIndex >= 0 && _waves.Length > waveIndex)
        {
            result = _waves[waveIndex];
            if (this.WaveStarted != null)
            {
                this.WaveStarted(this, new WaveEventArgs(result, waveIndex));
            }
            this.StartCoroutine("SpawnWaveCoroutine", new object[] { waveNumber, result });
        }

        return result;
    }

    #endregion

    #region Coroutines

    private IEnumerator AdvanceWaveCoroutine()
    {
        yield return null;

        WaveInfo waveInfo = null;
        int waveNumber = 0;
        float nextWaveTime = 0.0f;

        while (true)
        {
            waveInfo = this.StartRandomWave(waveNumber);
            nextWaveTime = Time.time + waveInfo.Duration;
            while (nextWaveTime > Time.time)
            {
                yield return null;
            }
            waveNumber++;
        }
    }

    private IEnumerator SpawnWaveCoroutine(object[] args)
    {
        int waveNumber = (int)args[0];
        WaveInfo waveInfo = args[1] as WaveInfo;

        yield return null;

        float nextSpawnTime = Time.time;
        float spawnTimeDelta = (waveInfo.Duration * waveInfo.SpawnPeriod) / (waveInfo.BaseSpawnCount * Mathf.Pow(waveInfo.WaveNumberSpawnCountMultiplier, waveNumber));
        while (Time.time >= nextSpawnTime)
        {
            foreach (var spawner in _spawners)
            {
                spawner.Spawn(waveInfo.EnemyPrefab);
            }
            nextSpawnTime += spawnTimeDelta;

            yield return null;
        }
    }

    #endregion
}

#region WaveEventArgs

public class WaveEventArgs : EventArgs
{
    public WaveInfo Info { get; private set; }
    public int Number { get; private set; }

    public WaveEventArgs(WaveInfo info, int number)
        : base()
    {
        this.Info = info;
        this.Number = number;
    }
}

#endregion