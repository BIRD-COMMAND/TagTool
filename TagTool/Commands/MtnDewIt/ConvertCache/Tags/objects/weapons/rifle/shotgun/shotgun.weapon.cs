using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using System.Collections.Generic;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class objects_weapons_rifle_shotgun_shotgun_weapon : TagFile
    {
        public objects_weapons_rifle_shotgun_shotgun_weapon(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<Weapon>($@"objects\weapons\rifle\shotgun\shotgun");
            var weap = CacheContext.Deserialize<Weapon>(Stream, tag);
            weap.FirstPerson = new List<Weapon.FirstPersonBlock> 
            {
                new Weapon.FirstPersonBlock() 
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\shotgun\fp_shotgun\fp_shotgun"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\masterchief\fp\weapons\rifle\fp_shotgun\fp_shotgun"),
                },
                new Weapon.FirstPersonBlock()
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\shotgun\fp_shotgun\fp_shotgun"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\dervish\fp\weapons\rifle\fp_shotgun\fp_shotgun"),
                },
            };
            CacheContext.Serialize(Stream, tag, weap);
        }
    }
}