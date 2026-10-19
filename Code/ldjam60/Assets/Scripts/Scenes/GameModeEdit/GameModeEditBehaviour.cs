using Assets.Scripts.Core.Definitions;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameModeEdit
{
	public class GameModeEditBehaviour : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text SubtitleText;

        private GameMode gameMode;

        public void SetGameMode(GameMode gameMode)
		{ 
			var serialiedGameMode = Newtonsoft.Json.JsonConvert.SerializeObject(gameMode);
			var copiedGameMode = Newtonsoft.Json.JsonConvert.DeserializeObject<GameMode>(serialiedGameMode);

            this.gameMode = copiedGameMode;

            this.SubtitleText.text = gameMode.Name;
		}

		public void OnBack()
		{
			Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
		}

		public void StartGame()
		{
			Base.Core.Game.Start(gameMode);
		}
	}
}
