using System;

using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public Level Level { get; set; }
        public Player CurrentPlayer { get; set; }
        public Int32 TurnCount { get; set; }
    }
}
