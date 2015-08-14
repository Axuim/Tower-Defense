using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveController : MonoBehaviour
{
    #region Singleton

    public static WaveController Singleton { get; private set; }

    #endregion

    #region Private Properties

    private bool _started = false;
    
    [SerializeField]
    private WaveInfo[] _waves;

    #endregion

    #region Events

    public event EventHandler<WaveEventArgs> WaveStarted;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Singleton = this;

        GameStateManager.GameStateChanged += this.GameStateChangedHandler;
    }

    void OnDestroy()
    {
        GameStateManager.GameStateChanged -= this.GameStateChangedHandler;
    }

    #endregion

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
                this.WaveStarted(this, new WaveEventArgs(result, waveNumber));
            }
            this.StartCoroutine("SpawnWaveCoroutine", new object[] { waveNumber, result });
        }

        return result;
    }

    #endregion

    #region Event Handlers
    
    private void GameStateChangedHandler(object sender, EventArgs args)
    {
        if (GameStateManager.GameState == GameStates.Playing)
        {
            this.Begin();
        }
        else if (GameStateManager.GameState == GameStates.Loss)
        {
            this.Stop();
        }
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

        int spawnedCount = 0;
        int enemiesToSpawn = Mathf.RoundToInt(waveInfo.BaseSpawnCount * Mathf.Pow(waveInfo.WaveNumberSpawnCountMultiplier, waveNumber));
        float nextSpawnTime = Time.time;
        float spawnTimeDelta = (waveInfo.Duration * waveInfo.SpawnPeriod) / enemiesToSpawn;
        while (spawnedCount < enemiesToSpawn)
        {
            foreach (var spawner in Spawner.Instances)
            {
                spawner.Spawn(waveInfo.EnemyPrefab);
            }
            nextSpawnTime = Time.time + spawnTimeDelta;
            spawnedCount++;

            while (Time.time < nextSpawnTime)
            {
                yield return null;
            }
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