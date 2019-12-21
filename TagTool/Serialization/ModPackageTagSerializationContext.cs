﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : TagSerializationContext
    {
        private ModPackage Package;

        public ModPackageTagSerializationContext(Stream stream, HaloOnlineCacheContext context, ModPackage package, CachedTagInstance tag) : base(stream, context, tag)
        {
            Package = package;
        }

        public override CachedTagInstance GetTagByIndex(int index)
        {
            if (index < 0)
                return null;

            return Package.TagCaches[0].Index[index];
        }

        public override CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            foreach(var tag in Context.TagCache.Index)
            {
                if (tag.Name == name && group == tag.Group)
                    return tag;
            }
            return null;
        }
    }
}
