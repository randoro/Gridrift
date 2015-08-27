﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public enum Direction { None, North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    public enum TagID { End, Byte, Short, Int, Long, Float, Double, ByteArray, String, List, Compound, IntArray };

    /// <summary>
    ///  Global Objects and variables.
    /// </summary>
    public static class Globals
    {
        //constants
        

        /// <summary>
        ///  Amount of pixels per tile length.
        /// </summary>
        public const int tileLength = 32;

        /// <summary>
        ///  Amount of tiles per chunk length.
        /// </summary>
        public const int chunkLength = 16;

        /// <summary>
        ///  Amount of chunks per region length.
        /// </summary>
        public const int regionLength = 32;



        //global variables
        public static int currentWindowWidth;
        public static int currentWindowHeight;
        public static readonly String gamePath = AppDomain.CurrentDomain.BaseDirectory;


        //test variables
        public static Texture2D testPlayerTexture;
        public static Texture2D testBackgroundTexture;
        public static SpriteFont testFont;


    }
}