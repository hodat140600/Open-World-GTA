using UnityEngine;
namespace _GAME._Scripts._CharacterController._Shooter
{
    /// <summary>
    /// Base Class used to create Impact Effect
    /// </summary>
    public abstract class ImpactEffectBase : ScriptableObject
    {
        /// <summary>
        /// Do Impact effect
        /// </summary>
        /// <param name="position">position of impact effect</param>
        /// <param name="rotation">rotation of impact effect</param>
        /// <param name="sender">Impact effect sender</param>
        /// <param name="receiver">Impact effect receiver</param>
        public abstract void DoImpactEffect(Vector3 position, Quaternion rotation, GameObject sender, GameObject receiver);
    }
}