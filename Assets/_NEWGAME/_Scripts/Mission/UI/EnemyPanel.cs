using _GAME._Scripts.Npc;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NEWGAME._Scripts.Mission.UI
{
    public class EnemyPanel : MonoBehaviour
    {
        [field: SerializeField] private Animator animator { get; set; }
        public ModelType modelType { get; private set; }

        [SerializeField] private Image    image;
        [SerializeField] private TMP_Text countText;


        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int OutHash = Animator.StringToHash("Out");

        public void Init(ModelType modelType, Sprite avatar)
        {
            this.modelType = modelType;
            image.sprite   = avatar;
        }

        public void Out()
        {
            if (animator)
                animator.SetBool(OutHash, true);
        }

        public void Die()
        {
            if (animator)
                animator.SetBool(DieHash, true);
        }

        public void UpdateCount(int count)
        {
            countText.DOScale(1.4f, .08f).From(1f).OnComplete(() =>
            {
                countText.SetText($"{count}");
                countText.DOScale(1f, .08f).From(1.4f);
            });
        }
    }
}