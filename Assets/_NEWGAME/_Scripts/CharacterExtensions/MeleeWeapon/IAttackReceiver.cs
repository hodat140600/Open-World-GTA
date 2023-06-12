using UnityEngine;
namespace _GAME._Scripts._EventSystems
{
    public interface IAttackReceiver
    {
        void OnReceiveAttack(Damage damage, IMeleeFighter attacker);
    }
}

