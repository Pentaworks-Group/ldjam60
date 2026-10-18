using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
	public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
	{
        public void OnOpenGameMode()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.Start();
        }

    }
}
