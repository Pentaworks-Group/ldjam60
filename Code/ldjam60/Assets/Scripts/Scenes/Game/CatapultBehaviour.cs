using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core.Models;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class CatapultBehaviour : MonoBehaviour
    {
        private Catapult catapult;

        public void Init(Catapult catapult)
        {
            this.catapult = catapult;
        }
    }
}
