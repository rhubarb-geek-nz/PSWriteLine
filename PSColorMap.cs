// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Drawing;

namespace RhubarbGeekNz.PSWriteLine
{
    internal sealed class PSColorMap
    {
        internal struct ColorMap
        {
            internal readonly ConsoleColor ConsoleColor;
            internal readonly Color Color;
            internal ColorMap(ConsoleColor cc, Color c)
            {
                ConsoleColor = cc;
                Color = c;
            }
        }

        internal readonly static ColorMap[] ConsoleColorMap = new ColorMap[]
        {
            new ColorMap(ConsoleColor.Black, Color.FromArgb(0xFF, 0, 0, 0)),
            new ColorMap(ConsoleColor.DarkRed, Color.FromArgb(0xFF, 0x80, 0, 0)),
            new ColorMap(ConsoleColor.DarkGreen, Color.FromArgb(0xFF, 0, 0x80, 0)),
            new ColorMap(ConsoleColor.DarkYellow, Color.FromArgb(0xFF, 0x80, 0x80, 0)),
            new ColorMap(ConsoleColor.DarkBlue, Color.FromArgb(0xFF, 0, 0, 0x80)),
            new ColorMap(ConsoleColor.DarkMagenta, Color.FromArgb(0xFF, 0x80, 0, 0x80)),
            new ColorMap(ConsoleColor.DarkCyan, Color.FromArgb(0xFF, 0x00, 0x80, 0x80)),
            new ColorMap(ConsoleColor.Gray, Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0)),
            new ColorMap(ConsoleColor.DarkGray, Color.FromArgb(0xFF, 0x80, 0x80, 0x80)),
            new ColorMap(ConsoleColor.Red, Color.FromArgb(0xFF, 0xFF, 0, 0)),
            new ColorMap(ConsoleColor.Green, Color.FromArgb(0xFF, 0, 0xFF, 0)),
            new ColorMap(ConsoleColor.Yellow, Color.FromArgb(0xFF, 0xFF, 0xFF, 0)),
            new ColorMap(ConsoleColor.Blue, Color.FromArgb(0xFF, 0, 0, 0xFF)),
            new ColorMap(ConsoleColor.Magenta, Color.FromArgb(0xFF, 0xFF, 0, 0xFF)),
            new ColorMap(ConsoleColor.Cyan, Color.FromArgb(0xFF, 0, 0xFF, 0xFF)),
            new ColorMap(ConsoleColor.White, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
        };

        private static int GetDistance(Color color, Color baseColor)
        {
            int r = color.R - baseColor.R,
                g = color.G - baseColor.G,
                b = color.B - baseColor.B;
            return r * r + g * g + b * b;
        }

        internal static ConsoleColor GetNearestConsoleColor(Color baseColor)
        {
            int current = Int32.MaxValue;
            int index = ConsoleColorMap.Length, value = 0;

            while (0 != (index--))
            {
                int distance = GetDistance(ConsoleColorMap[index].Color, baseColor);

                if (distance < current)
                {
                    value = index;
                    current = distance;

                    if (distance == 0)
                    {
                        break;
                    }
                }
            }

            return ConsoleColorMap[value].ConsoleColor;
        }
    }
}
