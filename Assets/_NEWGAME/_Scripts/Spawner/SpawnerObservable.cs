using _GAME._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SpawnerObservable : IObservable
{
    private CompositeDisposable _disposables;
    private MissionMonitor _missionMonitor;

    public SpawnerObservable(MissionMonitor missionMonitor)
    {
        _missionMonitor = missionMonitor;
        _disposables = new CompositeDisposable();
    }

    public IObservable<EnemySpawnerModel> modelSpawnStream { get; set; }
    
    public void AddDisposable(IDisposable disposable)
    {
        disposable.AddTo(_disposables);
    }

    public void ClearObservers()
    {
        _disposables.Clear();
    }
}
