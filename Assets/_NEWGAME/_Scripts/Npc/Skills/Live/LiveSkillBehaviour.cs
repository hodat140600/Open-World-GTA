using _GAME._Scripts;
using _GAME._Scripts.Inventory;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.SkillSystem;
using UniRx;
using UnityEngine;

public class LiveSkillBehaviour : HealthController
{
    public void LevelUp(LiveSkillLevel liveSkillLevel)
    {
        ChangeMaxHealth(liveSkillLevel.maxHealth);
    }

    public override void ChangeMaxHealth(int value)
    {
        maxHealth = value;
        if (maxHealth < 0)
            maxHealth = 0;
    }

    protected override void Start()
    {
        base.Start();
        onDead.AddListener(ActionOnDied);
    }

    void ActionOnDied(GameObject target)
    {
        if (TryGetComponent(out NpcSkillSystem skillSystem))
        {
            ModelType npc = skillSystem.npc.ModelType;
            MessageBroker.Default.Publish(new NpcKilledEvent(npc, false, WeaponType.Handgun));
            Debug.Log(target.name + "Died");
        }
    }
}