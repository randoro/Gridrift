using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    class Chunk
    {
        public int xCoordinate, yCoordinate;
        public long lastUpdate { set; get; } //saved in file
        public byte terrainPopulated { set; get; } //saved in file
        public long inhabitedTime { set; get; } //saved in file
        public byte[] biomes { set; get; } //saved in file
        public byte[] blocks { set; get; } //saved in file
        public byte[] objects { set; get; } //saved in file

        /// <summary>
        ///  Holds all information a specific chunk has.
        /// </summary>
        public Chunk(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }

        /// <summary>
        ///  Draws the chunk on screen.
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    spriteBatch.Draw(Globals.testPigTexture, new Rectangle(xCoordinate * 512 + j * 32, yCoordinate * 512 + i * 32, 32, 32), Color.White);
                }
                
            }
           
            
        }
    }
}
