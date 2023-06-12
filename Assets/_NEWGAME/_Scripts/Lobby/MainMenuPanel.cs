using _GAME._Scripts.Game;
using _NEWGAME._Scripts.Game;
using UnityEngine.UI;

namespace _SDK.UI
{
    public class MainMenuPanel : AbstractPanel
    {
        public Button playButton, settingsButton;

        private void Start()
        {
            LetFireOnClick(playButton, GameTrigger.Play, Sounds.Select);
            LetFireOnClick(settingsButton, GameTrigger.ShowSettings,Sounds.Select);
        }
    }
}