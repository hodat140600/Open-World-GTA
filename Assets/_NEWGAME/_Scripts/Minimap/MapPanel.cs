using _GAME._Scripts;
using _NEWGAME._Scripts.Game;
using _SDK.UI;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : AbstractPanel
{
    [SerializeField] private Button        openButton, backButton;
    [SerializeField] private MinimapCamera minimapCamera;

    private void Start()
    {
        openButton.onClick.AddListener(() =>
        {
            if (Gameplay.Instance.IsInState(GameplayState.Running))
            {
                Gameplay.Instance.Fire(GameplayTrigger.ShowMap);
                SfxManager.Instance.Play(Sounds.ZoomIn);
            }
        });
        LetFireOnGameplayClick(backButton, GameplayTrigger.BackToPlaying,Sounds.Select);
    }
    
    // called by Animator
    public void ZoomOut()
    {
        minimapCamera.ZoomOut();
    }

    public void ZoomIn()
    {
        minimapCamera.ZoomIn();
    }
}