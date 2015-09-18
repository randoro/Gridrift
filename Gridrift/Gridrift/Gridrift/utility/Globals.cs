using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Gridrift.Utility
{

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
        public static Texture2D testGUITexture;


        public static SpriteFont testFont;


    }
}
