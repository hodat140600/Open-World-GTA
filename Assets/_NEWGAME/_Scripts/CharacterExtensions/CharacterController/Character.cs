using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace _GAME._Scripts._CharacterController
{
    [System.Serializable]
    public class OnActionHandle : UnityEvent<Collider> { }
    [System.Serializable]

    [ClassHeader("Character")]
    public class Character : HealthController, ICharacter
    {
        #region Character Variables 

        public enum DeathBy
        {
            Animation,
            AnimationWithRagdoll,
            Ragdoll
        }

        [EditorToolbar("Health")]
        public DeathBy deathBy = DeathBy.Animation;
        public bool removeComponentsAfterDie;

        [EditorToolbar("Debug", order = 9)]
        [@HideInInspector]
        public bool debugActionListener;
        public Animator animator { get; protected set; }
        public bool ragdolled { get; set; }

        [EditorToolbar("Events")]

        public UnityEvent OnCrouch;
        public UnityEvent OnStandUp;

        [SerializeField] protected OnActiveRagdoll _onActiveRagdoll = new OnActiveRagdoll();
        public OnActiveRagdoll onActiveRagdoll { get { return _onActiveRagdoll; } protected set { _onActiveRagdoll = value; } }
        public UnityEvent onDisableRagdoll;
        [Header("Check if Character is in Trigger with tag Action")]
        [@HideInInspector]
        public OnActionHandle onActionEnter = new OnActionHandle();
        [@HideInInspector]
        public OnActionHandle onActionStay = new OnActionHandle();
        [@HideInInspector]
        public OnActionHandle onActionExit = new OnActionHandle();

        protected AnimatorParameter hitDirectionHash;
        protected AnimatorParameter reactionIDHash;
        protected AnimatorParameter triggerReactionHash;
        protected AnimatorParameter triggerResetStateHash;
        protected AnimatorParameter recoilIDHash;
        protected AnimatorParameter triggerRecoilHash;

        protected bool isInit;

        public virtual bool isCrouching
        {
            get
            {
                return _isCrouching;
            }
            set
            {
                if (value != _isCrouching)
                {
                    if (value)
                        OnCrouch.Invoke();
                    else
                        OnStandUp.Invoke();
                }
                _isCrouching = value;
            }
        }

        protected bool _isCrouching;

        #endregion        

        public virtual void Init()
        {
            animator = GetComponent<Animator>();
            if (animator)
            {
                hitDirectionHash = new AnimatorParameter(animator, "HitDirection");
                reactionIDHash = new AnimatorParameter(animator, "ReactionID");
                triggerReactionHash = new AnimatorParameter(animator, "TriggerReaction");
                triggerResetStateHash = new AnimatorParameter(animator, "ResetState");
                recoilIDHash = new AnimatorParameter(animator, "RecoilID");
                triggerRecoilHash = new AnimatorParameter(animator, "TriggerRecoil");
            }

            this.LoadActionControllers(debugActionListener);
        }

        public virtual void ResetRagdoll()
        {

        }

        public virtual void EnableRagdoll()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            onActionEnter.Invoke(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            onActionStay.Invoke(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            onActionExit.Invoke(other);
        }

        public override void TakeDamage(Damage damage)
        {
            base.TakeDamage(damage);
            TriggerDamageReaction(damage);
        }

        protected virtual void TriggerDamageReaction(Damage damage)
        {
            if (animator != null && animator.enabled && !damage.activeRagdoll && CurrentHealth > 0)
            {
                if (hitDirectionHash.isValid && damage.sender) animator.SetInteger(hitDirectionHash, (int)transform.HitAngle(damage.sender.position));

                // trigger hitReaction animation
                if (damage.hitReaction)
                {
                    // set the ID of the reaction based on the attack animation state of the attacker - Check the MeleeAttackBehaviour script
                    if (reactionIDHash.isValid) animator.SetInteger(reactionIDHash, damage.reaction_id);
                    if (triggerReactionHash.isValid) SetTrigger(triggerReactionHash);
                    if (triggerResetStateHash.isValid) SetTrigger(triggerResetStateHash);
                }
                else
                {
                    if (recoilIDHash.isValid) animator.SetInteger(recoilIDHash, damage.recoil_id);
                    if (triggerRecoilHash.isValid) SetTrigger(triggerRecoilHash);
                    if (triggerResetStateHash.isValid) SetTrigger(triggerResetStateHash);
                }
            }
            if (damage.activeRagdoll)
                onActiveRagdoll.Invoke(damage);
        }

        private IEnumerator SetTriggerRoutine(int trigger)
        {
            animator.SetTrigger(trigger);
            yield return new WaitForSeconds(0.1f);
            animator.ResetTrigger(trigger);
        }

        public virtual void SetTrigger(int trigger)
        {
            StartCoroutine(SetTriggerRoutine(trigger));
        }
    }
}