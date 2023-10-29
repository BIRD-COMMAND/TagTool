using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class objects_multi_spawning_respawn_zone_scenery : TagFile
    {
        public objects_multi_spawning_respawn_zone_scenery(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
        (
            cache,
            cacheContext,
            stream
        )
        {
            Cache = cache;
            CacheContext = cacheContext;
            Stream = stream;
            TagData();
        }

        public override void TagData()
        {
            var tag = GetCachedTag<Scenery>($@"objects\multi\spawning\respawn_zone");
            var scen = CacheContext.Deserialize<Scenery>(Stream, tag);
            scen.MultiplayerObject[0].EngineFlags = GameEngineSubTypeFlags.All;
            CacheContext.Serialize(Stream, tag, scen);
        }
    }
}