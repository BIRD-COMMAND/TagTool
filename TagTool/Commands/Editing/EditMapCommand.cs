﻿using System.Collections.Generic;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;

namespace TagTool.Commands.Editing 
{
    class EditMapCommand : Command 
    {
        private CommandContextStack ContextStack { get; }
        private GameCacheHaloOnlineBase CacheContext { get; }
        private GameCache Cache { get; }

        public EditMapCommand(CommandContextStack contextStack, GameCacheHaloOnlineBase cacheContext, GameCache cache) : base(
            false,

            "EditMap",
            "Edit map-specific data",

            "EditTag <Map File>",

            "If the map file contains data which is supported by this program,\n" +
            "this command will make special map-specific commands available\n" +
            "which can be used to edit or view the data in the map.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
            Cache = cache;
        }

        public override object Execute(List<string> args) 
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);
            if (!TryGetMapFile(args[0], out var mapFile))
                return new TagToolError(CommandError.FileNotFound, $"\"{args[0]}\"");

            ContextStack.Push(EditMapContextFactory.Create(ContextStack, Cache, mapFile));

            return true;
        }

        public bool TryGetMapFile(string mapName, out MapFile result) 
        {
            try
            {
                var file = new FileInfo($@"{CacheContext.Directory.FullName}\{mapName}");

                var mapFileData = new MapFile();

                using (var stream = file.OpenRead())
                {
                    var reader = new EndianReader(stream);

                    mapFileData.Read(reader);
                }

                result = mapFileData;

                return true;
            }
            catch 
            {
                result = null;

                return false;
            }
        }
    }
}