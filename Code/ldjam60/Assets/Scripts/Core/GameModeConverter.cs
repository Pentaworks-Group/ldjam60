using Assets.Scripts.Constants;
using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;
using Assets.Scripts.Core.Models.Players.Bots;
using GameFrame.Core.Extensions;
using System;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.Core
{
    public class GameModeConverter
    {

        public static List<Type> availableBots = new List<Type>()
        {
            typeof(DefaultBot),
            typeof(RandomBot)
        };

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

            var humanPlayer = new HumanPlayer()
            {
                ID = Guid.NewGuid(),
                Name = "Player 1",
                Credits = 100000m,
                Catapult = ConvertCatapult(playerCatapult),
                Health = 100,
                Position = new GameFrame.Core.Math.Vector2(-3, 0)
            };

            players.Add(humanPlayer);

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

            for (int botNumber = 0; botNumber < 2; botNumber++)
            {
                var bot = GenerateBot(players);

                players.Add(bot);
            }

            //var defaultBot = new DefaultBot()
            //{
            //    ID = Guid.NewGuid(),
            //    Name = "Karl Heinz",
            //    Credits = 100000m,
            //    Catapult = ConvertCatapult(playerCatapult),
            //    Health = 100,
            //    Position = new GameFrame.Core.Math.Vector2(0, 1.5f)
            //};

            //players.Add(defaultBot);

            //var defaultBot2 = new RandomBot()
            //{
            //    ID = Guid.NewGuid(),
            //    Name = "Klaus Dieter",
            //    Credits = 100000m,
            //    Catapult = ConvertCatapult(playerCatapult),
            //    Health = 100,
            //    Position = new GameFrame.Core.Math.Vector2(-1.5f, 0)
            //};

            //players.Add(defaultBot2);

            return players;
        }

        private BotPlayer GenerateBot(List<Player> players)
        {
            var botCatapult = gameMode.AvailableCatapults.GetRandomEntry();

            var botType = availableBots.GetRandomEntry();

            var botPlayer = (BotPlayer)Activator.CreateInstance(botType);

            botPlayer.ID = Guid.NewGuid();
            botPlayer.Name = BotNames.GetRandomEntry();
            botPlayer.Credits = 100000m;
            botPlayer.Catapult = ConvertCatapult(botCatapult);
            botPlayer.Health = 100;

            var position = GetBotSpawnPosition(players);

            botPlayer.Position = new GameFrame.Core.Math.Vector2(position, 0.5f);

            return botPlayer;
        }

        private int GetBotSpawnPosition(List<Player> players)
        {
            int position = 0;
            int attemptsLeft = 100;
            while (attemptsLeft >= 0)
            {
                position = UnityEngine.Random.Range(-6, 6);
                if (CheckIfPositionAvailable(position, players))
                {
                    break;
                }
                attemptsLeft--;
            }
            return position;
        }

        private bool CheckIfPositionAvailable(int position, List<Player> players)
        {
            foreach (Player player in players)
            {
                if (Math.Abs(player.Position.X - position) < 3)
                {
                    return false;
                }
            }

            return true;
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
