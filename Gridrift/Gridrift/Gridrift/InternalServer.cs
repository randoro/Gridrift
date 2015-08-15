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
        Dictionary<String, InternalPlayer> playerList;
        long lastsyncUpdate;
        private const int secondsTimeToLiveForRegions = 5;

        public InternalServer(bool online)
        {
            isOnline = online;
            regionList = new Dictionary<Tuple<int, int>, Region>();
            playerList = new Dictionary<String, InternalPlayer>();
            lastsyncUpdate = DateTime.Now.Ticks;
            playerList.Add("offlinePlayer", new InternalPlayer(Tuple.Create(0, 0)));
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

        public void syncUpdate()
        {
            DateTime now = DateTime.Now;
            long nowTicks = now.Ticks;
            if (nowTicks > (lastsyncUpdate + (TimeSpan.TicksPerSecond) / 20))
            {
                lastsyncUpdate = lastsyncUpdate + (TimeSpan.TicksPerSecond / 20);

                //Make changes - this happens 20 times per second ideally
                foreach (KeyValuePair<Tuple<int, int>, Region> pair in regionList)
                {
                    Region region = pair.Value;
                    Point regionID = new Point(pair.Key.Item1, pair.Key.Item2);
                    if (nowTicks > (region.lastUsedTick + (TimeSpan.TicksPerSecond * secondsTimeToLiveForRegions)))
                    {
                        Console.WriteLine("Region x:" + regionID.X + " y:" + regionID.Y + " TTL expired, unloading.");
                        region.unloadRegion();
                        regionList.Remove(pair.Key); //temporary not saving lel
                        break;
                    }
                }

            }
        }

        

        public Chunk getChunk(World world, Point chunkCordinates)
        {
            Console.WriteLine("Regions in InternalServer regionList: "+regionList.Count);

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
                if (newRegion != null)
                {
                    regionList.Add(Tuple.Create(regionValue.X, regionValue.Y), newRegion);
                    return newRegion.getChunk(chunkCordinates);
                }
                else
                {
                    getChunk(world, chunkCordinates);
                }
            }
            return null;
        }


    }
}
