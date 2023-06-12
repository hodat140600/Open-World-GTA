using Extensions;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Animator _animator;
    private static readonly  int      Start  = Animator.StringToHash("Start");
    private static readonly  int      Finish = Animator.StringToHash("Finish");


    public QuestPanel Init(string description, string progress,string reward)
    {
        descriptionText.SetText(description);
        progressText.SetText(progress);
        rewardText.SetText(reward);
        _animator.SetTrigger(Start);
        return this;
    }

    public void UpdateProgress(string progress)
    {
        progressText.SetText(progress);
    }

    public void FinishProgress()
    {
        _animator.SetTrigger(Finish);
    }
}