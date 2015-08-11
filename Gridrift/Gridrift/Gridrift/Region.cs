using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLibNet;

namespace Gridrift
{
    public class Region
    {
        private World world;
        private Point regionID;
        private FileStream fileStream;

        #region regionScheme
        private byte[] chunkIsPresent; //1 = chunk is there, 0 = chunk is not there.
        private int[] chunkPositionInFile; //position of chunk
        private int[] chunkSizeInBytes; //length of chunk
        #endregion

        public Region(World world, Point regionID, FileStream fileStream)
        {
            this.world = world;
            this.regionID = regionID;
            this.fileStream = fileStream;

            chunkIsPresent = new byte[1024];
            chunkPositionInFile = new int[1024];
            chunkSizeInBytes = new int[1024];
            tempCreateChunkScheme();
            //saveRegionScheme();
            loadRegionScheme();
            int test = 0;
            //Zipper z = new Zipper();
            //z.ZipFile = @"d:\test\my.zip";
            //z.ItemList.Add(name);
            //z.PathInZip = enPathInZip.Relative;
            //z.Recurse = true;
            //z.Zip();

            //UnZipper uz = new UnZipper();
            //uz.Destination = @"d:\test\out\";
            //uz.IfFileExist = enIfFileExist.Exception;
            //uz.ItemList.Add("*.*");
            //uz.Recurse = true;
            //uz.ZipFile = @"d:\test\my.zip";
            //uz.UnZip();

            

        }

        public void getChunk(Point chunkID)
        {
            Point checkingRegionID = Translation.chunkCoordsToRegionCoords(chunkID);

            

            //Makes sure the chunk is inside this specific region
            if (regionID.X == checkingRegionID.X && regionID.Y == checkingRegionID.Y)
            {
                Point internalChunkPos = Translation.chunkCoordsToInternalRegionChunkCoords(chunkID);
                int chunkIDInArrays = internalChunkPos.X + internalChunkPos.Y * 32;
                //Checks if chunk is present
                if (chunkIsPresent[chunkIDInArrays] == 1) 
                {
                    fetchByteArray(chunkPositionInFile[chunkIDInArrays], chunkSizeInBytes[chunkIDInArrays]);
                    
                }
            }
            else
            {
                throw new System.Exception("Trying to open a chunk in wrong region file.");
            }
            
            
        }

        private void loadRegionScheme()
        {
            fileStream.Position = 0;
            //fetch chunkIsPresent bytes
            chunkIsPresent = fetchByteArray(0, 1024);

            //fetch chunkPositionInFile ints
            chunkPositionInFile = fetchInt32Array(sizeof(byte)*1024, sizeof(int)*1024);

            //fetch chunkSizeInBytes ints
            chunkSizeInBytes = fetchInt32Array(sizeof(byte) * 1024 + sizeof(int) * 1024, sizeof(int) * 1024);
        }

        private void saveRegionScheme()
        {
            fileStream.Position = 0;
            //fetch chunkIsPresent bytes
            pushByteArray(chunkIsPresent, 0, chunkIsPresent.Length);

            //fetch chunkPositionInFile ints
            pushInt32Array(chunkPositionInFile, sizeof(byte) * 1024, chunkPositionInFile.Length);

            //fetch chunkSizeInBytes ints
            pushInt32Array(chunkSizeInBytes, sizeof(byte) * 1024 + sizeof(int) * 1024, chunkSizeInBytes.Length);
        }

        //test and try making a chunkScheme
        private void tempCreateChunkScheme()
        {
            Random rand = new Random();
            for (int i = 0; i < 1024; i++)
            {
            chunkIsPresent[i] = (byte)rand.Next(0, 2);
            chunkPositionInFile[i] = rand.Next(100, 150);
            chunkSizeInBytes[i] = rand.Next(666, 700);
            }
        }

        private byte[] fetchByteArray(int position, int length)
        {
            fileStream.Position = position;
            byte[] bytebuffer = new byte[length];
            fileStream.Read(bytebuffer, 0, length);
            return bytebuffer;
        }

        private int[] fetchInt32Array(int position, int length)
        {
            fileStream.Position = position;
            byte[] bytebuffer = new byte[length];
            fileStream.Read(bytebuffer, 0, length);

            int[] returnBuffer = new int[length / sizeof(int)];
            for (int i = 0; i < (length / sizeof(int)); i++)
            {
                returnBuffer[i] = BitConverter.ToInt32(bytebuffer, i*sizeof(int));
            }
            return returnBuffer;

        }

        private void pushByteArray(byte[] array, int position, int length)
        {
            fileStream.Position = position;
            fileStream.Write(array, position, length);
        }

        private void pushInt32Array(int[] array, int position, int length)
        {
            fileStream.Position = position;
            byte[] bytebuffer = new byte[length*sizeof(int)];
            for (int i = 0; i < length; i++)
            {
                byte[] intBytebuffer = new byte[sizeof(int)];
                intBytebuffer = BitConverter.GetBytes(array[i]);

                bytebuffer[i * sizeof(int)] = intBytebuffer[0];
                bytebuffer[i * sizeof(int) + 1] = intBytebuffer[1];
                bytebuffer[i * sizeof(int) + 2] = intBytebuffer[2];
                bytebuffer[i * sizeof(int) + 3] = intBytebuffer[3];
            }
            fileStream.Write(bytebuffer, 0, length * sizeof(int));
        }
    }
}
