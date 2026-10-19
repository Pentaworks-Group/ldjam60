using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class GameSettingsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField numHumansField;
        [SerializeField]
        private TMP_InputField numBotsField;
        [SerializeField]
        private Toggle autoRepeat;

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(Init);
        }

        private void Init()
        {
            if (Base.Core.Game.State?.GameMode != default)
            {
                var gameMode = Base.Core.Game.State.GameMode;

                numHumansField.text = gameMode.NumHumans.ToString();
                numBotsField.text = gameMode.NumBots.ToString();
                autoRepeat.isOn = gameMode.AutoRepeat;
            }
        }

        public void ApplySettings()
        {
            if (int.TryParse(numHumansField.text, out int humanCount))
            {
                Debug.Log($"Humans: {humanCount}");
            }
            else
            {
                Debug.LogWarning("Invalid or empty input for Human count! Defaulting to 0.");
                humanCount = 0;
            }
            if (int.TryParse(numBotsField.text, out int botCount))
            {
                Debug.Log($"Bots: {botCount}");
            }
            else
            {
                Debug.LogWarning("Invalid or empty input for Bot count! Defaulting to 0.");
                botCount = 0;
            }

            var gameMode = Base.Core.Game.State.GameMode;

            gameMode.NumHumans = humanCount;
            gameMode.NumBots = botCount;
            gameMode.AutoRepeat = autoRepeat.isOn;

            Base.Core.Game.Start(gameMode);

        }
    }
}
