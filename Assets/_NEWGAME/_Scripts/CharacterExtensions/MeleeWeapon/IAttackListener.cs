using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._EventSystems
{
    public interface IAttackListener
    {
        void OnEnableAttack();

        void OnDisableAttack();

        void ResetAttackTriggers();
    }
}
