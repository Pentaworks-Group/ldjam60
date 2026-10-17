using GameFrame.Core.Definitions.Loaders;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class GameModeLoader : BaseLoader<GameMode>
    {
        private readonly DefinitionCache<LevelDefinition> levelCache;

        public GameModeLoader(DefinitionCache<GameMode> targetCache, DefinitionCache<LevelDefinition> levelCache) : base(targetCache)
        {
            this.levelCache = levelCache;
        }
    }
}
