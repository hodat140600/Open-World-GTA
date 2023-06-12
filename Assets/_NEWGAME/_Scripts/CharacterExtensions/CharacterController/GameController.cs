using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME._Scripts
{
    using UnityEngine.Events;
    using _GAME._Scripts._CharacterController;

    [ClassHeader("Simple GameController Example", openClose = false)]
    public class GameController : ExtendMonoBehaviour
    {
        [System.Serializable]
        public class OnRealoadGame : UnityEvent { }
        [HelpBox("Assign your Character Prefab to be instantiate at the SpawnPoint, leave it unassigned to Restart the Scene instead")]
        public GameObject playerPrefab;
        [HelpBox("Assign a empty transform to spawn the Player to a specific location")]
        public Transform spawnPoint;
        [HelpBox("Time to wait until the scene restart or the player will be spawned again")]
        public float respawnTimer = 4f;
        [HelpBox("Check this if you want to destroy the dead body after the respawn")]
        public bool destroyBodyAfterDead;
        [HelpBox("Display a message using the FadeText UI")]
        public bool displayInfoInFadeText = true;

        [@HideInInspector]
        public OnRealoadGame OnReloadGame = new OnRealoadGame();
        [@HideInInspector]
        public GameObject currentPlayer;
        private ThirdPersonController currentController;
        public static GameController instance;
        private GameObject oldPlayer;

        public UnityEvent onSpawn;
        public bool dontDestroyOnLoad = true;

        protected virtual void Start()
        {
            if (instance == null)
            {
                instance = this;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }

                gameObject.name = gameObject.name + " Instance";
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            if (displayInfoInFadeText && HUDController.instance)
            {
                HUDController.instance.ShowText("Init Scene");
            }

            FindPlayer();
        }

        /// <summary>
        /// Show/Hide Cursor
        /// </summary>
        /// <param name="value"></param>
        public virtual void ShowCursor(bool value)
        {
            Cursor.visible = value;
        }

        /// <summary>
        /// Lock/Unlock the cursor to the center of screen
        /// </summary>
        /// <param name="value"></param>
        public virtual void LockCursor(bool value)
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        protected virtual void OnCharacterDead(GameObject _gameObject)
        {
            oldPlayer = _gameObject;

            if (playerPrefab != null)
            {
                StartCoroutine(RespawnRoutine());
            }
            else
            {
                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Restarting Scene...");
                }

                Invoke("ResetScene", respawnTimer);
            }
        }

        protected virtual IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnTimer);

            if (playerPrefab != null && spawnPoint != null)
            {
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Player destroyed: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    Destroy(oldPlayer);
                }
                else
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Remove Player Components: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    DestroyPlayerComponents(oldPlayer);
                }

                yield return new WaitForEndOfFrame();

                currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                currentController = currentPlayer.GetComponent<ThirdPersonController>();
                currentController.onDead.AddListener(OnCharacterDead);

                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Respawn player: " + currentPlayer.name.Replace("(Clone)", ""));
                }

                OnReloadGame.Invoke();
                onSpawn.Invoke();
            }
        }

        protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (currentController == null)
            {
                return;
            }

            if (currentController.CurrentHealth > 0)
            {
                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Load Scene: " + scene.name);
                }

                return;
            }
            if (displayInfoInFadeText && HUDController.instance)
            {
                HUDController.instance.ShowText("Reload Scene");
            }

            OnReloadGame.Invoke();
            FindPlayer();
        }

        protected virtual void FindPlayer()
        {
            var player = FindObjectOfType<ThirdPersonController>();

            if (player)
            {
                currentPlayer = player.gameObject;
                currentController = player;
                player.onDead.AddListener(OnCharacterDead);
                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Found player: " + currentPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                }
            }
            else if (currentPlayer == null && playerPrefab != null && spawnPoint != null)
            {
                SpawnAtPoint(spawnPoint);
            }
        }

        protected virtual void DestroyPlayerComponents(GameObject target)
        {
            if (!target)
            {
                return;
            }

            var comps = target.GetComponentsInChildren<MonoBehaviour>();
            for (int i = 0; i < comps.Length; i++)
            {
                Destroy(comps[i]);
            }
            var coll = target.GetComponent<Collider>();
            if (coll != null)
            {
                Destroy(coll);
            }

            var rigidbody = target.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }

            var animator = target.GetComponent<Animator>();
            if (animator != null)
            {
                Destroy(animator);
            }
        }

        /// <summary>
        /// Set a custom spawn point (or use it as checkpoint to your level) 
        /// </summary>
        /// <param name="newSpawnPoint"> new point to spawn</param>
        public virtual void SetSpawnSpoint(Transform newSpawnPoint)
        {
            spawnPoint = newSpawnPoint;
        }

        /// <summary>
        /// Set the default player prefab
        /// </summary>
        /// <param name="prefab"></param>
        public void SetPlayerPrefab(GameObject prefab)
        {
            playerPrefab = prefab;
        }

        /// <summary>
        /// Spawn New Player at a specific point
        /// </summary>
        /// <param name="targetPoint"> Point to spawn player</param>
        public virtual void SpawnAtPoint(Transform targetPoint)
        {
            if (playerPrefab != null)
            {
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Player destroyed: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    Destroy(oldPlayer);
                }

                else if (oldPlayer != null)
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Remove Player Components: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    DestroyPlayerComponents(oldPlayer);
                }

                currentPlayer = Instantiate(playerPrefab, targetPoint.position, targetPoint.rotation);
                currentController = currentPlayer.GetComponent<ThirdPersonController>();
                currentController.onDead.AddListener(OnCharacterDead);
                OnReloadGame.Invoke();

                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Spawn player: " + currentPlayer.name.Replace("(Clone)", ""));
                }
            }
        }

        /// <summary>
        /// Spawn player
        /// </summary>
        /// <param name="prefab"></param>
        public virtual void SpawnPlayer(GameObject prefab)
        {
            if (prefab != null && spawnPoint != null)
            {
                var targetPoint = spawnPoint;
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Player destroyed: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    Destroy(oldPlayer);
                }

                else if (oldPlayer != null)
                {
                    if (displayInfoInFadeText && HUDController.instance)
                    {
                        HUDController.instance.ShowText("Remove Player Components: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    DestroyPlayerComponents(oldPlayer);
                }

                currentPlayer = Instantiate(prefab, targetPoint.position, targetPoint.rotation);
                currentController = currentPlayer.GetComponent<ThirdPersonController>();
                currentController.onDead.AddListener(OnCharacterDead);
                OnReloadGame.Invoke();

                if (displayInfoInFadeText && HUDController.instance)
                {
                    HUDController.instance.ShowText("Spawn player: " + currentPlayer.name.Replace("(Clone)", ""));
                }
            }
        }

        /// <summary>
        /// Reload  current Scene and current Player
        /// </summary>
        public virtual void ResetScene()
        {
            if (oldPlayer)
            {
                DestroyPlayerComponents(oldPlayer);
            }

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);

            if (oldPlayer && destroyBodyAfterDead)
            {
                Destroy(oldPlayer);
            }
        }

    }
}