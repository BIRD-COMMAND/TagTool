﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    public class ExportCommandsCommand : Command
    {
        [Flags]
        public enum ExportFlags
        {
            None = 0,
            NoDefault = (1 << 0),
        }

        private GameCache Cache;
        private TagStructure Structure;
        private ExportFlags Flags;

        public ExportCommandsCommand(GameCache cache, TagStructure structure)
            : base(false,

                  "ExportCommands",
                  "Exports the commands needed to generate the current tag structure",

                  "ExportCommands [NoDefault]",
                  "Exports the commands needed to generate the tag structure. Specify option 'nodefault' to omit default values.")
        {
            Cache = cache;
            Structure = structure;
        }

        public override object Execute(List<string> args)
        {
            ExportFlags flags = ExportFlags.None;
            if (args.Count > 0 && !ParseExportFlags(args, out flags))
                return new TagToolError(CommandError.ArgInvalid, $"Unknown option specified '{args[0]}'");

            Flags = flags;
            DumpCommands(Console.Out, Cache, Structure);
            return true;
        }

        private void DumpCommands(TextWriter writer, GameCache cache, object data, string fieldName = null)
        {
            if (Flags.HasFlag(ExportFlags.NoDefault) && IsDefaultValue(data))
                return;

            switch (data)
            {
                case TagStructure tagStruct:
                    {
                        foreach (var field in tagStruct.GetTagFieldEnumerable(cache.Version))
                            DumpCommands(writer, cache, field.GetValue(data), fieldName != null ? $"{fieldName}.{field.Name}" : field.Name);
                    }
                    break;
                case IList collection:
                    {
                        if (collection.Count > 0)
                        {
                            writer.WriteLine($"AddBlockElements {fieldName} {collection.Count}");
                            for (int i = 0; i < collection.Count; i++)
                                DumpCommands(writer, cache, collection[i], $"{fieldName}[{i}]");
                        }
                    }
                    break;
                default:
                    writer.WriteLine($"SetField {fieldName} {FormatValue(data)}");
                    break;
            }
        }

        private string FormatValue(object value)
        {
            switch (value)
            {
                case null:
                    return "null";
                case string str:
                    return $"\"{str}\"";
                case Angle angle:
                    return $"{angle.Degrees}";
                case RealEulerAngles2d angles2d:
                    return $"{angles2d.Yaw.Degrees} {angles2d.Pitch.Degrees}";
                case RealEulerAngles3d angles3d:
                    return $"{angles3d.Yaw.Degrees} {angles3d.Pitch.Degrees} {angles3d.Roll.Degrees}";
                case RealVector3d vector3d:
                    return $"{vector3d.I} {vector3d.J} {vector3d.K}";
                case RealQuaternion quaternion:
                    return $"{quaternion.I} {quaternion.J} {quaternion.K} {quaternion.W}";
                case RealPoint3d point3d:
                    return $"{point3d.X} {point3d.Y} {point3d.Z}";
                case RealPoint2d point2d:
                    return $"{point2d.X} {point2d.Y}";
                case RealPlane3d plane3d:
                    return $"{plane3d.I} {plane3d.J} {plane3d.K} {plane3d.D}";
                case RealPlane2d plane2d:
                    return $"{plane2d.I} {plane2d.J} {plane2d.D}";
                case RealArgbColor realArgb:
                    return $"{realArgb.Alpha} {realArgb.Red} {realArgb.Green} {realArgb.Blue}";
                case ArgbColor argb:
                    return $"{argb.Alpha} {argb.Red} {argb.Green} {argb.Blue}";
                case RealRgbColor realRgb:
                    return $"{realRgb.Red} {realRgb.Green} {realRgb.Blue}";
                case RealRectangle3d realRect3d:
                    return $"{realRect3d.X0} {realRect3d.X1} {realRect3d.Y0} {realRect3d.Y1} {realRect3d.Z0} {realRect3d.Z1}";
                case Rectangle2d rect2d:
                    return $"{rect2d.Top} {rect2d.Left} {rect2d.Bottom} {rect2d.Right}";
                case RealMatrix4x3 realMatrix4x3:
                    return FormatRealMatrix4x3(realMatrix4x3);
                case IBounds bounds:
                    return FormatBounds(bounds);
                case DatumHandle datumHandle:
                    return $"{datumHandle.Salt} {datumHandle.Index}";
                case StringId stringId:
                    return Cache.StringTable.GetString(stringId);
                default:
                    return $"{value}";
            }
        }

        private string FormatBounds(IBounds bounds)
        {
            var boundsType = bounds.GetType();
            var lower = boundsType.GetProperty("Lower").GetValue(bounds);
            var upper = boundsType.GetProperty("Upper").GetValue(bounds);
            return $"{FormatValue(lower)} {FormatValue(upper)}";
        }

        private string FormatRealMatrix4x3(RealMatrix4x3 matrix)
        {
            return
                $"{matrix.m11} {matrix.m12} {matrix.m13}" +
                $"{matrix.m21} {matrix.m22} {matrix.m23}" +
                $"{matrix.m31} {matrix.m32} {matrix.m33}" +
                $"{matrix.m41} {matrix.m42} {matrix.m43}";
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var valueProperty = type.GetProperty("Value");
                type = valueProperty.PropertyType;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private static object GetDefaultValue(object value)
        {
            if (value == null)
                return null;
            if (value is StringId)
                return StringId.Invalid;

            return GetDefaultValue(value.GetType());
        }

        private static bool IsDefaultValue(object value)
        {
            var defaultValue = GetDefaultValue(value);
            if (value == null)
                return defaultValue == null;
            if (value is string s)
                return string.IsNullOrEmpty(s);

            return value.Equals(defaultValue);
        }

        private static bool ParseExportFlags(List<string> args, out ExportFlags flags)
        {
            flags = ExportFlags.None;

            var names = Enum.GetNames(typeof(ExportFlags));
            var values = Enum.GetValues(typeof(ExportFlags));
            while (args.Count > 0)
            {
                var arg = args[0];
                bool not = arg.StartsWith("!");
                if (not) arg = arg.Substring(1);

                for (int i = 0; i < names.Length; i++)
                {
                    if (string.Equals(arg, names[i], StringComparison.OrdinalIgnoreCase))
                    {
                        if (not)
                            flags &= ~(ExportFlags)values.GetValue(i);
                        else
                            flags |= (ExportFlags)values.GetValue(i);

                        args.RemoveAt(0);
                    }
                }
                if (args.Count > 0)
                    return false;
            }
            return true;
        }
    }
}