using _GAME._Scripts;
using _GAME._Scripts._Melee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualSwordExample : ExtendMonoBehaviour
{
    public MeleeWeapon secundaryWeaponPrefab;
    public string otherSideHandlerName;
    [LeoReadOnly, SerializeField] protected MeleeWeapon secundaryWeapon;
    [LeoReadOnly, SerializeField] protected Transform otherSideTransform;
    [LeoReadOnly, SerializeField] protected MeleeManager manager;

    private void Start()
    {
        OnEnable();
    }

    private void OnDestroy()
    {
        OnDisable();
        if (secundaryWeapon) Destroy(secundaryWeapon.gameObject);

    }

    private void OnEnable()
    {
        if (!otherSideTransform)
        {
            Animator animator = GetComponentInParent<Animator>();
            if (animator)
            {
                var _otherSideHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
                var childrens = _otherSideHand.GetComponentsInChildren<Transform>();
                for (int i = 0; i < childrens.Length; i++)
                    if (childrens[i].gameObject.name.Equals(otherSideHandlerName))
                    {
                        otherSideTransform = childrens[i]; break;
                    }
            }

        }
        if (otherSideTransform)
        {
            ActiveSecundaryWeapon();
        }
    }

    private void ActiveSecundaryWeapon()
    {
        if (secundaryWeapon)
        {
            secundaryWeapon.gameObject.SetActive(true);
        }
        else
        {
            secundaryWeapon = Instantiate(secundaryWeaponPrefab);
            secundaryWeapon.transform.parent = otherSideTransform;
            secundaryWeapon.transform.localPosition = Vector3.zero;
            secundaryWeapon.transform.localEulerAngles = Vector3.zero;

        }
        if (!manager) manager = GetComponentInParent<MeleeManager>();
        if (manager)
        {
            manager.SetLeftWeapon(secundaryWeapon);
        }
    }

    private void OnDisable()
    {
        if (secundaryWeapon)
        {
            secundaryWeapon.gameObject.SetActive(false);
            manager.leftWeapon = null;
        }
    }
}
