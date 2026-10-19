using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
    {
        [SerializeField]
        private GameObject howTo;

        [SerializeField]
        private GameObject quitSign;

        [SerializeField]
        private TMP_Text versionText;

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

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer
                || Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.quitSign.SetActive(false);
            }

            this.versionText.text = $"Version: {Application.version}";
        }
    }
}
