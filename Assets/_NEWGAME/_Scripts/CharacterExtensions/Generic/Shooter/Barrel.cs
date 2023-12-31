﻿using UnityEngine;
using System.Collections.Generic;

namespace _GAME._Scripts._CharacterController._Shooter
{
    public class Barrel : HealthController
    {
        public Transform referenceTransformUP;
        public float maxAngleUp = 90;
        protected bool isBarrelRoll;
        public UnityEngine.Events.UnityEvent onBarrelRoll;
        public List<string> acceptableAttacks = new List<string>() { "explosion", "projectile" };

        void OnCollisionEnter()
        {
            if (!referenceTransformUP) return;
            var angle = Vector3.Angle(referenceTransformUP.up, Vector3.up);

            if (angle > maxAngleUp && !isBarrelRoll)
            {
                isBarrelRoll = true;
                onBarrelRoll.Invoke();
            }
        }

        public override void TakeDamage(Damage damage)
        {
            if (acceptableAttacks.Contains(damage.damageType))
            {
                base.TakeDamage(damage);
            }
        }
    }
}