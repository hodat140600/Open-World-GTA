using MonKey.Extensions;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public class GangsterSkillSystem : NpcSkillSystem
    {
        public Gangster gangster;

        public override INpc npc => gangster;

        private void Start()
        {
            if (skillSlots.IsEmpty())
                InitGangster(gangster);
        }

        public void InitGangster(Gangster gangster)
        {

        }

        public override void UpdateSkillSlot<T>()
        {
            
        }
    }
}