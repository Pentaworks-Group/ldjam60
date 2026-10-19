using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.GameModeEdit
{
    public class GameModeEditBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text SubtitleText;
        [SerializeField]
        private TMP_InputField playerAmoutInput;
        [SerializeField]
        private TMP_InputField botAmoutInput;
        [SerializeField]
        private Toggle autoReplayToggle;

        private GameMode gameMode;
        private List<GameMode> gameModes;
        private GameMode selectedGameMode;

        public void OnBack()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
        }

        public void StartGame()
        {
            var canStart = true;

            if (Int32.TryParse(this.playerAmoutInput.text, out var playerAmout))
            {
                gameMode.NumHumans = playerAmout;
            }
            else
            {
                canStart = false;
            }

            if (Int32.TryParse(this.botAmoutInput.text, out var botAmount))
            {
                gameMode.NumBots = botAmount;
            }
            else
            {
                canStart = false;
            }

            gameMode.AutoRepeat = this.autoReplayToggle.isOn;

            if (canStart)
            {
                Base.Core.Game.PlayButtonSound();
                Base.Core.Game.Start(gameMode);
            }
        }

        private void SetGameMode(GameMode gameMode)
        {
            var serialiedGameMode = Newtonsoft.Json.JsonConvert.SerializeObject(gameMode);
            var copiedGameMode = Newtonsoft.Json.JsonConvert.DeserializeObject<GameMode>(serialiedGameMode);

            this.gameMode = copiedGameMode;

            SetValues(gameMode);
        }

        private void SetValues(GameMode gameMode)
        {
            this.SubtitleText.text = gameMode.Name;
            this.playerAmoutInput.text = gameMode.NumHumans.ToString();
            this.botAmoutInput.text = gameMode.NumBots.ToString();
            this.autoReplayToggle.isOn = gameMode.AutoRepeat;
        }

        private void Awake()
        {
            this.gameModes = Base.Core.Game.GetGameModes();
            this.selectedGameMode = this.gameModes[0];

            if (this.gameModes.Count > 1)
            {
                // perhaps enable select.
            }

            SetGameMode(this.selectedGameMode);
        }
    }
}
