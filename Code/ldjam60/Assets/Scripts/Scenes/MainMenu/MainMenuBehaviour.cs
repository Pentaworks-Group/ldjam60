using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
	public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
	{
        [SerializeField]
        private GameObject howTo;

        public void OnOpenGameMode()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(Constants.Scenes.GameModeEdit);
        }

        public void ShowHowto()
        {
            Base.Core.Game.PlayButtonSound();
            howTo.SetActive(true);
        }

        public void HideHowto()
        {
            Base.Core.Game.PlayButtonSound();
            howTo.SetActive(false);
        }

        public void ShowCredits()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(Constants.Scenes.Credits);
        }

        public void OnQuit()
        {
            Base.Core.Game.Quit();
        }
    }
}
