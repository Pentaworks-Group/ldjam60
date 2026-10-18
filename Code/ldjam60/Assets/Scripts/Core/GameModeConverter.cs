using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;
using Assets.Scripts.Core.Models.Players.Bots;

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
                GameModeReference = gameMode.Reference,
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

            //var humanPlayer = new HumanPlayer()
            //{
            //    ID = Guid.NewGuid(),
            //    Name = "Player 1",
            //    Credits = 100000m,
            //    Catapult = ConvertCatapult(playerCatapult),
            //    Health = 100,
            //    Position = new GameFrame.Core.Math.Vector2(-3, 0)
            //};

            //players.Add(humanPlayer);

            //var humanPlayer2 = new HumanPlayer()
            //{
            //    ID = Guid.NewGuid(),
            //    Name = "Player 2",
            //    Credits = 100000m,
            //    Catapult = ConvertCatapult(playerCatapult),
            //    Health = 100,
            //    Position = new GameFrame.Core.Math.Vector2(3, 0)
            //};

            //players.Add(humanPlayer2);

            var defaultBot = new DefaultBot()
            {
                ID = Guid.NewGuid(),
                Name = "Karl Heinz",
                Credits = 100000m,
                Catapult = ConvertCatapult(playerCatapult),
                Health = 100,
                Position = new GameFrame.Core.Math.Vector2(0, 1.5f)
            };

            players.Add(defaultBot);

            var defaultBot2 = new RandomBot()
            {
                ID = Guid.NewGuid(),
                Name = "Klaus Dieter",
                Credits = 100000m,
                Catapult = ConvertCatapult(playerCatapult),
                Health = 100,
                Position = new GameFrame.Core.Math.Vector2(-1.5f, 0)
            };

            players.Add(defaultBot2);

            return players;
        }

        private Catapult ConvertCatapult(CatapultDefinition definition)
        {
            var catapult = new Catapult()
            {
                FiringArc = definition.FiringArc,
                PossibleFiringPower = definition.PossibleFiringPower,
                TemplateReference = definition.TemplateReference,
                FiringAngle = definition.FiringArc.GetRandom(),
                FiringPower = definition.PossibleFiringPower.GetRandom()
            };

            ConvertProjectileTypes(definition.AvailableProjectileTypes, catapult.AvailableProjectileTypes);

            return catapult;
        }

        private void ConvertProjectileTypes(List<ProjectileTypeDefinition> availableProjectileTypeDefinitions, List<ProjectileType> avalableProjectileTypes)
        {
            if (availableProjectileTypeDefinitions?.Count > 0)
            {
                foreach (var definition in availableProjectileTypeDefinitions)
                {
                    var pojectileType = new ProjectileType()
                    {
                        Name = definition.Name,
                        IsDefault = definition.IsDefault.GetValueOrDefault(),
                        Cost = definition.Cost.GetValueOrDefault(0),
                        Damage = definition.Damage.GetValueOrDefault(),
                        TemplateReference = definition.TemplateReference,
                        Mass = definition.Mass.GetRandom()
                    };

                    avalableProjectileTypes.Add(pojectileType);
                }
            }
        }
    }
}
