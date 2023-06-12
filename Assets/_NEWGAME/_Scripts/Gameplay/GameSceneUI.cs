using System.Collections;
using _NEWGAME._Scripts.GameTimer.UI;
using _SDK.UI;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using UniRx;
using UnityEngine;

namespace _GAME._Scripts.UI.Game
{
    public class GameSceneUI : AbstractSceneUI
    {
        private const string MapPanelDir              = "SafeArea/HUD/TopLeft/Minimap";
        private const string WastedPanelDir           = "SafeArea/Panels/WastedPanel";
        private const string MissionCompletedPanelDir = "SafeArea/Panels/MissionCompletedPanel";
        private const string SettingsPanelDir         = "SafeArea/Panels/SettingsPanel";
        private const string ScorePanelDir            = "SafeArea/Panels/ScorePanel";
        private const string RevivePanelDir           = "SafeArea/Panels/RevivePanel";
        private const string PreventInputPanelDir      = "SafeArea/Panels/PreventInputPanel";

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            Init();

            ReactiveProperty<GameplayState> currentState = Gameplay.Instance.CurrentState;

            currentState.Subscribe(state => ShowIf(MapPanelDir, state is GameplayState.ShowingMap));
            currentState.Subscribe(state => ShowIf(SettingsPanelDir, state is GameplayState.ShowingSettings));

            ReactiveProperty<MissionState> currentMissionState = Gameplay.Instance.MissionManager.CurrentMissionState;
            // currentMissionState.Subscribe(state => ShowIf(MissionCompletedPanelDir, state is MissionState.DayEnded));
            currentMissionState.Subscribe(state => ShowIf(RevivePanelDir, state is MissionState.Failed));
            currentMissionState.Subscribe(state => ShowIf(ScorePanelDir, state is MissionState.DayEnded));

            MissionManager.PreventInputIf += ShowPreventInputPanelIf;
        }

        private void ShowPreventInputPanelIf(bool isShown)
        {
            GetOrAddPanel(PreventInputPanelDir).gameObject.SetActive(isShown);
        }

        private void ShowIf(string panelName, bool isShown)
        {
            CallAnimationOnPanel(panelName, "isOpening", isShown);
        }

        private void ActiveIf(string panelName, bool isShown)
        {
            GetOrAddPanel(panelName).gameObject.SetActive(isShown);
        }
    }
}