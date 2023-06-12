using UnityEngine;

namespace _GAME._Scripts.Npc.Pedestrian
{
    [CreateAssetMenu(fileName = "Pedestrian", menuName = "NPC/Pedestrian")]
    public class PedestrianSettings : NpcSettings
    {
        public Pedestrian pedestrian;
        public override INpc Npc => pedestrian;
    }
}
