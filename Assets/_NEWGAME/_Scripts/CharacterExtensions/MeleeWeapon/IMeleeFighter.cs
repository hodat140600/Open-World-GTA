using _GAME._Scripts._CharacterController;
using System.Collections;
using UnityEngine;

namespace _GAME._Scripts._EventSystems
{
    public interface IMeleeFighter : IAttackReceiver, IAttackListener
    {
        void BreakAttack(int breakAtkID);

        void OnRecoil(int recoilID);

        bool isAttacking { get; }

        bool isArmed { get; }

        bool isBlocking { get; }

        Transform transform { get; }

        GameObject gameObject { get; }

        ICharacter character { get; }
    }

    public static class IMeeleFighterHelper
    {
        /// <summary>
        /// check if gameObject has a <see cref="IMeleeFighter"/> Component
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns>return true if gameObject contains a <see cref="IMeleeFighter"/></returns>
        public static bool IsMeleeFighter(this GameObject receiver)
        {
            return receiver.GetComponent<IMeleeFighter>() != null;
        }

        /// <summary>
        /// Apply damage using OnReeiveAttack method if receiver dosent has a vIAttackReceiver, the Simple ApplyDamage is called
        /// </summary>
        /// <param name="receiver">target damage receiver</param>
        /// <param name="damage">damage</param>
        /// <param name="attacker">damage sender</param>
        public static void ApplyDamage(this GameObject receiver, Damage damage, IMeleeFighter attacker)
        {
            var attackReceiver = receiver.GetComponent<IAttackReceiver>();
            if (attackReceiver != null)
            {
                attackReceiver.OnReceiveAttack(damage, attacker);
            }
            else
            {
                receiver.ApplyDamage(damage);
            }
        }

        /// <summary>
        /// Get <see cref="IMeleeFighter"/> of gameObject
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns>the <see cref="IMeleeFighter"/> component</returns>
        public static IMeleeFighter GetMeleeFighter(this GameObject receiver)
        {
            return receiver.GetComponent<IMeleeFighter>();
        }
    }
}
