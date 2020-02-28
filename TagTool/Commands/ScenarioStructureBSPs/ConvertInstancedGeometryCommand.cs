﻿using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Scenarios
{
    class ConvertInstancedGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private Scenario Scnr { get; }

        public ConvertInstancedGeometryCommand(GameCache cacheContext, Scenario scnr) :
            base(false,

                "ConvertInstancedGeometry",
                "Convert Instanced Geometry to forge objects",

                "ConvertInstancedGeometry",

                "Convert Instanced Geometry to forge objects")
        {
            CacheContext = cacheContext;
            Scnr = scnr;
        }

        public override object Execute(List<string> args)
        {
            using (var stream = CacheContext.OpenCacheReadWrite())
            {
                for (var sbspindex = 0; sbspindex < Scnr.StructureBsps.Count; sbspindex++)
                {
                    var Sbsp = (ScenarioStructureBsp)CacheContext.Deserialize(stream, Scnr.StructureBsps[sbspindex].StructureBsp);
                    var bspresource = CacheContext.ResourceCache.GetStructureBspTagResources(Sbsp.CollisionBspResource);
                    var sLdT = (ScenarioLightmap)CacheContext.Deserialize(stream, Scnr.Lightmap);
                    var Lbsp = (ScenarioLightmapBspData)CacheContext.Deserialize(stream, sLdT.LightmapDataReferences[sbspindex]);

                    //set resource definition
                    var resourceDefinition = CacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);
                    Lbsp.Geometry.SetResourceBuffers(resourceDefinition);

                    var meshlist = new List<short>();
                    for (int InstancedGeometryIndex = 0; InstancedGeometryIndex < bspresource.InstancedGeometry.Count; InstancedGeometryIndex++)
                    {
                        var InstancedGeometryBlock = bspresource.InstancedGeometry[InstancedGeometryIndex];

                        short currentmeshindex = (short)(InstancedGeometryBlock.MeshIndex);
                        short currentcompressionindex = (short)(InstancedGeometryBlock.CompressionIndex);

                        if (currentmeshindex < 0 || meshlist.Contains(currentmeshindex) || currentmeshindex > Lbsp.Geometry.Meshes.Count)
                            continue;

                        meshlist.Add(currentmeshindex);

                        //strip digits from the end of the instancedgeometry name
                        //string instancedgeoname = CacheContext.StringTable.GetString(InstancedGeometryBlock.Name);
                        //var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        //var instancedgeoname = tempname.TrimEnd(digits);

                        string NewName = $"objects\\reforge\\instanced_geometry\\{currentmeshindex}";
                        
                        //if the tag we are trying to create already exists, continue
                        if (CacheContext.TryGetTag<Crate>(NewName, out var unused))
                            continue;

                        //duplicate traffic cone bloc
                        CachedTag originalbloctag = CacheContext.GetTag<Crate>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newbloc = CacheContext.TagCache.AllocateTag(originalbloctag.Group, NewName);
                        newbloc.Name = NewName;
                        var originalbloc = CacheContext.Deserialize(stream, originalbloctag);
                        CacheContext.Serialize(stream, newbloc, originalbloc);

                        //duplicate traffic cone hlmt
                        CachedTag originalhlmttag = CacheContext.GetTag<Model>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newhlmt = CacheContext.TagCache.AllocateTag(originalhlmttag.Group, NewName);
                        newhlmt.Name = NewName;
                        var originalhlmt = CacheContext.Deserialize(stream, originalhlmttag);
                        CacheContext.Serialize(stream, newhlmt, originalhlmt);
                        
                        //duplicate traffic cone render model
                        CachedTag originalmodetag = CacheContext.GetTag<RenderModel>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newmode = CacheContext.TagCache.AllocateTag(originalmodetag.Group, NewName);
                        newmode.Name = NewName;
                        var originalmode = CacheContext.Deserialize(stream, originalmodetag);
                        CacheContext.Serialize(stream, newmode, originalmode);

                        //duplicate traffic cone collision model
                        CachedTag originalcolltag = CacheContext.GetTag<CollisionModel>(@"objects\gear\human\industrial\street_cone\street_cone");
                        var newcoll = CacheContext.TagCache.AllocateTag(originalcolltag.Group, NewName);
                        newcoll.Name = NewName;
                        var originalcoll = CacheContext.Deserialize(stream, originalcolltag);
                        CacheContext.Serialize(stream, newcoll, originalcoll);

                        //copy block elements and resources from sbsp for new mode
                        RenderModel editedmode = (RenderModel)CacheContext.Deserialize(stream, originalmodetag);

                        //
                        // warning: this relies on GetSingleMeshResourceDefinition updating the vertex/index buffer indices in the Lbsp, not safe
                        //

                        var newResourceDefinition = GetSingleMeshResourceDefinition(Lbsp.Geometry, currentmeshindex);
                        editedmode.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newResourceDefinition);
                        
                        //copy meshes tagblock
                        editedmode.Geometry.Meshes = new List<Mesh>
                            {
                                Lbsp.Geometry.Meshes[currentmeshindex]
                            };

                        //copy compression tagblock
                        editedmode.Geometry.Compression = new List<RenderGeometryCompression>
                            {
                                Lbsp.Geometry.Compression[currentcompressionindex]
                            };

                        //copy over materials block, and reindex mesh part materials
                        var newmaterials = new List<RenderMaterial>();
                        for (var i = 0; i < editedmode.Geometry.Meshes[0].Parts.Count; i++)
                        {
                            newmaterials.Add(Sbsp.Materials[editedmode.Geometry.Meshes[0].Parts[i].MaterialIndex]);
                            editedmode.Geometry.Meshes[0].Parts[i].MaterialIndex = (short)i;
                        }
                        editedmode.Materials = newmaterials;                     
                        CacheContext.Serialize(stream, originalmodetag, editedmode);

                        //fixup coll data
                        var tmpcoll = (CollisionModel)CacheContext.Deserialize(stream, newcoll);
                        //copy collision geometry
                        tmpcoll.Regions[0].Permutations[0].Bsps[0].Geometry = InstancedGeometryBlock.CollisionInfo;
                        //copy over mopps if they exist
                        if (InstancedGeometryBlock.CollisionMoppCodes.Count > 0)
                        {
                            tmpcoll.Regions[0].Permutations[0].BspMoppCodes = new List<Havok.TagHkpMoppCode>();
                            tmpcoll.Regions[0].Permutations[0].BspMoppCodes.Add(InstancedGeometryBlock.CollisionMoppCodes[0]);
                        }
                        CacheContext.Serialize(stream, newcoll, tmpcoll);

                        //fixup hlmt references
                        var tmphlmt = (Model)CacheContext.Deserialize(stream, newhlmt);
                        tmphlmt.RenderModel = newmode;
                        tmphlmt.CollisionModel = newcoll;
                        tmphlmt.PhysicsModel = null;
                        tmphlmt.ReduceToL1SuperLow = 300.0f;
                        tmphlmt.ReduceToL2Low = 280.0f;
                        tmphlmt.ReduceToL4High = 18.0f;
                        CacheContext.Serialize(stream, newhlmt, tmphlmt);

                        //fixup bloc references
                        var tmpbloc = (Crate)CacheContext.Deserialize(stream, newbloc);
                        tmpbloc.Model = newhlmt;
                        CacheContext.Serialize(stream, newbloc, tmpbloc);

                        //add new object to forge globals
                        CachedTag forgeglobal = CacheContext.GetTag<ForgeGlobalsDefinition>(@"multiplayer\forge_globals");
                        var tmpforg = (ForgeGlobalsDefinition)CacheContext.Deserialize(stream, forgeglobal);
                        tmpforg.Palette.Add(new ForgeGlobalsDefinition.PaletteItem
                        {
                            Name = NewName,
                            Type = ForgeGlobalsDefinition.PaletteItemType.Structure,
                            CategoryIndex = 64,
                            DescriptionIndex = -1,
                            MaxAllowed = 0,
                            Object = newbloc
                        });
                        CacheContext.Serialize(stream, forgeglobal, tmpforg);                        
                    }
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private RenderGeometryApiResourceDefinition GetSingleMeshResourceDefinition(RenderGeometry renderGeometry, int meshindex)
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, InteropLocations should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            var mesh = renderGeometry.Meshes[meshindex];

            for (int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
            {
                var vertexBuffer = mesh.ResourceVertexBuffers[i];
                if (vertexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = vertexBuffer;
                    result.VertexBuffers.Add(d3dPointer);
                    mesh.VertexBufferIndices[i] = (short)(result.VertexBuffers.Elements.Count - 1);
                }
                else
                    mesh.VertexBufferIndices[i] = -1;
            }

            for (int i = 0; i < mesh.ResourceIndexBuffers.Length; i++)
            {
                var indexBuffer = mesh.ResourceIndexBuffers[i];
                if (indexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<IndexBufferDefinition>();
                    d3dPointer.Definition = indexBuffer;
                    result.IndexBuffers.Add(d3dPointer);
                    mesh.IndexBufferIndices[i] = (short)(result.IndexBuffers.Elements.Count - 1);
                }
                else
                    mesh.IndexBufferIndices[i] = -1;
            }

            // if the mesh is unindexed the index in the index buffer should be 0, but the buffer is empty. Copying what h3\ho does.
            if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
            {
                mesh.IndexBufferIndices[0] = 0;
                mesh.IndexBufferIndices[1] = 0;
            }

            return result;
        }
    }
}