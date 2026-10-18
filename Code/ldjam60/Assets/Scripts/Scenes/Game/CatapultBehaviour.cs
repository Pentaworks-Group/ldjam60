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
    public class CatapultBehaviour : MonoBehaviour
    {
        [SerializeField] TMP_Text nameText;

        private Player player;

        public void Init(Player player)
        {
            this.player = player;

            this.nameText.text = player.Name;
        }
    }
}
