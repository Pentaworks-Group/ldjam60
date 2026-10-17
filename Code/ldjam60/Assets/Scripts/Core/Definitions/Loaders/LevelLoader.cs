using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameFrame.Core.Definitions.Loaders;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class LevelLoader : BaseLoader<LevelDefinition>
    {
        private readonly DefinitionCache<CatapultDefinition> catapultCache;

        public LevelLoader(DefinitionCache<LevelDefinition> targetCache, DefinitionCache<CatapultDefinition> catapultCache) : base(targetCache)
        {
            this.catapultCache = catapultCache;
        }
    }
}
