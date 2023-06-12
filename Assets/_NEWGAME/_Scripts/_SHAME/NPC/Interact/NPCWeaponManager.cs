using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWeaponManager : MonoBehaviour
{
    public NPCWeapons weapon;

    public void HideWeapon()
    {
        weapon.weaponObject.SetActive(false);
    }
    public void ShowWeapon()
    {
        weapon.weaponObject.SetActive(true);    
    }
}
