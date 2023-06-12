using _GAME._Scripts.Inventory;
using _GAME._Scripts.Npc;

namespace _GAME._Scripts
{
    public class NpcKilledEvent : IMissionEvent
    {
        public readonly ModelType  Type;
        public          WeaponType KilledByWeapon;
        public          bool       IsHeadshot;

        public NpcKilledEvent(ModelType type, bool isHeadshot, WeaponType killedByWeapon)
        {
            Type         = type;
            IsHeadshot   = isHeadshot;
            KilledByWeapon = killedByWeapon;
        }
    }
}