using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    [System.Serializable]
    public class OnActiveRagdoll : UnityEngine.Events.UnityEvent<Damage> { }
    public interface ICharacter : IHealthController
    {
        OnActiveRagdoll onActiveRagdoll { get; }
        Animator animator { get; }
        bool isCrouching { get; }
        bool ragdolled { get; set; }
        void EnableRagdoll();
        void ResetRagdoll();
    }
}