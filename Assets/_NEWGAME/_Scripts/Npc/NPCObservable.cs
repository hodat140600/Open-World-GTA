using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using _GAME._Scripts;

public class NPCObservable : IObservable
{
    private CompositeDisposable _disposables;

    private AIMeleeSkillBehaviour _aIMeleeSkillBehaviour;

    public NPCObservable()
    {

    }

    public NPCObservable(AIMeleeSkillBehaviour aIMeleeSkillBehaviour)
    {
        _aIMeleeSkillBehaviour = aIMeleeSkillBehaviour;
        _disposables = new CompositeDisposable();
    }
    public IObservable<bool> AIStateStream => Observable.EveryUpdate()
        .Select(_ => _aIMeleeSkillBehaviour.currentState == _GAME._Scripts._CharacterController._AI.SimpleMeleeAI_Motor.AIStates.Chase)
        /*.TakeWhile(_ => Gameplay.Instance.CurrentState.Value == GameplayState.Running)*/;

    public void AddDisposable(IDisposable disposable)
    {
        disposable.AddTo(_disposables);
    }

    public void ClearObservers()
    {
        _disposables.Clear();
    }
}
