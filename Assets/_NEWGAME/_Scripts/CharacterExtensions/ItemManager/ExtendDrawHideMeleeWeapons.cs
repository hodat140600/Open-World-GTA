﻿using _GAME._Scripts;
using UnityEngine;

namespace _GAME._Scripts
{
    using _GAME._Scripts;
    using _GAME._Scripts._CharacterController;
    using _ItemManager;

    [ClassHeader("Draw/Hide Melee Weapons", "This component works with vItemManager, vWeaponHolderManager and vMeleeCombatInput", useHelpBox = true)]
    public class ExtendDrawHideMeleeWeapons : ExtendMonoBehaviour
    {
        [EditorToolbar("Default")]
        public bool hideWeaponsAutomatically = true;
        [_Scripts.LeoHideInInspector("hideWeaponsAutomatically")]
        public float hideWeaponsTimer = 5f;
        [HelpBox("Set Lock input to Inventory when Lock method is called")]
        public bool lockInventoryInputOnLock;
        [LeoReadOnly]
        public bool isLocked;
        public GenericInput hideAndDrawWeaponsInput = new GenericInput("H", "LB", "LB");

        [EditorToolbar("Melee")]
        [Header("Draw Immediate Conditions")]
        public bool meleeWeakAttack = true;
        public bool meleeStrongAttack = true;
        public bool meleeBlock = true;

        [EditorToolbar("Debug")]
        [LeoReadOnly(false)]
        public bool weaponsHided;
        [LeoReadOnly(false)]
        public bool previouslyWeaponsHided;

        protected float currentTimer;
        protected bool forceHide;

        protected virtual void Start()
        {
            holderManager = GetComponent<WeaponHolderManager>();
            melee = GetComponent<MeleeCombatInput>();

            if (holderManager && melee)
            {
                melee.onUpdate -= ControlWeapons;
                melee.onUpdate += ControlWeapons;
                if (melee == null) Debug.LogWarning("You're missing a vMeleeCombatInput, please add one", gameObject);
            }
        }

        protected virtual void ControlWeapons()
        {
            if (isLocked || melee.cc == null || melee.cc.customAction)
                return;

            HandleInput();
            DrawWeaponsImmediateHandle();
            HideWeaponsAutomatically();
        }

        protected virtual GameObject RightWeaponObject(bool checkIsActve = false)
        {
            return melee && melee.meleeManager && melee.meleeManager.rightWeapon &&
                  (!checkIsActve || melee.meleeManager.rightWeapon.gameObject.activeInHierarchy) ? melee.meleeManager.rightWeapon.gameObject : null;
        }

        protected virtual GameObject LeftWeaponObject(bool checkIsActve = false)
        {
            return melee && melee.meleeManager && melee.meleeManager.leftWeapon &&
                (!checkIsActve || melee.meleeManager.leftWeapon.gameObject.activeInHierarchy) ? melee.meleeManager.leftWeapon.gameObject : null;
        }


        public virtual MeleeCombatInput melee { get; set; }

        public virtual WeaponHolderManager holderManager { get; set; }

        public virtual void ReturnToLastState(bool immediate = false)
        {
            if (previouslyWeaponsHided)
            {
                HideWeapons(immediate);
            }
            else DrawWeapons(immediate);
        }

        public virtual void LockDrawHideInput(bool value)
        {
            isLocked = value;
            if (lockInventoryInputOnLock && holderManager.itemManager)
                holderManager.itemManager.LockInventoryInput(value);
        }

        public virtual void HideWeapons(bool immediate = false)
        {
            previouslyWeaponsHided = weaponsHided;
            if (CanHideRightWeapon())
            {
                weaponsHided = true;
                HideRightWeapon(immediate);
            }

            else if (CanHideLeftWeapon())
            {
                weaponsHided = true;
                HideLeftWeapon(immediate);
            }
        }

        public virtual void ForceHideWeapons(bool immediate = false)
        {
            forceHide = true;
            HideWeapons(immediate);
            Invoke("ResetForceHide", 1);
        }

        protected virtual void ResetForceHide()
        {
            forceHide = false;
        }

        public virtual void DrawWeapons(bool immediate = false)
        {
            if (CanDrawRightWeapon())
            {
                previouslyWeaponsHided = weaponsHided;
                weaponsHided = false;
                DrawRightWeapon(immediate);
            }
            else if (CanDrawLeftWeapon())
            {
                previouslyWeaponsHided = weaponsHided;
                weaponsHided = false;
                DrawLeftWeapon(immediate);
            }
        }

        protected virtual void HideWeaponsAutomatically()
        {
            if (hideWeaponsAutomatically)
            {
                if (HideTimerConditions())
                {
                    currentTimer += Time.deltaTime;
                }
                else currentTimer = 0;

                if (currentTimer >= hideWeaponsTimer && !IsEquipping)
                {
                    currentTimer = 0;
                    HideWeapons();
                }
            }
            else if (currentTimer > 0) currentTimer = 0;
        }

        protected virtual bool HideTimerConditions()
        {
            return CanHideWeapons() && (CanHideRightWeapon() || CanHideLeftWeapon());
        }

        protected virtual bool CanHideWeapons()
        {
            return melee && melee.meleeManager && (forceHide || !melee.isAttacking && !melee.isBlocking && (melee.meleeManager.rightWeapon || melee.meleeManager.leftWeapon));
        }

        protected virtual bool CanDrawWeapons()
        {
            return melee && melee.meleeManager;
        }

        protected virtual bool CanHideRightWeapon()
        {
            return CanHideWeapons() && RightWeaponObject() && RightWeaponObject().activeInHierarchy;
        }

        protected virtual bool CanHideLeftWeapon()
        {
            return CanHideWeapons() && LeftWeaponObject() && LeftWeaponObject().activeInHierarchy;
        }

        protected virtual bool CanDrawRightWeapon()
        {
            return CanDrawWeapons() && RightWeaponObject() && !RightWeaponObject().activeInHierarchy;
        }

        protected virtual bool CanDrawLeftWeapon()
        {
            return CanDrawWeapons() && LeftWeaponObject() && !LeftWeaponObject().activeInHierarchy;
        }

        protected virtual bool IsEquipping
        {
            get
            {
                return melee != null && melee.cc && melee.cc.IsAnimatorTag("IsEquipping");
            }
        }

        protected virtual void HandleInput()
        {
            if (hideAndDrawWeaponsInput.GetButtonDown() && !IsEquipping)
            {

                if (CanHideRightWeapon() || CanHideLeftWeapon())
                {
                    HideWeapons();
                }
                else if (CanDrawRightWeapon() || CanDrawLeftWeapon())
                {
                    DrawWeapons();
                }

            }
        }

        protected virtual void DrawWeaponsImmediateHandle()
        {
            if (DrawWeaponsImmediateConditions())
            {
                DrawWeapons(true);
            }
        }

        protected virtual bool DrawWeaponsImmediateConditions()
        {
            if (!melee || melee.cc.customAction || !melee.meleeManager || melee.meleeManager.CurrentAttackWeapon == null && melee.meleeManager.CurrentDefenseWeapon == null)
                return false;
            else
            {
                return melee.weakAttackInput.GetButton() && meleeWeakAttack || melee.strongAttackInput.GetButton() && meleeStrongAttack || melee.blockInput.GetButton() && meleeBlock;
            }
        }

        protected virtual void HideRightWeapon(bool immediate = false)
        {
            var weapon = RightWeaponObject(true);
            if (weapon)
            {
                var equipment = weapon.GetComponent<Equipment>();
                if (equipment == null || equipment.equipPoint == null || equipment.equipPoint.area == null)
                {
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                HideWeaponsHandle(melee, equipment,
                null,
                () =>
                {
                    if (holder) holder.SetActiveWeapon(true);
                    if (weapon) weapon.gameObject.SetActive(false);
                    if (CanHideLeftWeapon()) HideLeftWeapon(immediate);
                }, immediate);
            }
        }

        protected virtual void HideLeftWeapon(bool immediate = false)
        {
            var weapon = LeftWeaponObject(true);
            if (weapon)
            {
                var equipment = weapon.GetComponent<Equipment>();
                if (equipment == null || equipment.equipPoint == null || equipment.equipPoint.area == null)
                {
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                HideWeaponsHandle(melee, equipment,
                null,
                () =>
                {
                    if (holder) holder.SetActiveWeapon(true);
                    if (weapon) weapon.gameObject.SetActive(false);
                }, immediate);
            }
        }

        protected virtual void DrawRightWeapon(bool immediate = false)
        {
            var weapon = RightWeaponObject();
            if (weapon)
            {
                var equipment = weapon.GetComponent<Equipment>();
                if (equipment == null || equipment.equipPoint == null || equipment.equipPoint.area == null)
                {
                    return;
                }
                if (equipment.equipPoint.area.isLockedToEquip) return;

                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                DrawWeaponsHandle(melee, equipment, null,
                                                            () =>
                                                            {
                                                                if (holder) holder.SetActiveWeapon(false);
                                                                if (weapon && weapon.gameObject)
                                                                    weapon.gameObject.SetActive(true);
                                                                if (CanDrawLeftWeapon())
                                                                    DrawLeftWeapon(immediate);
                                                            }, immediate);
            }
        }

        protected virtual void DrawLeftWeapon(bool immediate = false)
        {
            var weapon = LeftWeaponObject();
            if (weapon)
            {
                var equipment = weapon.GetComponent<Equipment>();
                if (equipment == null || equipment.equipPoint == null || equipment.equipPoint.area == null)
                {
                    return;
                }
                if (equipment.equipPoint.area.isLockedToEquip) return;

                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);

                DrawWeaponsHandle(melee, equipment, null,
                                                        () =>
                                                        {
                                                            if (holder) holder.SetActiveWeapon(false);
                                                            if (weapon && weapon.gameObject)
                                                                weapon.gameObject.SetActive(true);
                                                        }, immediate);
            }
        }

        protected virtual void DrawWeaponsHandle(ThirdPersonInput tpInput, Equipment equipment, UnityEngine.Events.UnityAction onStart, UnityEngine.Events.UnityAction onFinish, bool immediate = false)
        {
            if (holderManager.inEquip) return;
            if (!immediate)
            {
                if (!string.IsNullOrEmpty(equipment.referenceItem.EnableAnim) && equipment != null && equipment.equipPoint != null)
                {
                    tpInput.animator.SetBool("FlipEquip", equipment.equipPoint.equipPointName.Contains("Left"));
                    tpInput.animator.CrossFade(equipment.referenceItem.EnableAnim, 0.25f);
                }
                else immediate = true;
            }
            StartCoroutine(holderManager.EquipRoutine(equipment.referenceItem.enableDelayTime, immediate, onStart, onFinish));
        }

        protected virtual void HideWeaponsHandle(ThirdPersonInput tpInput, Equipment equipment, UnityEngine.Events.UnityAction onStart, UnityEngine.Events.UnityAction onFinish, bool immediate = false)
        {
            if (holderManager.inUnequip) return;
            if (!immediate)
            {
                if (!string.IsNullOrEmpty(equipment.referenceItem.DisableAnim) && equipment != null && equipment.equipPoint != null)
                {
                    tpInput.animator.SetBool("FlipEquip", equipment.equipPoint.equipPointName.Contains("Left"));
                    tpInput.animator.CrossFade(equipment.referenceItem.DisableAnim, 0.25f);
                }
                else immediate = true;
            }
            StartCoroutine(holderManager.UnequipRoutine(equipment.referenceItem.disableDelayTime, immediate, onStart, onFinish));
        }
    }
}

