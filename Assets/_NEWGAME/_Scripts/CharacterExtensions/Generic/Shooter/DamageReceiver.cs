using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    using _EventSystems;
    [ClassHeader("DAMAGE RECEIVER", "You can add damage multiplier for example causing twice damage on Headshots", openClose = false)]
    public partial class DamageReceiver : ExtendMonoBehaviour, IAttackReceiver
    {
        public void OnReceiveAttack(Damage damage, IMeleeFighter attacker)
        {
            if (ragdoll && !ragdoll.iChar.isDead)
            {
                var _damage = ApplyDamageModifiers(damage);
                ragdoll.gameObject.ApplyDamage(_damage, attacker);
                onReceiveDamage.Invoke(_damage);
            }
            else if (targetReceiver)
            {
                var _damage = ApplyDamageModifiers(damage);
                targetReceiver.gameObject.ApplyDamage(_damage, attacker);
                onReceiveDamage.Invoke(_damage);
            }
            else
            {
                TakeDamage(damage);
            }
        }

    }
}