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
            FileLoader.loadRegionFile(testWorld, new Point(0, 0));
            getChunk(testWorld, new Point(32, 32));

        }

        public Chunk getChunk(World world, Point chunkCordinates)
        {
            Point regionValue = Translation.chunkCoordsToRegionCoords(chunkCordinates);
            Region fetchedRegion;
            bool regionInDictionary = regionList.TryGetValue(Tuple.Create(regionValue.X, regionValue.Y), out fetchedRegion);
            if (regionInDictionary)
            {
                //get chunk
            }
            else
            {

                regionList.Add(Tuple.Create(regionValue.X, regionValue.Y), FileLoader.loadRegionFile(world, regionValue));
                //load region
            }
            return null;
        }


    }
}
