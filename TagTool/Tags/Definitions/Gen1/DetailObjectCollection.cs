using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "detail_object_collection", Tag = "dobc", Size = 0x80)]
    public class DetailObjectCollection : TagStructure
    {
        public CollectionTypeValue CollectionType;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public float GlobalZOffset; // applied to all detail objects of in this collection so they don't float above the ground
        [TagField(Length = 0x2C)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SpritePlate;
        public List<DetailObjectTypeBlock> Types;
        [TagField(Length = 0x30)]
        public byte[] Padding2;
        
        public enum CollectionTypeValue : short
        {
            ScreenFacing,
            ViewerFacing
        }
        
        [TagStructure(Size = 0x60)]
        public class DetailObjectTypeBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte SequenceIndex; // [0,15]
            public TypeFlagsValue TypeFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// fraction of detail object color to use instead of the base map color in the environment:[0,1]
            /// </summary>
            public float ColorOverrideFactor;
            [TagField(Length = 0x8)]
            public byte[] Padding1;
            public float NearFadeDistance; // world units
            public float FarFadeDistance; // world units
            public float Size; // world units per pixel
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            public RealRgbColor MinimumColor; // [0,1]
            public RealRgbColor MaximumColor; // [0,1]
            public ArgbColor AmbientColor; // [0,255]
            [TagField(Length = 0x4)]
            public byte[] Padding3;
            
            public enum TypeFlagsValue : byte
            {
                Unused,
                Unused1,
                InterpolateColorInHsv,
                MoreColors
            }
        }
    }
}

