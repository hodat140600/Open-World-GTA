using _GAME._Scripts.Game;
using _SDK.Money;
using _SDK.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts
{
    public class MissionCompletedPanel : AbstractPanel
    {
        [SerializeField] private ParticleSystem winParticle;
        [SerializeField] private Button         collectx3Button, collectButton;
        [SerializeField] private TMP_Text       rewardText;
        
        private int rewardCash;

        private void Start()
        {
            Gameplay.Instance.MissionManager.CurrentMissionState
                .Where(state => state == MissionState.Running)
                .Subscribe(_ => { SetReward(Gameplay.Instance.MissionManager.CurrentMission.cash); }).AddTo(this);

            collectx3Button.onClick.AddListener(CollectX3Reward);
            collectButton.onClick.AddListener(CollectReward);
        }

        private void SetReward(int newReward)
        {
            rewardCash = newReward;
            rewardText.SetText(GameFormat.RewardCash(rewardCash));
        }

        private void CollectX3Reward()
        {
           
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.NextDay);
            // Gameplay.Instance.Fire(GameplayTrigger.BackToPlaying);
        }

        public void CollectReward()
        {
            GameManager.Instance.Wallet.Deposit(rewardCash);

            // Gameplay.Instance.Fire(GameplayTrigger.BackToPlaying);
        }

        // Called by Animator
        private void PlayParticle()
        {
            Instantiate(winParticle);
        }
    }
}