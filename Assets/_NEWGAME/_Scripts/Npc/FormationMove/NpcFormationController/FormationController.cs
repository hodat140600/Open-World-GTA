using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts;
using _GAME._Scripts.Npc.Group;
using TRavljen.UnitFormation;
using TRavljen.UnitFormation.Formations;
using UnityEngine;

public enum FormationType
{
    Line,
    Triangle,
    Rectangle,
    Circle
}

public class FormationController
{
    private List<NpcGroup> Units => SpawnerController.groups;
    private SpawnerController SpawnerController => Gameplay.Instance.spawnerController;


    private void ApplyFormationTo(NpcGroup group, Vector3 destination, Vector3 facingPosition, float moveSpeed)
    {
        Vector3                 direction    = facingPosition - destination;
        float                   angle        = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        List<Vector3>           newPositions = FormationPositioner.GetAlignedPositions(group.NpcList.Count, group.currentFormation, destination, angle);
        UnitsFormationPositions formationPos = new(newPositions, angle);

        UpdateGroupSpeed(group, moveSpeed, formationPos);
    }

    public void UpdateGroupSpeed(NpcGroup group, float moveSpeed, UnitsFormationPositions formationPos)
    {
        for (int i = 0; i < group.NpcList.Count; i++)
            if (group.NpcList[i].TryGetComponent(out AIMoveSkillBehavior aiMoveSkillBehavior))
                aiMoveSkillBehavior.SetDestinationAndSpeed(formationPos.UnitPositions[i], moveSpeed);
    }

    public void ApplyFormation(int leaderId, Vector3 destination, Vector3 facingPosition, float moveSpeed)
    {
        NpcGroup group = Units.First(group => leaderId == group.leaderId);
        ApplyFormationTo(group, destination, facingPosition, moveSpeed);
    }

    public IFormation GetFormation(FormationType formationType, float unitSpacing) => formationType switch
    {
        FormationType.Triangle => new TriangleFormation(unitSpacing),
        FormationType.Circle   => new CircleFormation(unitSpacing),
        FormationType.Line     => new LineFormation(unitSpacing),
        _                      => new TriangleFormation(unitSpacing)
    };
}