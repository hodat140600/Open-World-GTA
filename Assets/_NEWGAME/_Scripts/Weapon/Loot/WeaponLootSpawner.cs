using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Game;
using _NEWGAME._Scripts.Utils;
using MyBox;
using UnityEngine;

namespace _GAME._Scripts.Inventory.Bonus
{
    public class WeaponLootSpawner : MonoBehaviour
    {
        [SerializeField] private WeaponLoot  weaponLootPrefab;
        [SerializeField] private MapSettings spawnPositions;
        [SerializeField] private Transform   spawnAroundTarget;

        [SerializeField] private LootPanel lootPanel;

        private const float SPAWN_LOOT_GAP_SECONDS = 15f;
        private const int   MAX_LOOT_COUNT         = 5;

        private const float MAX_DISTANCE_TO_SPAWN = 50f;

        // for performance
        private WaitForSeconds WaitForGapSeconds = new(SPAWN_LOOT_GAP_SECONDS);

        private List<WeaponLoot> _spawnedLoots;
        private int              currentIndexToRespawn = 0;

        private void Start()
        {
            _spawnedLoots = new();
            InvokeRepeating(nameof(AddOrRespawnLoot), 5, SPAWN_LOOT_GAP_SECONDS);
        }

        private void AddOrRespawnLoot()
        {
            // to not stop main thread from searching
            StartCoroutine(SpawnLootRoutine());
        }

        private IEnumerator SpawnLootRoutine()
        {
            WeaponLoot loot;

            Weapon   spawnWeapon = GetSpawnWeapon();
            Waypoint spawnPoint  = GetSpawnWaypoint();

            if (spawnPoint == default || spawnWeapon == default)
                yield break;

            if (_spawnedLoots.Count < MAX_LOOT_COUNT)
            {
                loot = Instantiate(weaponLootPrefab, transform);
                _spawnedLoots.Add(loot);
            }
            else
            {
                loot = _spawnedLoots[currentIndexToRespawn % _spawnedLoots.Count];
                currentIndexToRespawn++;
            }

            loot.Init(spawnWeapon, spawnPoint, () =>
            {
                lootPanel.ShowPanel(spawnWeapon);
                Destroy(loot.gameObject);
                StartCoroutine(RemoveLater(loot));
            });
        }


        private IEnumerator RemoveLater(WeaponLoot loot)
        {
            yield return WaitForGapSeconds;
            _spawnedLoots.Remove(loot);
        }

        private Weapon GetSpawnWeapon()
        {
            IEnumerable<int> spawnedWeaponsIds = _spawnedLoots.Select(loot => loot.Weapon.Id);

            return GameManager.Instance.Resources.WeaponResources.UnownedWeapons
                .Shuffle().FirstOrDefault(w => spawnedWeaponsIds.None(id => id == w.Id));
        }

        private Waypoint GetSpawnWaypoint()
        {
            Vector3          currentPosition    = spawnAroundTarget.position;
            IEnumerable<int> spawnedWaypointsId = _spawnedLoots.Select(loot => loot.Waypoint.Id);

            return spawnPositions.Entity.Waypoints
                .FirstOrDefault(w =>
                    Vector3.Distance(w.Position, currentPosition) < MAX_DISTANCE_TO_SPAWN &&
                    spawnedWaypointsId.None(id => id == w.Id));
        }
    }
}