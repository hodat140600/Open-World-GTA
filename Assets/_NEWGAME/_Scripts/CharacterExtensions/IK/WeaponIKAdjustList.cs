using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._CharacterController._Shooter
{
    [CreateAssetMenu(menuName = "DatHQ/Shooter/New Weapon IK Adjust List")]
    public class WeaponIKAdjustList : ScriptableObject
    {
        [Separator("Global Offsets for all Weapons Hand IK target")]
        public Vector3 ikTargetPositionOffsetR;
        public Vector3 ikTargetRotationOffsetR;
        public Vector3 ikTargetPositionOffsetL;
        public Vector3 ikTargetRotationOffsetL;
        [Separator("Offsets for specific Weapon categories")]
        public List<WeaponIKAdjust> weaponIKAdjusts = new List<WeaponIKAdjust>();




        public WeaponIKAdjust GetWeaponIK(string category)
        {
            return weaponIKAdjusts != null ? weaponIKAdjusts.Find(ik => ik != null && ik.weaponCategories.Contains(category)) : null;
        }

        public void ReplaceWeaponIKAdjust(WeaponIKAdjust currentIK, WeaponIKAdjust newIK)
        {
            if (weaponIKAdjusts != null && weaponIKAdjusts.Contains(currentIK))
            {
                int index = IndexOfIK(currentIK);
                weaponIKAdjusts[index] = newIK;
            }
        }

        public int IndexOfIK(WeaponIKAdjust currentIK)
        {
            if (weaponIKAdjusts != null && weaponIKAdjusts.Contains(currentIK))
            {
                int index = weaponIKAdjusts.IndexOf(currentIK);
                return index;
            }
            else return -1;
        }
    }
}