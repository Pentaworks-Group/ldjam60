using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.UI;

using FrameVector3 = GameFrame.Core.Math.Vector3;

namespace Assets.Scripts.Scenes.Game
{
    public class GameMainBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, GameObject> catapultTemplateCache = new Dictionary<String, GameObject>();
        private readonly Dictionary<String, GameObject> projectileTemplateCache = new Dictionary<String, GameObject>();
        private readonly Dictionary<Guid, PlayerBehaviour> playerBehaviours = new Dictionary<Guid, PlayerBehaviour>();

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private GameObject templateRootObject;

        [SerializeField]
        private Transform objectContainer;

        private void Awake()
        {

            Base.Core.Game.ExecuteAfterInstantation(Init);
        }

        private void Init()
        {
            LoadTemplates();

            var currentLevel = Base.Core.Game.State.Level;

            this.backgroundImage.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(currentLevel.BackgroundReference);

            InitWorld(Base.Core.Game.State);
        }

        private void OnShotFired(PlayerBehaviour playerBehaviour)
        {
            if (playerBehaviour != default)
            {
                var projectileType = playerBehaviour.Player.Catapult.SelectedProjectileType;

                if (projectileType == default)
                {
                    projectileType = playerBehaviour.Player.Catapult.AvailableProjectileTypes.FirstOrDefault(p => p.IsDefault);

                    if (projectileType == default)
                    {
                        projectileType = playerBehaviour.Player.Catapult.AvailableProjectileTypes[0];
                    }
                }

                if (projectileType != null)
                {
                    var renderer = playerBehaviour.GetComponent<Renderer>();

                    var position = new FrameVector3(playerBehaviour.Player.Position.X, renderer.bounds.extents.y + 0.1f, 0f);

                    var projectile = new Projectile()
                    {
                        Type = projectileType,
                        Position = position,
                        Force = FrameVector3.Zero,
                    };

                    var projectileBehaviour = SpawnProjectile(projectile);

                    var rigidbody = projectileBehaviour.GetComponent<Rigidbody>();

                    var rotation = Quaternion.Euler(0f, 0f, playerBehaviour.Player.Catapult.FiringAngle);
                    var direction = rotation * Vector3.right;

                    var force = direction.normalized * playerBehaviour.Player.Catapult.FiringStrength;

                    rigidbody.AddForce(force, ForceMode.Impulse);

                    Base.Core.Game.State.Projectiles.Add(projectile);

                    playerBehaviour.Player.Credits -= projectileType.Cost;

                    playerBehaviour.DisableInput();
                }
            }
        }

        private void MoveToNextPlayer()
        {
            var gameState = Base.Core.Game.State;

            if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var behaviour))
            {
                behaviour.DisableInput();
            }

            var nextPlayer = default(Player);

            var availablePlayers = gameState.Players.Where(p => !p.IsDestroyed).ToList();

            if (availablePlayers.Count <= 1)
            {
                // Game Over!
            }
            else
            {
                var currentPlayer = gameState.CurrentPlayer;
                var currentIndex = availablePlayers.IndexOf(currentPlayer);
                var startIndex = currentIndex;

                while (nextPlayer == default)
                {
                    currentIndex++;

                    if (startIndex == currentIndex)
                    {
                        // Sum Ting wong!
                    }

                    if (currentIndex >= availablePlayers.Count)
                    {
                        currentIndex = 0;
                    }

                    nextPlayer = availablePlayers[currentIndex];
                }
            }

            gameState.CurrentPlayer = nextPlayer;

            if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var nextPlayerBehaviour))
            {
                nextPlayerBehaviour.EnableInput();
            }
        }

        private void LoadTemplates()
        {
            LoadTemplates<CatapultBehaviour>(templateRootObject, catapultTemplateCache);
            LoadTemplates<ProjectileBehaviour>(templateRootObject, projectileTemplateCache);
        }

        private void LoadTemplates<TBehaviour>(GameObject templateRootObject, Dictionary<String, GameObject> templateCache) where TBehaviour : MonoBehaviour
        {
            foreach (var child in templateRootObject.GetComponentsInChildren<TBehaviour>())
            {
                var childObject = child.gameObject;

                templateCache[childObject.name] = childObject;
            }
        }

        private void InitWorld(GameState gameState)
        {
            SpawnPlayers(gameState.Players);
            SpawnProjectiles(gameState.Projectiles);

            if (gameState.CurrentPlayer == default)
            {
                gameState.CurrentPlayer = gameState.Players[0];

                if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var playerBehaviour))
                {
                    playerBehaviour.EnableInput();
                }
            }
        }

        private void SpawnPlayers(List<Player> players)
        {
            foreach (var player in players)
            {
                SpawnPlayer(player);
            }
        }

        private void SpawnPlayer(Player player)
        {
            var postion = new UnityEngine.Vector3(player.Position.X, player.Position.Y, 0);

            var catapultTemplate = GetTemplate(catapultTemplateCache, player.Catapult.TemplateReference);
            var spawnedCatapult = SpawnObject(catapultTemplate, objectContainer, player.Name, postion);

            var playerBehaviour = SetPlayerBehaviour(player, spawnedCatapult);

            playerBehaviours[player.ID] = playerBehaviour;

            spawnedCatapult.SetActive(true);
        }

        private void SpawnProjectiles(List<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                SpawnProjectile(projectile);
            }
        }

        private ProjectileBehaviour SpawnProjectile(Projectile projectile)
        {
            var startPosition = projectile.Position.ToUnity();

            var template = GetTemplate(projectileTemplateCache, projectile.Type.TemplateReference);
            var spawnedProjectile = SpawnObject(template, objectContainer, projectile.Type.Name, startPosition);

            var rigidbody = spawnedProjectile.GetComponent<Rigidbody>();
            var projectileBehaviour = spawnedProjectile.GetComponent<ProjectileBehaviour>();

            rigidbody.mass = projectile.Type.Mass;

            if (projectile.Rotation.HasValue)
            {
                rigidbody.rotation = projectile.Rotation.ToUnityQuaternion();
            }

            if (projectile.Force.HasValue)
            {
                rigidbody.linearVelocity = projectile.Force.ToUnity();
            }

            spawnedProjectile.SetActive(true);

            return projectileBehaviour;
        }

        private PlayerBehaviour SetPlayerBehaviour(Player player, GameObject catapult)
        {
            PlayerBehaviour playerBehaviour;

            switch (player)
            {
                case HumanPlayer:
                    playerBehaviour = catapult.AddComponent<HumanPlayerBehaviour>();
                    break;

                case BotPlayer:
                    playerBehaviour = catapult.AddComponent<BotPlayerBehaviour>();
                    break;

                default:

                    throw new NotSupportedException($"Unsupported Player type '{player.GetType().FullName}'!");
            }

            playerBehaviour.Init(player);
            playerBehaviour.ShotFired.AddListener(OnShotFired);

            return playerBehaviour;
        }

        private GameObject GetTemplate(Dictionary<String, GameObject> templateCache, String templateName)
        {
            if (templateCache.TryGetValue(templateName, out var template))
            {
                return template;
            }

            return default;
        }

        private GameObject SpawnObject(GameObject template, Transform targetContainer, String name, UnityEngine.Vector3 position)
        {
            var newGameObject = Instantiate(template, position, template.transform.rotation, targetContainer);
            newGameObject.name = name;

            if (newGameObject.TryGetComponent<ObjectsBehaviour>(out var objectBehaviour))
            {
                objectBehaviour.ObjectDestroyed.AddListener(OnObjectDestroyed);
            }

            return newGameObject;
        }

        private void OnObjectDestroyed(GameObject destroyedGameObject)
        {
            if (destroyedGameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour))
            {
                this.playerBehaviours.Remove(playerBehaviour.Player.ID);
            }

            if (destroyedGameObject.TryGetComponent<ProjectileBehaviour>(out var projectileBehaviour))
            {
                MoveToNextPlayer();
            }
        }
    }
}
