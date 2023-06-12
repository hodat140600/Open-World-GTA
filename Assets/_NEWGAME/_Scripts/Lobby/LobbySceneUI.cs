using _GAME._Scripts.Game;
using _SDK.UI;
using UniRx;

namespace _GAME.Scripts.Lobby
{
    public class LobbySceneUI : AbstractSceneUI
    {
        private bool SettingsPanel;
            // WeaponShopPanel

        private void Start()
        {
            Init();
            var currentState = GameManager.Instance.CurrentState;
            
            // currentState.Subscribe(state => ShowIf(nameof(WeaponShopPanel), state == GameState.LobbyHome)).AddTo(this);
            currentState.Subscribe(state => ShowIf(nameof(SettingsPanel), state == GameState.LobbySettings)).AddTo(this);
        }

        private void ShowIf(string panelName, bool isShown)
        {
            CallAnimationOnPanel(panelName, "isOpening", isShown);
        }
    }
}