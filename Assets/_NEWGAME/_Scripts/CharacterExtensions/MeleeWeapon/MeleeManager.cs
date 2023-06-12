using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._Melee
{
    using UnityEngine.Events;
    using _GAME._Scripts;
    using _GAME._Scripts._ItemManager;
    using _GAME._Scripts._EventSystems;

    /// <summary>
    /// Event called when you equip a weapon (Weapon, isLeftWeapon)
    /// </summary>
    [Serializable]
    public class OnEquipWeaponEvent : UnityEvent<GameObject, bool> { }
    public class MeleeManager : ExtendMonoBehaviour, IWeaponEquipmentListener
    {
        #region SeralizedProperties in CustomEditor

        public List<BodyMember> Members = new List<BodyMember>();
        public Damage defaultDamage = new Damage(10);
        public HitProperties hitProperties;
        public MeleeWeapon leftWeapon, rightWeapon;
        public OnHitEvent onDamageHit, onRecoilHit;
        public OnEquipWeaponEvent onEquipWeapon;
        #endregion

        [Tooltip("NPC ONLY- Ideal distance for the attack")]
        public float defaultAttackDistance = 1f;
        [Tooltip("Default cost for stamina when attack")]
        public float defaultStaminaCost = 20f;
        [Tooltip("Default recovery delay for stamina when attack")]
        public float defaultStaminaRecoveryDelay = 1f;
        [Range(0, 100)]
        public int defaultDefenseRate = 100;
        [Range(0, 180)]
        public float defaultDefenseRange = 90;

        [@HideInInspector]
        public IMeleeFighter fighter;
        private int damageMultiplier;
        private int currentRecoilID;
        private int currentReactionID;
        private bool ignoreDefense;
        private bool activeRagdoll;
        private float senselessTime;
        private bool inRecoil;
        private string attackName;

        protected virtual void Start()
        {
            Init();
        }

        /// <summary>
        /// Init properties
        /// </summary>
        public virtual void Init()
        {
            fighter = gameObject.GetMeleeFighter();
            ///Initialize all bodyMembers and weapons
            foreach (BodyMember member in Members)
            {
                ///damage of member will be always the defaultDamage
                //member.attackObject.damage = defaultDamage;       
                if (member.attackObject == null)
                {
                    var attackObjects = GetComponentsInChildren<MeleeAttackObject>();
                    if (attackObjects.Length > 0)
                        member.attackObject = Array.Find(attackObjects, a => a.attackObjectName.Equals(member.bodyPart));

                    if (member.attackObject == null)
                    {
                        Debug.LogWarning("Can't find the attack Object " + member.bodyPart);
                        continue;
                    }
                }
                member.attackObject.damage.damageValue = defaultDamage.damageValue;
                if (member.bodyPart == HumanBodyBones.LeftLowerArm.ToString())
                {
                    var weapon = member.attackObject.GetComponentInChildren<MeleeWeapon>(true);
                    leftWeapon = weapon;
                }
                if (member.bodyPart == HumanBodyBones.RightLowerArm.ToString())
                {
                    var weapon = member.attackObject.GetComponentInChildren<MeleeWeapon>(true);
                    rightWeapon = weapon;
                }
                member.attackObject.meleeManager = this;
                member.SetActiveDamage(false);
            }

            if (leftWeapon != null)
            {
                leftWeapon.SetActiveDamage(false);
                leftWeapon.meleeManager = this;
            }
            if (rightWeapon != null)
            {
                rightWeapon.meleeManager = this;
                rightWeapon.SetActiveDamage(false);
            }
        }

        /// <summary>
        /// Set active Multiple Parts to attack
        /// </summary>
        /// <param name="bodyParts"></param>
        /// <param name="type"></param>
        /// <param name="active"></param>
        /// <param name="damageMultiplier"></param>
        public virtual void SetActiveAttack(List<string> bodyParts, AttackType type, bool active = true, int damageMultiplier = 0, int recoilID = 0, int reactionID = 0, bool ignoreDefense = false, bool activeRagdoll = false, float senselessTime = 0, string attackName = "")
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                var bodyPart = bodyParts[i];
                SetActiveAttack(bodyPart, type, active, damageMultiplier, recoilID, reactionID, ignoreDefense, activeRagdoll, senselessTime, attackName);
            }
        }

        /// <summary>
        /// Set active Single Part to attack
        /// </summary>
        /// <param name="bodyPart"></param>
        /// <param name="type"></param>
        /// <param name="active"></param>
        /// <param name="damageMultiplier"></param>
        public virtual void SetActiveAttack(string bodyPart, AttackType type, bool active = true, int damageMultiplier = 0, int recoilID = 0, int reactionID = 0, bool ignoreDefense = false, bool activeRagdoll = false, float senselessTime = 0, string attackName = "")
        {
            this.damageMultiplier = damageMultiplier;
            currentRecoilID = recoilID;
            currentReactionID = reactionID;
            this.ignoreDefense = ignoreDefense;
            this.activeRagdoll = activeRagdoll;
            this.attackName = attackName;
            this.senselessTime = senselessTime;
            if (type == AttackType.Unarmed)
            {
                /// find attackObject by bodyPart
                var attackObject = Members.Find(member => member.bodyPart == bodyPart);
                if (attackObject != null)
                {
                    attackObject.SetActiveDamage(active);
                }
            }
            else
            {   ///if AttackType == MeleeWeapon
                ///this work just for Right and Left Lower Arm         
                if (bodyPart == "RightLowerArm" && rightWeapon != null)
                {
                    rightWeapon.meleeManager = this;
                    rightWeapon.SetActiveDamage(active);
                }
                else if (bodyPart == "LeftLowerArm" && leftWeapon != null)
                {
                    leftWeapon.meleeManager = this;
                    leftWeapon.SetActiveDamage(active);
                }
            }
        }

        /// <summary>
        /// Listener of Damage Event
        /// </summary>
        /// <param name="hitInfo"></param>
        public virtual void OnDamageHit(ref HitInfo hitInfo)
        {
            Damage damage = new Damage(hitInfo.attackObject.damage);
            damage.sender = transform;
            damage.reaction_id = currentReactionID;
            damage.recoil_id = currentRecoilID;
            if (activeRagdoll) damage.activeRagdoll = activeRagdoll;
            if (attackName != string.Empty) damage.damageType = attackName;
            if (ignoreDefense) damage.ignoreDefense = ignoreDefense;
            if (senselessTime != 0) damage.senselessTime = senselessTime;
            /// Calc damage with multiplier 
            /// and Call ApplyDamage of attackObject 

            damage.damageValue *= damageMultiplier > 1 ? damageMultiplier : 1;
            hitInfo.targetIsBlocking = !hitInfo.attackObject.ApplyDamage(hitInfo.hitBox, hitInfo.targetCollider, damage);

            onDamageHit.Invoke(hitInfo);
        }

        /// <summary>
        /// Listener of Recoil Event
        /// </summary>
        /// <param name="hitInfo"></param>
        public virtual void OnRecoilHit(HitInfo hitInfo)
        {
            if (hitProperties.useRecoil && InRecoilRange(hitInfo) && !inRecoil)
            {
                inRecoil = true;
                var id = currentRecoilID;
                if (fighter != null) fighter.OnRecoil(id);
                onRecoilHit.Invoke(hitInfo);
                Invoke("ResetRecoil", 1f);
            }
        }

        /// <summary>
        /// Call Weapon Defense Events.
        /// </summary>
        public virtual void OnDefense()
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                leftWeapon.OnDefense();
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                rightWeapon.OnDefense();
            }
        }

        /// <summary>
        /// Get Current Attack ID
        /// </summary>
        /// <returns></returns>
        public virtual int GetAttackID()
        {
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyDefense && rightWeapon.gameObject.activeInHierarchy) return rightWeapon.attackID;
            return 0;
        }

        /// <summary>
        /// Get StaminaCost
        /// </summary>
        /// <returns></returns>
        public virtual float GetAttackStaminaCost()
        {
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyDefense && rightWeapon.gameObject.activeInHierarchy) return rightWeapon.staminaCost;
            return defaultStaminaCost;
        }

        /// <summary>
        /// Get StaminaCost
        /// </summary>
        /// <returns></returns>
        public virtual float GetAttackStaminaRecoveryDelay()
        {
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyDefense && rightWeapon.gameObject.activeInHierarchy) return rightWeapon.staminaRecoveryDelay;
            return defaultStaminaRecoveryDelay;
        }

        /// <summary>
        /// Get ideal distance for the attack
        /// </summary>
        /// <returns></returns>
        public virtual float GetAttackDistance()
        {
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyDefense && rightWeapon.gameObject.activeInHierarchy) return rightWeapon.distanceToAttack;
            return defaultAttackDistance;
        }

        /// <summary>
        /// Get Current Defense ID
        /// </summary>
        /// <returns></returns>
        public virtual int GetDefenseID()
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                GetComponent<Animator>().SetBool("FlipAnimation", false);
                return leftWeapon.defenseID;
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                GetComponent<Animator>().SetBool("FlipAnimation", true);
                return rightWeapon.defenseID;
            }
            return 0;
        }

        /// <summary>
        /// Get Defense Rate of Melee Defense 
        /// </summary>
        /// <returns></returns>
        public int GetDefenseRate()
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                return leftWeapon.defenseRate;
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                return rightWeapon.defenseRate;
            }
            return defaultDefenseRate;
        }

        /// <summary>
        /// Get Current MoveSet ID
        /// </summary>
        /// <returns></returns>
        public virtual int GetMoveSetID()
        {
            if (rightWeapon != null && rightWeapon.gameObject.activeInHierarchy) return rightWeapon.movesetID;
            // if (leftWeapon != null && leftWeapon.gameObject.activeInHierarchy) return leftWeapon.movesetID;
            return 0;
        }

        /// <summary>
        /// Check if defence can break Attack
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBreakAttack()
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                return leftWeapon.breakAttack;
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                return rightWeapon.breakAttack;
            }
            return false;
        }

        /// <summary>
        /// Check if attack can be blocked
        /// </summary>
        /// <param name="attackPoint"></param>
        /// <returns></returns>
        public virtual bool CanBlockAttack(Vector3 attackPoint)
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                return Math.Abs(transform.HitAngle(attackPoint)) <= leftWeapon.defenseRange;
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                return Math.Abs(transform.HitAngle(attackPoint)) <= rightWeapon.defenseRange;
            }
            return Math.Abs(transform.HitAngle(attackPoint)) <= defaultDefenseRange;
        }

        /// <summary>
        /// Get defense recoilID for break attack
        /// </summary>
        /// <returns></returns>
        public virtual int GetDefenseRecoilID()
        {
            if (leftWeapon != null && leftWeapon.meleeType != MeleeType.OnlyAttack && leftWeapon.gameObject.activeInHierarchy)
            {
                return leftWeapon.recoilID;
            }
            if (rightWeapon != null && rightWeapon.meleeType != MeleeType.OnlyAttack && rightWeapon.gameObject.activeInHierarchy)
            {
                return rightWeapon.recoilID;
            }
            return 0;
        }

        /// <summary>
        /// Check if angle of hit point is in range of recoil
        /// </summary>
        /// <param name="hitInfo"></param>
        /// <returns></returns>
        protected virtual bool InRecoilRange(HitInfo hitInfo)
        {
            var center = new Vector3(transform.position.x, hitInfo.hitPoint.y, transform.position.z);
            var euler = (Quaternion.LookRotation(hitInfo.hitPoint - center).eulerAngles - transform.eulerAngles).NormalizeAngle();

            return euler.y <= hitProperties.recoilRange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponObject"></param>
        public virtual void SetLeftWeapon(GameObject weaponObject)
        {
            if (weaponObject)
            {
                leftWeapon = weaponObject.GetComponent<MeleeWeapon>();
                SetLeftWeapon(leftWeapon);
            }
            else leftWeapon = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponObject"></param>
        public virtual void SetRightWeapon(GameObject weaponObject)
        {
            if (weaponObject)
            {
                rightWeapon = weaponObject.GetComponent<MeleeWeapon>();
                SetRightWeapon(rightWeapon);
            }
            else rightWeapon = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponObject"></param>
        public virtual void SetLeftWeapon(MeleeWeapon weapon)
        {
            if (weapon)
            {
                onEquipWeapon.Invoke(weapon.gameObject, true);
                leftWeapon = weapon;
                leftWeapon.SetActiveDamage(false);
                leftWeapon.meleeManager = this;
            }
            else leftWeapon = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponObject"></param>
        public virtual void SetRightWeapon(MeleeWeapon weapon)
        {
            if (weapon)
            {
                onEquipWeapon.Invoke(weapon.gameObject, false);
                rightWeapon = weapon;
                rightWeapon.meleeManager = this;
                rightWeapon.SetActiveDamage(false);
            }
            else
            {
                rightWeapon = null;
            }
        }

        public virtual MeleeWeapon CurrentActiveAttackWeapon
        {
            get
            {
                if (rightWeapon && rightWeapon.gameObject.activeInHierarchy && (rightWeapon.meleeType == MeleeType.OnlyAttack || rightWeapon.meleeType == MeleeType.AttackAndDefense)) return rightWeapon;
                if (leftWeapon && leftWeapon.gameObject.activeInHierarchy && (leftWeapon.meleeType == MeleeType.OnlyAttack || leftWeapon.meleeType == MeleeType.AttackAndDefense)) return leftWeapon;
                return null;
            }
        }

        public virtual MeleeWeapon CurrentActiveDefenseWeapon
        {
            get
            {
                if (rightWeapon && rightWeapon.gameObject.activeInHierarchy && (rightWeapon.meleeType == MeleeType.OnlyDefense || rightWeapon.meleeType == MeleeType.AttackAndDefense)) return rightWeapon;
                if (leftWeapon && leftWeapon.gameObject.activeInHierarchy && (leftWeapon.meleeType == MeleeType.OnlyDefense || leftWeapon.meleeType == MeleeType.AttackAndDefense)) return leftWeapon;
                return null;
            }
        }

        public virtual MeleeWeapon CurrentAttackWeapon
        {
            get
            {
                if (rightWeapon && (rightWeapon.meleeType == MeleeType.OnlyAttack || rightWeapon.meleeType == MeleeType.AttackAndDefense)) return rightWeapon;
                if (leftWeapon && (leftWeapon.meleeType == MeleeType.OnlyAttack || leftWeapon.meleeType == MeleeType.AttackAndDefense)) return leftWeapon;
                return null;
            }
        }

        public virtual MeleeWeapon CurrentDefenseWeapon
        {
            get
            {
                if (rightWeapon && (rightWeapon.meleeType == MeleeType.OnlyDefense || rightWeapon.meleeType == MeleeType.AttackAndDefense)) return rightWeapon;
                if (leftWeapon && (leftWeapon.meleeType == MeleeType.OnlyDefense || leftWeapon.meleeType == MeleeType.AttackAndDefense)) return leftWeapon;
                return null;
            }
        }

        protected virtual void ResetRecoil()
        {
            inRecoil = false;
        }
    }

    #region Secundary Classes
    [Serializable]
    public class OnHitEvent : UnityEvent<HitInfo> { }

    [Serializable]
    public class BodyMember
    {
        public Transform transform;
        public string bodyPart;

        public MeleeAttackObject attackObject;
        public bool isHuman;
        public void SetActiveDamage(bool active)
        {
            attackObject.SetActiveDamage(active);
        }
    }

    public enum HumanBones
    {
        RightHand, RightLowerArm, RightUpperArm, RightShoulder,
        LeftHand, LeftLowerArm, LeftUpperArm, LeftShoulder,
        RightFoot, RightLowerLeg, RightUpperLeg,
        LeftFoot, LeftLowerLeg, LeftUpperLeg,
        Chest,
        Head
    }

    public enum AttackType
    {
        Unarmed, MeleeWeapon
    }
    #endregion
}
