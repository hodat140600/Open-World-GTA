using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._CharacterController._Shooter
{
    [ClassHeader("PowerCharge Projectile", openClose = false)]
    public class PowerChargeProjectileControl : ExtendMonoBehaviour
    {
        public List<ProjectilePerPower> projectiles;
        private ShooterWeapon weapon;
        private ProjectilePerPower lastProjectilePerPower;

        void Start()
        {
            weapon = GetComponent<ShooterWeapon>();
            if (weapon)
            {
                weapon.onPowerChargerChanged.AddListener(OnChangerPower);
            }
        }

        public void OnChangerPower(float value)
        {
            if (value <= 0) return;

            if (weapon)
            {
                var projectilePerPower = projectiles.Find(projectile => value >= projectile.min && value <= projectile.max);

                if (projectilePerPower != null && projectilePerPower.projectile && lastProjectilePerPower != projectilePerPower)
                {
                    lastProjectilePerPower = projectilePerPower;
                    weapon.projectile = projectilePerPower.projectile;
                    projectilePerPower.OnValidatePower.Invoke();
                }
            }
        }

        [System.Serializable]
        public class ProjectilePerPower
        {
            public float min;
            public float max;
            public GameObject projectile;
            [Tooltip("Called when power is between min and max value")]
            public UnityEngine.Events.UnityEvent OnValidatePower;
        }
    }
}

