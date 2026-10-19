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
            Base.Core.Game.Start();
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

    }
}
