using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public class Region
    {
        private World world;
        private Point regionID;
        private FileStream fileStream;
        private Int32[] chunkScheme;

        public Region(World world, Point regionID, FileStream fileStream)
        {
            this.world = world;
            this.regionID = regionID;
            this.fileStream = fileStream;

            chunkScheme = new Int32[1024];
            tempCreateChunkScheme();
        }

        public void getChunk(Point chunkID)
        {
            Point checkingRegionID = Translation.chunkCoordsToRegionCoords(chunkID);

            

            //Makes sure the chunk is inside this specific region
            if (regionID.X == checkingRegionID.X && regionID.Y == checkingRegionID.Y)
            {

            }
            else
            {
                throw new System.Exception();
            }
            
        }

        private void updateChunkScheme()
        {
            fileStream.Position = 0;
            byte[] buffer = new byte[4];

            for (int i = 0; i < 1024; i++)
			{
			fileStream.Read(buffer, 0, 4);
            chunkScheme[i] = BitConverter.ToInt32(buffer, 0);
			}
            
        }
        //test and try making a chunkScheme
        private void tempCreateChunkScheme()
        {
            fileStream.Position = 1024 * 256;
            Random rand = new Random();
            byte[] buffer;
            for (int i = 0; i < 1024; i++)
            {
                Int32 numberToWrite = rand.Next(0, 100);
                buffer = BitConverter.GetBytes(numberToWrite);
                fileStream.Write(buffer, 0, 4);
            }
        }
    }
}
