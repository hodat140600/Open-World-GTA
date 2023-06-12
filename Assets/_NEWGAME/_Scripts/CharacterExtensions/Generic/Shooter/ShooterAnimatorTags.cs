using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts
{
    public static partial class AnimatorTags
    {
        [Tooltip("Use to identify Reloading animations")]
        public const string IsReloading = "IsReloading";
        [Tooltip("Use to Ignore IK while this animation is being played")]
        public const string IgnoreIK = "IgnoreIK";
        [Tooltip("Use to identify a Shooter Upperbody Pose animation")]
        public const string UpperbodyPose = "Upperbody Pose";
        [Tooltip("Use to identify a Throw animation")]
        public const string IsThrowing = "IsThrowing";


    }
}