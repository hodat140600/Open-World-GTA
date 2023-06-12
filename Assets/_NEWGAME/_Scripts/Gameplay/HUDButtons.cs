using _NEWGAME._Scripts.Game;
using _SDK.UI;
using UnityEngine.UI;

namespace _GAME._Scripts.UI.Game
{
    public class HUDButtons : AbstractPanel
    {
        public Button settingsButton;

        private void Start()
        {
            LetFireOnGameplayClick(settingsButton, GameplayTrigger.ShowSettings, Sounds.Select);
        }
    }
}