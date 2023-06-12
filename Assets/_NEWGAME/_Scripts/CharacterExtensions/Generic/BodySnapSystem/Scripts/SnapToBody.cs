using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToBody : MonoBehaviour
{
    public const string manuallyAssignBone = "ManuallyAssign";
    public BodySnappingControl bodySnap;
    public Transform boneToSnap;
    public string boneName;

    private void Start()
    {
        bodySnap = transform.root.GetComponentInChildren<BodySnappingControl>(true);
        if (boneName != manuallyAssignBone)
        {
            if (bodySnap != null && bodySnap.boneSnappingList != null)
            {
                boneToSnap = bodySnap.GetBone(boneName);               
            }
        }

        if (boneToSnap)
        {
            transform.parent = boneToSnap;           
        }
    }    
}
