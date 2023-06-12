using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME._Scripts
{
    [Serializable]
    public class Mission
    {
        public int order;

        [FormerlySerializedAs("Cash")]
        [FormerlySerializedAs("rewardCash")]
        [GUIColor(1f, 0.78f, 0.07f, 1f)]
        [SuffixLabel("$", true)]
        public int cash;

        [SerializeReference, HideReferenceObjectPicker]
        public List<SubMission> subMissions;
    }
}