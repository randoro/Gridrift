using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{

    public class Chunk
    {
        public int xCoordinate, yCoordinate;
        public long lastUpdate; //saved in file
        public byte terrainPopulated; //saved in file
        public byte structurePopulated; //saved in file
        public long inhabitedTime; //saved in file
        public byte[] biomes = new byte[256]; //saved in file
        public byte[] blocks = new byte[256]; //saved in file
        public byte[] objects = new byte[256]; //saved in file

        /// <summary>
        ///  Holds all information a specific chunk has.
        /// </summary>
        public Chunk(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;

        }

        public Chunk()
        {

        }

        /// <summary>
        ///  Draws the chunk on screen.
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {

            RenderTarget.drawChunk(spriteBatch, new Vector2(xCoordinate * Globals.chunkLength * Globals.tileLength, yCoordinate * Globals.chunkLength * Globals.tileLength), ref blocks);

        }
    }
}
