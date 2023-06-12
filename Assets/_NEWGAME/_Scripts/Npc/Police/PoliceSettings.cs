using _GAME._Scripts.Npc;
using UnityEngine;

[CreateAssetMenu(fileName = "Police", menuName = "NPC/Police")]
public class PoliceSettings : NpcSettings
{
    public Police police;
    public override INpc Npc => police;
}