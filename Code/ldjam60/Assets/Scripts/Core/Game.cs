using System.Collections;

using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Definitions.Loaders;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        private readonly DefinitionCache<Definitions.GameMode> gameModeCache = new DefinitionCache<Definitions.GameMode>();
        private readonly DefinitionCache<Definitions.LevelDefinition> levelCache = new DefinitionCache<Definitions.LevelDefinition>();
        private readonly DefinitionCache<Definitions.CatapultDefinition> catapultCache = new DefinitionCache<Definitions.CatapultDefinition>();
        private readonly DefinitionCache<Definitions.ProjectileDefinition> projectileCache = new DefinitionCache<Definitions.ProjectileDefinition>();

        protected override GameState InitializeGameState()
        {
            return new GameState();
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.9f,
                AmbienceVolume = 0.1f,
                BackgroundVolume = 0.3f,
            };
        }

        protected override void RegisterScenes()
        {
            RegisterScenes(Constants.Scenes.GetAll());
        }

        protected override IEnumerator LoadDefintions()
        {
            yield return new DefinitionLoader<LevelDefinition>(this.levelCache).LoadDefinitions("Levels.json");
            yield return new DefinitionLoader<ProjectileDefinition>(this.projectileCache).LoadDefinitions("Projectiles.json");
            yield return new SimpleReferenceLoader<CatapultDefinition>(this.catapultCache, this.projectileCache).LoadDefinitions("Catapults.json");
            yield return new SimpleReferenceLoader<GameMode>(this.gameModeCache, this.levelCache).LoadDefinitions("GameModes.json");

            Debug.Log("loaded definitions");
        }
    }
}
