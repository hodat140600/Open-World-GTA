using System.Collections;
using _GAME._Scripts.Game;
using _SDK.Inventory;
using _SDK.Money;
using Stateless;
using UnityEngine;

namespace Assets._SDK.Game
{
    public abstract class AbstractGameManager : GameSingleton<AbstractGameManager>
    {
        [SerializeField] private GameState StartingGameState;
        
        private StateMachine<GameState, GameTrigger> _stateMachine;
        public StateMachine<GameState, GameTrigger> StateMachine =>
            _stateMachine ??= new StateMachine<GameState, GameTrigger>(StartingGameState);
        public Wallet Wallet { get; protected set; }

        // public AbstractInventory<AbstractItem> Inventory { get; protected set; }

        public IEnumerator Restart()
        {
            // if (_stateMachine.CanFire(GameTrigger.Retry))
            // {
            //     _stateMachine.Fire(GameTrigger.Retry);
            // }

            yield return null;
        }

        void Start()
        {
            OnStart();
        }
        
        protected abstract void OnStart();
        protected abstract void OnPlayingEntry();
    }
}