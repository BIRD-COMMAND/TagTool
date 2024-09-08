﻿using System.IO;
using TagTool.Cache.HaloOnline;
using TagTool.Cache;
using TagTool.MtnDewIt.JSON.Handlers;
using TagTool.IO;
using TagTool.Tags;
using TagTool.MtnDewIt.BlamFiles;

namespace TagTool.MtnDewIt.JSON.Parsers
{
    public class BlfObjectParser 
    {
        private GameCache Cache;
        private GameCacheHaloOnline CacheContext;
        private Stream CacheStream;
        private BlfObjectHandler Handler;

        public BlfObjectParser(GameCache cache, GameCacheHaloOnline cacheContext, Stream cacheStream)
        {
            Cache = cache;
            CacheContext = cacheContext;
            CacheStream = cacheStream;
            Handler = new BlfObjectHandler(Cache, CacheContext);
        }

        public void ParseFile(string filePath)
        {
            var jsonData = File.ReadAllText($@"{filePath}.json");
            var blfObject = Handler.Deserialize(jsonData);

            var blfFile = new FileInfo($@"{Cache.Directory.FullName}\{blfObject.FileName}.{blfObject.FileType}");

            using (var stream = blfFile.Create())
            using (var writer = new EndianWriter(stream)) 
            {
                if (blfObject.BlfData.Version == CacheVersion.HaloOnlineED)
                {
                    if (blfObject.BlfData != null && blfObject.BlfData.MapVariant != null && blfObject.BlfData.MapVariantTagNames != null)
                    {
                        UpdateQuotaIndexes(blfObject.BlfData.MapVariantTagNames.Names, blfObject.BlfData.MapVariant.MapVariant.Quotas);
                    }
                }

                blfObject.BlfData.WriteData(writer);
            }
        }

        public void UpdateQuotaIndexes(BlfTagName[] tagNames, VariantDataObjectQuota[] quotaList)
        {
            for (int i = 0; i < tagNames.Length; i++)
            {
                var tagName = tagNames[i];

                if (tagName.Name != null && tagName.Name != "")
                {
                    var tag = CacheContext.TagCache.GetTag(tagName.Name);

                    quotaList[i].ObjectDefinitionIndex = tag.Index;
                }
            }
        }
    }
}