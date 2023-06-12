using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable]
    public abstract class SubMission
    {
#if UNITY_EDITOR
        [PropertySpace(3, 3)]
        [Button("THEN", ButtonSizes.Large)]
        [ShowIf("@this.blockBehind == false", false)]
        [HorizontalGroup("Button", .2f)]
        private void ThenOn()
        {
            blockBehind = true;
        }

        [PropertySpace(3, 3)]
        [Button("THEN", ButtonSizes.Large)]
        [ShowIf("@this.blockBehind == true", false)]
        [HorizontalGroup("Button", .2f)]
        [GUIColor(0.27f, 1f, 0f, 1f)]
        private void ThenOff()
        {
            blockBehind = false;
        }

        [PropertySpace(3, 3)]
        [Button("QUEST", ButtonSizes.Large)]
        [ShowIf("@this.hasQuest == false", false)]
        [HorizontalGroup("Button", .2f)]
        private void QuestOn()
        {
            hasQuest         = true;
            questDescription = DefaultQuestDescription;
        }

        [PropertySpace(3, 3)]
        [Button("QUEST", ButtonSizes.Large)]
        [ShowIf("@this.hasQuest == true", false)]
        [HorizontalGroup("Button", .2f)]
        [GUIColor(1f, 0.6f, 0.12f, 1f)]
        private void QuestOff()
        {
            hasQuest = false;
        }
#endif


        [HideInInspector]
        public bool blockBehind;

        [HideInInspector]
        public bool hasQuest;

        [PropertySpace(10, 3)]
        [ShowIf("@this.hasQuest == true", false)]
        [GUIColor(1f, 0.6f, 0.12f, 1f)]
        [HorizontalGroup("Button", .2f)]
        [HideLabel]
        [PropertyOrder(2)]
        public string questDescription;

        [PropertySpace(10, 3)]
        [ShowIf("@this.hasQuest == true", false)]
        [GUIColor(1f, 0.78f, 0.07f, 1f)]
        [HorizontalGroup("Button", .15f)]
        [PropertyOrder(2)]
        [SuffixLabel("$", true)]
        [HideLabel]
        public int rewardCash;

        public virtual string Progress => "";

        public bool Done { get; set; }

        public abstract void Accept(MissionMonitor monitor);

        protected virtual string DefaultQuestDescription => "";

        public virtual void ResetProgress()
        {
            Done = false;
        }
    }
}