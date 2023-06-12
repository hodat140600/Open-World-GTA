using System.Collections;
using System.Collections.Generic;
using _GAME._Scripts;
using _GAME._Scripts._Camera;
using _GAME._Scripts._CharacterController;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using _GAME._Scripts.Inventory.Ammo;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Group;
using _GAME._Scripts.Npc.SkillSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;

namespace Assets._SDK.Game
{
    [DefaultExecutionOrder(-1)]
    public class GameDriver : GameSingleton<GameDriver>
    {
        #region States

        [TitleGroup("GameManager")]
        [HorizontalGroup("GameManager/State", .333f), VerticalGroup("GameManager/State/Source"), ReadOnly, LabelText("Source"), GUIColor(100f / 255f, 220f / 255f, 255f / 255f)]
        public List<string> sources;

        [HorizontalGroup("GameManager/State", .333f), VerticalGroup("GameManager/State/Trigger"), ReadOnly, LabelText("Trigger"), LabelWidth(10), GUIColor(255f / 255f, 188f / 255f, 107f / 255f)]
        public List<string> triggers;

        [HorizontalGroup("GameManager/State", .333f), VerticalGroup("GameManager/State/Destination"), ReadOnly, LabelText("Destination"), LabelWidth(10), GUIColor(255f / 255f, 100f / 255f, 100f / 255f)]
        public List<string> destinations;

        public bool GoToIngame;

        private void Start()
        {
            if (GoToIngame)
            {
                StartCoroutine(PlayImmediately());
            }

            StartCoroutine(RetrieveStateMachinesStates());
        }

        private IEnumerator PlayImmediately()
        {
            yield return new WaitUntil(() => GameManager.Instance.CurrentState.Value == GameState.LobbyHome);
            GameManager.Instance.Fire(GameTrigger.Play);
        }

        private IEnumerator RetrieveStateMachinesStates()
        {
            sources      = new List<string>();
            triggers     = new List<string>();
            destinations = new List<string>();

            yield return new WaitUntil(() => GameManager.Instance.StateMachine != null);
            GameManager.Instance.StateMachine.OnTransitioned(t =>
            {
                sources.Add(t.Source.ToString());
                triggers.Add(t.Trigger.ToString());
                destinations.Add(t.Destination.ToString());
            });
        }

        #endregion

        #region Navigation

        [TabGroup("Navigation")]
        [Button("Start Game", ButtonSizes.Medium)]
        public void StartGame()
        {
            GameManager.Instance.Fire(GameTrigger.Play);
        }

        [TabGroup("Navigation")]
        [Button("Go Shopping", ButtonSizes.Medium)]
        public void GoShopping()
        {
            GameManager.Instance.Fire(GameTrigger.ShowShop);
        }

        [TabGroup("Navigation")]
        [Button("Back To Lobby", ButtonSizes.Medium)]
        public void BackToLobby()
        {
            GameManager.Instance.Fire(GameTrigger.BackToLobby);
        }

        [TabGroup("Navigation")]
        [Button("Start Mission", ButtonSizes.Medium)]
        public void StartMission()
        {
            // Gameplay.Instance.Fire(GameplayTrigger.StartMission);
            // GameManager.Instance.Fire(GameTrigger.BackToLobby);
        }

        #endregion

        #region Cheat

        [TabGroup("Cheat")]
        [Button("Deposit 100000 Coin", ButtonSizes.Medium)]
        public void Deposit100000Coin()
        {
            GameManager.Instance.Wallet?.DefaultAccount.Deposit(100000);
        }

        [TabGroup("Kill NPC")]
        public ModelType npcToKill;

        [TabGroup("Kill NPC")]
        public WeaponType weaponToUse;

        [TabGroup("Kill NPC")]
        [Button("Bủh", ButtonSizes.Medium)]
        public void KillNpc()
        {
            MessageBroker.Default.Publish(new NpcKilledEvent(npcToKill, true, weaponToUse));
        }

        [TabGroup("Cheat")]
        [Button("Kill Player", ButtonSizes.Medium)]
        public void KillPlayer()
        {
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.Fail);
        }

        [TabGroup("Cheat")]
        [Button("Revise Player", ButtonSizes.Medium)]
        public void RevivePlayer()
        {
            Gameplay.Instance.Fire(GameplayTrigger.BackToPlaying);
        }

        [TabGroup("Cheat")]
        [Button("Win Mission", ButtonSizes.Medium)]
        public void WinMission()
        {
            Gameplay.Instance.MissionManager.Finish();
        }

        [TabGroup("Cheat")]
        [Button("OutOfAmmo", ButtonSizes.Medium)]
        public void OutOfAmmo()
        {
            AmmoRefillerPanel.OnOutOfAmmo?.Invoke(WeaponType.Handgun);
        }

        #endregion

        #region Others

        [TabGroup("Others")]
        [Button("Clear PlayerPrefs", ButtonSizes.Medium)]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            // winParticle.Play();
        }

        #endregion

        #region NPC Spawn

        [TabGroup("NPC")]
        public NpcGroupSettings npcGroupSettings;

        [TabGroup("NPC")]
        [OdinSerialize] private Dictionary<int, List<GameObject>> npcs = new Dictionary<int, List<GameObject>>();

        [TabGroup("NPC")]
        [SerializeField] private ObjectPoolController objectPoolController => FindObjectOfType<ObjectPoolController>();

        [TabGroup("NPC")]
        [Button("Init NPC", ButtonSizes.Medium)]
        void InitNPC()
        {
            foreach (var id in npcGroupSettings.npcGroup.npcIds)
            {
                npcs.Add(id, new List<GameObject>());
            }
        }

        [TabGroup("NPC")]
        [Button("Spawn NPC", ButtonSizes.Medium)]
        void SpawnNPC()
        {
            foreach (var id in npcGroupSettings.npcGroup.npcIds)
            {
                npcs[id].Add(objectPoolController.GetNPCFromPool<PedestrianSkillSystem>(id.ToString(), npcGroupSettings.npcGroup.startingWaypointPosition));
            }
        }

        [TabGroup("NPC")]
        [Button("Discard NPC", ButtonSizes.Medium)]
        void DiscardNPC()
        {
            foreach (var npc in npcs)
            {
                objectPoolController.ReturnNPCToPool(npc.Value[0], npc.Key.ToString());
                npc.Value.RemoveAt(0);
            }
        }

        [SerializeField]
        private string Name;

        [TabGroup("NPC")]
        [Button("Hashcode NPC", ButtonSizes.Medium)]
        void HashcodeNPC()
        {
            Debug.Log((nameof(Npc) + Name).GetHashCode());
        }

        #endregion
        [TabGroup("Test")]
        [Button("Dance", ButtonSizes.Medium)]
        public void TestAnim()
        {
            GameManager.Instance.ItemManager.GetComponent<ThirdPersonController>().PlayAnimHighestScore();
        }
   


    }
}