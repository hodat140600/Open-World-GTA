using System;
using Extensions;
using UnityEngine;

namespace _GAME._Scripts.Inventory.Bonus
{
    public class WeaponLoot : MonoBehaviour
    {
        public Weapon Weapon { get; private set; }
        public Waypoint Waypoint { get; private set; }
        public int WaypointId { get; private set; }

        [SerializeField] private Transform meshHolder;

        private Action onEnter;
        private bool   ready = false;

        [SerializeField] private SpriteRenderer _weaponLootMarked;

        public void Init(Weapon weapon, Waypoint waypoint, Action onEnter)
        {
            Weapon     = weapon;
            Waypoint   = waypoint;
            WaypointId = waypoint.Id;


            meshHolder.DestroyAllChildren();
            Transform modelTransform = Instantiate(Weapon.Model, meshHolder).transform;

            modelTransform.eulerAngles = new Vector3(-30f, 0, 0);

            _weaponLootMarked.sprite = Resources.Load<Sprite>($"Weapons/Avatar/{weapon.weaponType}");

            transform.position         = waypoint.Position;
            transform.name             = Weapon.Name;

            Transform minimapIcon = transform.Find("Marked");
            
            MinimapCamera.LockRotate(minimapIcon, true);
            this.onEnter = onEnter;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onEnter?.Invoke();
            }
        }
    }
}