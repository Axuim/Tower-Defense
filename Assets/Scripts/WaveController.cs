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
    private List<Spawner> _spawners;
    [SerializeField]
    private List<WaveInfo> _waves;

    #endregion

    #region Events

    public event EventHandler<WaveEventArgs> WaveStarted;

    #endregion

    #region Public Methods

    public bool Begin()
    {
        bool result = false;

        if (_started)
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
            result = true;
        }

        return result;
    }

    public WaveInfo StartWave(int wave)
    {
        WaveInfo result = null;
        
        if (_waves.Count > wave)
        {
            result = _waves[wave];
            if (this.WaveStarted != null)
            {
                this.WaveStarted(this, new WaveEventArgs(result, wave));
            }
            this.StartCoroutine(this.SpawnWaveCoroutine(result));
        }

        return result;
    }

    #endregion

    #region Coroutines

    private IEnumerator AdvanceWaveCoroutine()
    {
        yield return null;
        
        int wave = 0;
        WaveInfo waveInfo = this.StartWave(wave);
        float nextWaveTime = 0.0f;

        while (waveInfo != null)
        {
            nextWaveTime = Time.time + waveInfo.Duration;
            while (nextWaveTime > Time.time)
            {
                yield return null;
            }

            waveInfo = this.StartWave(wave);
            wave++;
        }
    }

    private IEnumerator SpawnWaveCoroutine(WaveInfo waveInfo)
    {
        yield return null;

        float nextSpawnTime = Time.time;
        while (Time.time >= nextSpawnTime)
        {
            foreach (var spawner in _spawners)
            {
                spawner.Spawn(waveInfo.EnemyPrefab);
            }
            nextSpawnTime += waveInfo.SpawnInterval;

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