using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public GameMode GameMode { get; set; }
        public Level Level { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Projectile> Projectiles { get; set; } = new List<Projectile>();
        public Player CurrentPlayer { get; set; }
        public Int32 TurnCount { get; set; }
    }
}
