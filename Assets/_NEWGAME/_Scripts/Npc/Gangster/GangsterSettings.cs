using _GAME._Scripts.Npc;
using UnityEngine;

[CreateAssetMenu(fileName = "Gangster", menuName = "NPC/Gangster")]
public class GangsterSettings : NpcSettings
{
    public Gangster gangster;
    public override INpc Npc => gangster;
}