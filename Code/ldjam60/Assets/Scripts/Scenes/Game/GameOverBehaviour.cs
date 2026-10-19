using Assets.Scripts.Core.Models;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class GameOverBehaviour : MonoBehaviour
    {
        [SerializeField] GameObject contentContainer;
        [SerializeField] TMP_Text winnerText;
        [SerializeField] TMP_Text congratuloryText;
        [SerializeField] TMP_Text replayButtonText;
        [SerializeField] TMP_Text quitButtonText;

        public void SetWinner(Player player)
        {
            var winnerText = player.Name;

            var congratuloryText = "did the best at throwing stuff at others.\r\nLike a filthy conqueror, he got rid of all the others in all the lands.\r\nGood Job!";
            var replayText = "Once more into the breach!";
            var quitText = "I've had it";

            if (!player.IsHuman)
            {
                congratuloryText = "Look at yourself!\r\nEven a silly bot like ours can defeat you!\r\nI'd suggest trying again, but you might just go cry.";
                replayText = "Revenge!";
                quitText = "I suck and I am going to cry.";
            }

            this.winnerText.text = winnerText;
            this.winnerText.color = player.Color;
            this.congratuloryText.text = congratuloryText;
            this.replayButtonText.text = replayText;
            this.quitButtonText.text = quitText;
        }

        public void Show()
        {
            this.contentContainer.SetActive(true);
        }

        public void Hide()
        {
            this.contentContainer.SetActive(false);
        }
    }
}
