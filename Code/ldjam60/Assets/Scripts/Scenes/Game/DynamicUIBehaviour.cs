using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core.Models;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class DynamicUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text currentPlayerText;

        [SerializeField]
        private TMP_Text availableCreditsText;

        private void Update()
        {
            var gameState = Base.Core.Game.State;

            if (gameState != default)
            {
                UpdatePlayer(gameState.CurrentPlayer);
            }
        }

        private void UpdatePlayer(Player player)
        {
            var credits = 0m;
            var name = "";
            var color = Color.white;

            if (player != default)
            {
                credits = player.Credits;
                name = player.Name;
                color = player.Color;
            }

            availableCreditsText.text = String.Format("$: {0:#}", credits);
            currentPlayerText.text = name;
            currentPlayerText.color = color;
        }
    }
}
