using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;

namespace TagTool.MtnDewIt.Commands.ConvertCache.Tags 
{
    public class globals_masterchief_3p_shield_impact_shield_impact : TagFile
    {
        public globals_masterchief_3p_shield_impact_shield_impact(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<ShieldImpact>($@"globals\masterchief_3p_shield_impact");
            var shit = CacheContext.Deserialize<ShieldImpact>(Stream, tag);
            shit.Version = 4;
            shit.ShieldIntensity = new ShieldImpact.ShieldIntensityBlock
            {
                RecentDamageIntensity = 0.15f,
            };
            shit.ShieldEdge = new ShieldImpact.ShieldEdgeBlock
            {
                DepthFadeRange = 0.25f,
                OuterFadeRadius = 0.5f,
                CenterRadius = 0.75f,
                InnerFadeRadius = 1f,
                EdgeGlowColor = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x34, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00,
                            0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x80, 0xE5, 0xFF,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x1C, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0xED, 0xBE, 0x6B, 0x3F, 0x9E, 0x33,
                            0x2A, 0xC0, 0xE3, 0x43, 0x2F, 0x40, 0x00, 0x00, 0x00, 0x00,
                        },
                    },
                },
                EdgeGlowIntensity = new ShieldImpactFunction
                {
                    InputVariable = CacheContext.StringTable.GetOrAddString($@"shield_intensity"),
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00,
                            0xC0, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x1C, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00,
                            0x40, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                        },
                    },
                },
            };
            shit.Plasma = new ShieldImpact.PlasmaBlock
            {
                PlasmaDepthFadeRange = 0.15f,
                PlasmaNoiseBitmap1 = GetCachedTag<Bitmap>($@"fx\bitmaps\plasma\plasma_clouds_512_a"),
                PlasmaNoiseBitmap2 = GetCachedTag<Bitmap>($@"fx\bitmaps\plasma\plasma_clouds_512_b"),
                TilingScale = 3f,
                ScrollSpeed = 2f,
                EdgeSharpness = 20f,
                CenterSharpness = 60f,
                PlasmaCenterRadius = 0.5f,
                PlasmaInnerFadeRadius = 0.75f,
                PlasmaCenterColor = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x34, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x17, 0x39,
                            0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x34, 0x77, 0xB3, 0xFF,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x1C, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0xC4, 0x40, 0x67, 0x3F, 0xE0, 0xF9,
                            0xC5, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        },
                    },
                },
                PlasmaCenterIntensity = new ShieldImpactFunction
                {
                    InputVariable = CacheContext.StringTable.GetOrAddString($@"shield_intensity"),
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x42, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00,
                        },
                    },
                },
                PlasmaEdgeColor = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x34, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x17, 0x39,
                            0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x34, 0x00,
                            0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xAE, 0x47, 0xA1, 0x3E, 0x12, 0x25, 0x6D, 0xC0, 0x81, 0xCC,
                            0xF7, 0xC0, 0xC5, 0x68, 0xBF, 0x40, 0x00, 0x00, 0x00, 0x00,
                            0x07, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x7F, 0x7F, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x80, 0x3F,
                        },
                    },
                },
                PlasmaEdgeIntensity = new ShieldImpactFunction
                {
                    InputVariable = CacheContext.StringTable.GetOrAddString($@"shield_intensity"),
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x34, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00,
                            0x00, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0x10, 0xC6, 0x62, 0xC0, 0x10, 0xC6,
                            0x22, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                        },
                    },
                },
            };
            shit.ExtrusionOscillation = new ShieldImpact.ExtrusionOscillationBlock
            {
                OscillationBitmap1 = GetCachedTag<Bitmap>($@"test\chris\noise\blur_noise256_a"),
                OscillationBitmap2 = GetCachedTag<Bitmap>($@"test\chris\noise\blur_noise256_b"),
                OscillationTilingScale = 2f,
                OscillationScrollSpeed = 0.15f,
                ExtrusionAmount = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x34, 0x00, 0x00, 0x0A, 0xD7, 0xF3, 0x3B, 0x6F, 0x12,
                            0x03, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0x30, 0xFB, 0xBB, 0xBE, 0x98, 0xFD,
                            0xDD, 0x3F, 0x66, 0x7F, 0x17, 0xC0, 0x00, 0x00, 0x80, 0x3F,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00,
                        },
                    },
                },
                OscillationAmplitude = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00,
                        },
                    },
                },
            };
            shit.HitResponse = new ShieldImpact.HitResponseBlock
            {
                HitTime = 2.857143f,
                HitColor = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x08, 0x35, 0x02, 0x00, 0x39, 0x8A, 0xFF, 0xFF, 0xB4, 0x2A,
                            0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x30, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0xFF, 0xFF, 0x7F, 0x7F, 0xAB, 0x62, 0xA7, 0xBF, 0x5E, 0x16,
                            0x08, 0x40, 0x73, 0xAF, 0x39, 0x3E, 0x00, 0x00, 0x00, 0x00,
                            0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0xCD, 0xFF, 0xFF,
                            0x7F, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                        },
                    },
                },
                HitIntensity = new ShieldImpactFunction
                {
                    Function = new TagTool.Tags.TagFunction
                    {
                        Data = new byte[]
                        {
                            0x01, 0x34, 0x00, 0x00, 0xCD, 0xCC, 0xCC, 0x3D, 0xCD, 0xCC,
                            0x4C, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x6F, 0x12, 0x83, 0x3A, 0x6F, 0x12, 0x03, 0x3B, 0x2C, 0x00,
                            0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD,
                            0x5C, 0x8F, 0x02, 0x3F, 0x60, 0x17, 0x07, 0xC0, 0x19, 0xED,
                            0xBF, 0x40, 0x0F, 0x0F, 0x8F, 0xC0, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x7F, 0x7F, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        },
                    },
                },
            };
            shit.EdgeScales = new RealQuaternion(1f, -4f, 0f, -4f);
            shit.EdgeOffsets = new RealQuaternion(-0.8750001f, -2f, -2f, -2f);
            shit.PlasmaScales = new RealQuaternion(3.2f, 2f, -55f, 75f);
            shit.DepthFadeParameters = new RealQuaternion(20f, 0f, 20f, 20f);
            CacheContext.Serialize(Stream, tag, shit);
        }
    }
}
