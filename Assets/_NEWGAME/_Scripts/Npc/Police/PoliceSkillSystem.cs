using MonKey.Extensions;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public class PoliceSkillSystem : NpcSkillSystem
    {
        public Police police;

        public override INpc npc => police;

        private void Start()
        {
            if (skillSlots.IsEmpty())
                InitPolice(police);
        }

        public void InitPolice(Police police)
        {

        }

        public override void UpdateSkillSlot<T>()
        {
            
        }
    }
}