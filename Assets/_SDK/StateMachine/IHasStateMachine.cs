using Stateless;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets._SDK.StateMachine
{
    public interface IHasStateMachine<State, Trigger>
    {
        ReactiveProperty<State> PreviousState { get; }
        ReactiveProperty<State> CurrentState { get; }

        void Fire(Trigger trigger);

        bool CanFire(Trigger trigger);

        bool IsInState(State state);
    }
}