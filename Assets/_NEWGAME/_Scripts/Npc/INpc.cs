using _SDK.Entities;
using Sirenix.OdinInspector;

namespace _GAME._Scripts.Npc
{
    [GUIColor(.6f, .6f, 1f)]
    public enum ModelType
    {
        SwimsuitMale   = 0,
        SwimsuitFemale = 1,
        CasualMale     = 2,
        CasualFemale   = 3,
        OfficeMale     = 4,
        OfficeFemale   = 5,
        WorkerMale     = 6,
        Police         = 7,
        Gangster       = 8,
        AngryMunci     = 50,
        Armstrong      = 51,
        BingChilling   = 52,
        Grudges        = 53,
        Juggler        = 54,
        Obunga         = 55,
        Pinheadditto   = 56,
        Quandaledingle = 57,
        SeleneDelgado  = 58,
        SuperIdol      = 59,
        Yoshie         = 60,
        ZhongXina      = 61,
        Nerd           = 63,
    }

    public interface INpc : IEntity
    {
        public ModelType ModelType { get; }
    }
}