using _GAME._Scripts.GameResources;
using UnityEngine;

namespace _GAME._Scripts.Game
{
    [RequireComponent(typeof(NpcResources))]
    [RequireComponent(typeof(WeaponResources))]
    [RequireComponent(typeof(MissionResources))]
    // [RequireComponent(typeof(CharacterResources))]
    [RequireComponent(typeof(NpcGroupResource))]
    [RequireComponent(typeof(MapResources))]

    public class GameResources : MonoBehaviour
    {
        public NpcResources NpcResources { get; private set; }
        public WeaponResources WeaponResources { get; private set; }
        public MissionResources MissionResources { get; private set; }
        // public CharacterResources CharacterResources { get; private set; }
        public NpcGroupResource NpcGroupResources { get; private set; }
        public MapResources MapResources { get; private set; }


        private void OnValidate()
        {
            WeaponResources   ??= GetComponent<WeaponResources>();
            MissionResources  ??= GetComponent<MissionResources>();
            NpcResources      ??= GetComponent<NpcResources>();
            NpcGroupResources ??= GetComponent<NpcGroupResource>();
            MapResources      ??= GetComponent<MapResources>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            OnValidate();
        }
    }
}