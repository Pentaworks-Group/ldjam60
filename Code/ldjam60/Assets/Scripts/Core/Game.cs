using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        protected override GameState InitializeGameState()
        {
            return new GameState();
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            return new PlayerOptions();
        }

        protected override void RegisterScenes()
        {
            foreach (var scene in Constants.Scenes.GetAll())
            {
                RegisterScene(scene);
            }
        }
    }
}
