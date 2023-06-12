using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    [ClassHeader("DAMAGE RECEIVER", "You can add damage multiplier for example causing twice damage on Headshots", openClose = false)]
    public partial class DamageReceiver : ExtendMonoBehaviour, IDamageReceiver
    {

        [EditorToolbar("Default")]
        public float damageMultiplier = 1f;
        [@HideInInspector]
        public Ragdoll ragdoll;
        public bool overrideReactionID;
        [_Scripts.LeoHideInInspector("overrideReactionID")]
        public int reactionID;
        [EditorToolbar("Random")]
        public bool useRandomValues;
        [_Scripts.LeoHideInInspector("useRandomValues")]
        public bool fixedValues;
        [_Scripts.LeoHideInInspector("useRandomValues")]
        public float minDamageMultiplier, maxDamageMultiplier;
        [_Scripts.LeoHideInInspector("useRandomValues")]
        public int minReactionID, maxReactionID;
        [_Scripts.LeoHideInInspector("useRandomValues;fixedValues"), Tooltip("Change Between 0 and 100")]
        public float changeToMaxValue;
        public GameObject targetReceiver;
        public IHealthController healthController;
        [SerializeField] protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();
        [SerializeField] protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();
        public UnityEngine.Events.UnityEvent OnGetMaxValue;
        public OnReceiveDamage onStartReceiveDamage { get { return _onStartReceiveDamage; } protected set { _onStartReceiveDamage = value; } }
        public OnReceiveDamage onReceiveDamage { get { return _onReceiveDamage; } protected set { _onReceiveDamage = value; } }

        protected virtual void Start()
        {
            ragdoll = GetComponentInParent<Ragdoll>();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision != null)
            {
                if (ragdoll && ragdoll.isActive)
                {
                    ragdoll.OnRagdollCollisionEnter(new RagdollCollision(gameObject, collision));
                }
            }
        }

        public virtual void TakeDamage(Damage damage)
        {

            if (healthController == null && targetReceiver)
                healthController = targetReceiver.GetComponent<IHealthController>();
            else if (healthController == null)
                healthController = GetComponentInParent<IHealthController>();

            if (healthController != null)
            {
                onStartReceiveDamage.Invoke(damage);
                var _damage = ApplyDamageModifiers(damage);
                healthController.TakeDamage(_damage);
                onReceiveDamage.Invoke(_damage);
            }
        }

        public virtual Damage ApplyDamageModifiers(Damage damage)
        {
            float multiplier = useRandomValues && !fixedValues ? Random.Range(minDamageMultiplier, maxDamageMultiplier) :
                               useRandomValues && fixedValues ? randomChange ? maxDamageMultiplier : minDamageMultiplier : damageMultiplier;
            var _damage = new Damage(damage);
            _damage.damageValue *= (int)multiplier;
            if (multiplier == maxDamageMultiplier) OnGetMaxValue.Invoke();

            OverrideReaction(ref _damage);
            return _damage;
        }

        protected virtual void OverrideReaction(ref Damage damage)
        {
            if (overrideReactionID)
            {
                if (useRandomValues && !fixedValues) damage.reaction_id = Random.Range(minReactionID, maxReactionID);
                else if (useRandomValues && fixedValues) damage.reaction_id = randomChange ? maxReactionID : minReactionID;
                else
                    damage.reaction_id = reactionID;
            }
        }

        protected virtual bool randomChange
        {
            get
            {
                return Random.Range(0f, 100f) < changeToMaxValue;
            }
        }
    }
}