using Assets._SDK.Game;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets._SDK.Input
{
    public class GameInput : IInput
    {
        /* private Joystick _joystick;

         public GameInput(Joystick joystick)
         {
             _joystick = joystick;
         }

         public IObservable<Vector3> MoveStream => Observable.EveryUpdate()
                 .Where(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing
                         && (_joystick.Vertical != 0 || _joystick.Horizontal != 0)
                 )
                 .Select(_ => Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal);

         public IObservable<bool> FireStream => Observable.EveryUpdate()
                 .Where(_ => AbstractGameManager.Instance.CurrentState.Value == GameState.Playing
                         && UnityEngine.Input.GetKeyDown(KeyCode.A)
                 )
                 .Select(_ => true);*/
        public IObservable<Vector3> MoveStream => throw new NotImplementedException();

        public IObservable<bool> FireStream => throw new NotImplementedException();

        public IObservable<int> PunchStream => throw new NotImplementedException();
    }
}