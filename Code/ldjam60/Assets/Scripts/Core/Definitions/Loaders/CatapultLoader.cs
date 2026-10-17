using GameFrame.Core.Definitions.Loaders;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class CatapultLoader : BaseLoader<CatapultDefinition>
    {
        private readonly DefinitionCache<ProjectileDefinition> projectileCache;

        public CatapultLoader(DefinitionCache<CatapultDefinition> targetCache, DefinitionCache<ProjectileDefinition> projectileCache) : base(targetCache)
        {
            this.projectileCache = projectileCache;
        }
    }
}
