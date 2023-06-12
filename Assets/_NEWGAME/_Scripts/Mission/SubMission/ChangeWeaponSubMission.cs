using Sirenix.OdinInspector;

namespace _GAME._Scripts
{
    [Title("CHANGE WEAPON")]
    public class ChangeWeaponSubMission : SubMission
    {
        public override void Accept(MissionMonitor monitor)
        {
            monitor.ShowChangeWeapon(this);
        }
    }
}