using Assets._SDK.Skills;
using UnityEngine;

public class BeBraveSkillBehavior : MonoBehaviour
{
    [SerializeField]
    private LevelType levelType;

    private Animator            _animator;
    private AIMoveSkillBehavior aIMoveSkillBehavior;
    // private LiveSkillBehavior   liveSkillBehavior;
    private ShootSkillBehavior  shootSkillBehavior;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        levelType = LevelType.Weak;
    }

    private void Start()
    {
        aIMoveSkillBehavior = GetComponent<AIMoveSkillBehavior>();
        // liveSkillBehavior   = GetComponent<LiveSkillBehavior>();
        shootSkillBehavior  = GetComponent<ShootSkillBehavior>();
    }

    public void React(GameObject attacker, float damage)
    {
        SetAnim();
        switch (levelType)
        {
            case LevelType.Weak:
                // aIMoveSkillBehavior.RunAwayFrom(attacker.transform);
                break;
            case LevelType.Medium:
                ////AIMoveSkillBehavior.RunAwayFrom(attacker); - khi con nua mau, va o lai khi con tren nua mau
                break;
            case LevelType.Strong:
                // aIMoveSkillBehavior.ChaseAndAttack(attacker.transform);
                //aIMoveSkillBehavior.RunTo(attacker.transform);
                //shootSkillBehavior.ActiveShoot(attacker.transform);                
                break;
        }
    }


    public void LevelUp(BeBraveSkillLevel beBraveSkillLevel)
    {
        levelType = (LevelType)beBraveSkillLevel.Index;
    }

    private void SetAnim()
    {
        _animator.SetBool("die", false);
        _animator.SetBool("walk", false);
        _animator.SetBool("hit", true);
    }
}