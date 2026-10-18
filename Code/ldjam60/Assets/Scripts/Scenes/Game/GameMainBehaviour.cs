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
    [RequireComponent(typeof(GameOverBehaviour))]
    public class GameMainBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, GameObject> catapultTemplateCache = new Dictionary<String, GameObject>();
        private readonly Dictionary<String, GameObject> projectileTemplateCache = new Dictionary<String, GameObject>();
        private readonly Dictionary<Guid, PlayerBehaviour> playerBehaviours = new Dictionary<Guid, PlayerBehaviour>();

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Transform objectContainer;
        [SerializeField] private GameObject templateRootObject;
        [SerializeField] private GameObject cube;

        [SerializeField] private DialScriptableObjectScript angleDial;
        [SerializeField] private DialScriptableObjectScript strengthDial;

        private GameOverBehaviour gameOverBehaviour;

        public void OnRetryClicked()
        {
            var gameState = Base.Core.Game.State;

            Base.Core.Game.Start(gameState.GameModeReference);
        }

        public void OnQuitClicked()
        {
            Base.Core.Game.Stop();
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(Init);
        }

        private void Init()
        {
            this.gameOverBehaviour = GetComponent<GameOverBehaviour>();

            LoadTemplates();

            var currentLevel = Base.Core.Game.State.Level;

            this.backgroundImage.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(currentLevel.BackgroundReference);

            InitWorld(Base.Core.Game.State);
        }

        private void AngleDial_OnValueChanged(Single value)
        {
            var currentPlayer = Base.Core.Game.State.CurrentPlayer;

            if (currentPlayer != default)
            {
                currentPlayer.Catapult.FiringAngle = value;
            }
        }

        private void StrengthDial_OnValueChanged(Single value)
        {
            var currentPlayer = Base.Core.Game.State.CurrentPlayer;

            if (currentPlayer != default)
            {
                currentPlayer.Catapult.FiringStrength = value;
            }
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

                    DisableInput();
                    playerBehaviour.DisableInput();
                }
            }
        }

        private void MoveToNextPlayer()
        {
            var gameState = Base.Core.Game.State;

            DisableInput();

            if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var behaviour))
            {
                behaviour.DisableInput();
            }

            var nextPlayer = default(Player);

            var availablePlayers = gameState.Players.Where(p => !p.IsDestroyed).ToList();

            if (availablePlayers.Count <= 1)
            {
                ShowGameOver(Base.Core.Game.State.CurrentPlayer);
                gameState.CurrentPlayer = default;

                return;
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

            if (nextPlayer.IsHuman)
            {
                EnableInput();

                if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var nextPlayerBehaviour))
                {
                    nextPlayerBehaviour.EnableInput();
                }
            }
        }

        private void EnableInput()
        {
            this.angleDial.OnValueChanged += AngleDial_OnValueChanged;
            this.strengthDial.OnValueChanged += StrengthDial_OnValueChanged;
        }

        private void DisableInput()
        {
            this.angleDial.OnValueChanged -= AngleDial_OnValueChanged;
            this.strengthDial.OnValueChanged -= StrengthDial_OnValueChanged;
        }

        private void ShowGameOver(Player winner)
        {
            this.gameOverBehaviour.SetWinner(winner);
            this.gameOverBehaviour.Show();
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

                if (gameState.CurrentPlayer.IsHuman)
                {
                    EnableInput();

                    if (playerBehaviours.TryGetValue(gameState.CurrentPlayer.ID, out var nextPlayerBehaviour))
                    {
                        nextPlayerBehaviour.EnableInput();
                    }
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
            var height = GetWorldHeight(player.Position.X);

            var position = new UnityEngine.Vector3(player.Position.X, height + 0.2f, 0);

            var catapultTemplate = GetTemplate(catapultTemplateCache, player.Catapult.TemplateReference);
            var spawnedCatapult = SpawnObject(catapultTemplate, objectContainer, player.Name, position);

            var objectBehaviour = spawnedCatapult.GetComponent<ObjectsBehaviour>();

            objectBehaviour.Init(player.Health, false, 1);

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
            var objectBehaviour = spawnedProjectile.GetComponent<ObjectsBehaviour>();

            projectileBehaviour.Init(projectile);
            projectileBehaviour.MoveFinished += ProjectileBehaviour_MoveFinished;
            objectBehaviour.Init(1f, false, projectile.Type.Damage);

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

        private void ProjectileBehaviour_MoveFinished(ProjectileBehaviour behaviour)
        {
            behaviour.MoveFinished -= ProjectileBehaviour_MoveFinished;

            MoveToNextPlayer();
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
                playerBehaviour.Player.IsDestroyed = true;
                this.playerBehaviours.Remove(playerBehaviour.Player.ID);

                if (this.playerBehaviours.Count == 1)
                {
                    ShowGameOver(this.playerBehaviours.Values.First().Player);
                }
            }

            if (destroyedGameObject.TryGetComponent<ProjectileBehaviour>(out var projectileBehaviour))
            {
                Base.Core.Game.State.Projectiles.Remove(projectileBehaviour.Projectile);
            }
        }

        private Single GetWorldHeight(Single positionX)
        {
            // THIS IS A HACK! REMOVE IT!
            var renderer = this.cube.GetComponent<Renderer>();

            return renderer.bounds.extents.y;
        }
    }
}
