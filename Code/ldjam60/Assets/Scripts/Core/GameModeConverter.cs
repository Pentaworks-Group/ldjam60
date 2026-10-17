using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core
{
    public class GameModeConverter
    {
        private readonly GameMode gameMode;

        public GameModeConverter(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        public GameState Convert()
        {
            var gameState = new GameState()
            {
                CurrentScene = "Game",
                TurnCount = 1,
                Level = ConvertLevel(gameMode.Level),
                Players = new List<Player>(),
            };

            gameState.Players.AddRange(LoadPlayers());

            return gameState;
        }

        private Level ConvertLevel(LevelDefinition levelDefinition)
        {
            var level = new Level()
            {
                BackgroundReference = levelDefinition.BackgroundReferences.GetRandomEntry(),
            };

            return level;
        }

        private List<Player> LoadPlayers()
        {
            var players = new List<Player>();

            var playerCatapult = gameMode.AvailableCatapults.GetRandomEntry();

            var humanPlayer = new HumanPlayer()
            {
                Name = "Player 1",
                Credits = 100000m,
                Catapult = ConvertCatapult(playerCatapult),
                Health = 100,
                Position = new GameFrame.Core.Math.Vector2(-3, 0)
            };
            
            players.Add(humanPlayer);

            var humanPlayer2 = new HumanPlayer()
            {
                Name = "Player 2",
                Credits = 100000m,
                Catapult = ConvertCatapult(playerCatapult),
                Health = 100,
                Position = new GameFrame.Core.Math.Vector2(3, 0)
            };

            players.Add(humanPlayer2);

            return players;
        }

        private Catapult ConvertCatapult(CatapultDefinition definition)
        {
            var catapult = new Catapult()
            {
                Definition = definition,
                FiringAngle = definition.PossibleFiringAngle.GetRandom(),
                FiringStrength = definition.PossibleFiringStrength.GetRandom()
            };

            return catapult;
        }
    }
}
