using System;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class GameMainBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, GameObject> catapultTemplateCache = new Dictionary<string, GameObject>();
        private readonly Dictionary<String, GameObject> projectileTemplateCache = new Dictionary<string, GameObject>();

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
            var postion = new UnityEngine.Vector3(player.Position.X, player.Position.Y,0);

            var template = GetTemplate(catapultTemplateCache, player.Catapult.Definition.Reference);
            var spawnedCatapult = SpawnObject(template, objectContainer, player.Name, postion);

            var playerBehaviour = SetPlayerBehaviour(player, spawnedCatapult);

            spawnedCatapult.SetActive(true);
        }

        private void SpawnProjectiles(List<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                SpawnProjectile(projectile);
            }
        }

        private void SpawnProjectile(Projectile projectile)
        {

        }

        private PlayerBehvaiour SetPlayerBehaviour(Player player, GameObject catapult)
        {
            PlayerBehvaiour playerBehvaiour;

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
