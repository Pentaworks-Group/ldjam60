using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Constants;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using FrameVector3 = GameFrame.Core.Math.Vector3;
using FrameVector4 = GameFrame.Core.Math.Vector4;

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

        private InputAction fireAction;

        private void Awake()
        {
            fireAction = InputSystem.actions.FindAction(InputActions.Fire);

            Base.Core.Game.ExecuteAfterInstantation(Init);
        }

        private void Init()
        {
            LoadTemplates();

            var currentLevel = Base.Core.Game.State.Level;

            this.backgroundImage.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(currentLevel.BackgroundReference);

            InitWorld(Base.Core.Game.State);

            fireAction.performed += FireAction_performed;

            fireAction.Enable();
        }

        private void FireAction_performed(InputAction.CallbackContext obj)
        {
            var currentPlayer = Base.Core.Game.State.CurrentPlayer;

            if (currentPlayer != default)
            {
                var projectileType = currentPlayer.Catapult.SelectedProjectileType;

                if (projectileType == default)
                {
                    projectileType = currentPlayer.Catapult.AvailableProjectileTypes.FirstOrDefault(p => p.IsDefault);

                    if (projectileType == default)
                    {
                        projectileType = currentPlayer.Catapult.AvailableProjectileTypes[0];
                    }
                }

                if ((projectileType != null) && (playerBehaviours.TryGetValue(currentPlayer.ID, out var playerBehaviour)))
                {
                    var renderer = playerBehaviour.GetComponent<Renderer>();

                    var position = new FrameVector3(currentPlayer.Position.X, renderer.bounds.extents.y, 0);
                    
                    var projectile = new Projectile()
                    {
                        Type = projectileType,
                        Position = position ,
                        Force = FrameVector3.Zero, 
                    };

                    var projectileBehaviour = SpawnProjectile(projectile);

                    var rigidbody = projectileBehaviour.GetComponent<Rigidbody>();

                    var rotation = Quaternion.Euler(0f, 0f, 120);
                    var direction = rotation * Vector3.right;

                    var forcedForce = direction.normalized * currentPlayer.Catapult.FiringStrength;

                    rigidbody.AddForce(forcedForce, ForceMode.Impulse);

                    Base.Core.Game.State.Projectiles.Add(projectile);

                    currentPlayer.Credits -= projectileType.Cost;
                }
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

            var template = GetTemplate(catapultTemplateCache, player.Catapult.TemplateReference);
            var spawnedCatapult = SpawnObject(template, objectContainer, player.Name, postion);

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
            PlayerBehaviour playerBehvaiour;

            switch (player)
            {
                case HumanPlayer:
                    playerBehvaiour = catapult.AddComponent<HumanPlayerBehaviour>();
                    break;

                case BotPlayer:
                    playerBehvaiour = catapult.AddComponent<BotPlayerBehaviour>();
                    break;

                default:

                    throw new NotSupportedException($"Unsupported Player type '{player.GetType().FullName}'!");
            }

            return playerBehvaiour;
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

            return newGameObject;
        }
    }
}
