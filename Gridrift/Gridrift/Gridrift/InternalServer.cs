using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Gridrift
{
    class InternalServer
    {
        public bool isOnline;
        public static World testWorld = new World("world");
        Dictionary<Tuple<int,int>, Region> regionList;

        public InternalServer(bool online)
        {
            isOnline = online;
            regionList = new Dictionary<Tuple<int, int>, Region>();

            //Console.WriteLine("0: "+Translation.chunkCoordsToInternalRegionChunkCoords(new Point(0, 0)));
            //Console.WriteLine("1: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(1, 1)));
            //Console.WriteLine("31: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(31, 31)));
            //Console.WriteLine("32: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(32, 32)));
            //Console.WriteLine("33: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(33, 33)));


            //Console.WriteLine("-1: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(-1, -1)));
            //Console.WriteLine("-2: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(-2, -2)));
            //Console.WriteLine("-32: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(-32, -32)));
            //Console.WriteLine("-33: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(-33, -33)));
            //Console.WriteLine("-34: " + Translation.chunkCoordsToInternalRegionChunkCoords(new Point(-34, -34)));

        }

        public Chunk getChunk(World world, Point chunkCordinates)
        {
            if (chunkCordinates.X == 0 && chunkCordinates.Y == 0)
            {
                int test = 1;
            }

            Point regionValue = Translation.chunkCoordsToRegionCoords(chunkCordinates);
            Region fetchedRegion;
            bool regionInDictionary = regionList.TryGetValue(Tuple.Create(regionValue.X, regionValue.Y), out fetchedRegion);
            if (regionInDictionary)
            {
                //get chunk
                return fetchedRegion.getChunk(chunkCordinates);
            }
            else
            {
                //load region
                Region newRegion = FileLoader.loadRegionFile(world, regionValue);
                regionList.Add(Tuple.Create(regionValue.X, regionValue.Y), newRegion);
                return newRegion.getChunk(chunkCordinates);
            }
            return null;
        }


    }
}
