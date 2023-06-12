using Assets._SDK.Input;
using System;
using _SDK.Entities;
using _SDK.Inventory;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public abstract class AbstractSkillSystem : MonoBehaviour
    {
        protected abstract ISkillSlotFactory SkillSlotFactory { get; set; }
        protected AbstractSkillSlot AttachSkillSlot(ISkill skill, int? levelIndex = null, IObservable observable = null)
        {

            AbstractSkillSlot skillSlot = SkillSlotFactory.CreateSkillSlotFor(skill);

            skillSlot.LevelUp(gameObject , levelIndex);

            if (observable != null && skillSlot.SkillBehavior is IObserver)
            {
                IObserver observerBehavior = (IObserver)skillSlot.SkillBehavior;
                IDisposable disposable = observerBehavior.Observe(observable);
                observable.AddDisposable(disposable);
            }

            return skillSlot;
        }
    }
}