using Gridrift.Rendering;
using Gridrift.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gridrift.Server
{
    class InternalServer
    {
        public bool isOnline;
        public static World testWorld = new World("world");
        Dictionary<Tuple<int,int>, Region> regionList;
        public Dictionary<Tuple<int, int>, Chunk> chunkList;
        Dictionary<String, InternalPlayer> playerList;
        long lastsyncUpdate;
        private const int secondsTimeToLiveForRegions = 5;
        public static int ISchunkCount = 0;
        public static int ISregionCount = 0;
        public bool isRunning { get; private set; }
        //public static bool isListening { get; private set; }
        private static Object serverLock = new Object();

        

        public InternalServer(bool online)
        {
            isOnline = online;
            regionList = new Dictionary<Tuple<int, int>, Region>();
            playerList = new Dictionary<String, InternalPlayer>();
            chunkList = new Dictionary<Tuple<int, int>, Chunk>();
            lastsyncUpdate = DateTime.Now.Ticks;
            playerList.Add("offlinePlayer", new InternalPlayer(Player.getPosition(), DateTime.Now.Ticks));
            


            //Console.WriteLine("0: " + Translation.regionCoordsToFirstChunkCoords(new Point(0, 0)));
            //Console.WriteLine("1: " + Translation.regionCoordsToFirstChunkCoords(new Point(1, 1)));
            //Console.WriteLine("31: " + Translation.regionCoordsToFirstChunkCoords(new Point(31, 31)));
            //Console.WriteLine("32: " + Translation.regionCoordsToFirstChunkCoords(new Point(32, 32)));
            //Console.WriteLine("33: " + Translation.regionCoordsToFirstChunkCoords(new Point(33, 33)));


            //Console.WriteLine("-1: " + Translation.regionCoordsToFirstChunkCoords(new Point(-1, -1)));
            //Console.WriteLine("-2: " + Translation.regionCoordsToFirstChunkCoords(new Point(-2, -2)));
            //Console.WriteLine("-32: " + Translation.regionCoordsToFirstChunkCoords(new Point(-32, -32)));
            //Console.WriteLine("-33: " + Translation.regionCoordsToFirstChunkCoords(new Point(-33, -33)));
            //Console.WriteLine("-34: " + Translation.regionCoordsToFirstChunkCoords(new Point(-34, -34)));

        }


        public void syncUpdate()
        {
            DateTime now = DateTime.Now;
            long nowTicks = now.Ticks;
            if (nowTicks > (lastsyncUpdate + (TimeSpan.TicksPerSecond) / 20))
            {
                lastsyncUpdate = lastsyncUpdate + (TimeSpan.TicksPerSecond / 20);
                //Make changes - this happens 20 times per second ideally

                ISchunkCount = chunkList.Count;
                ISregionCount = regionList.Count;

                InternalPlayer player;
                playerList.TryGetValue("offlinePlayer", out player);
                if (player != null)
                {
                    player.setPosition(Player.getPosition());
                    //bool isCheating = AntiCheat.checkAndChangePlayerPosition(player, Player.getPosition(), DateTime.Now.Ticks);
                    //if (isCheating)
                    //{
                    //    Console.WriteLine("player: offlinePlayer is moving too fast");
                    //}

                }

                bool stillRemovingChunks = true;
                while (stillRemovingChunks)
                {
                    stillRemovingChunks = unloadUnusedChunks();
                }

                bool stillRemovingRegions = true;
                while (stillRemovingRegions)
                {
                    stillRemovingRegions = unloadUnusedRegions(nowTicks);
                }




            }
        }

        public void closeServer()
        {
            //AsyncSocketListener.Instance.Dispose();
            isRunning = false;

            playerList.Remove("offlinePlayer");

            bool stillRemovingChunks = true;
            while (stillRemovingChunks)
            {
                stillRemovingChunks = unloadUnusedChunks();
            }

            bool stillRemovingRegions = true;
            while (stillRemovingRegions)
            {
                stillRemovingRegions = unloadUnusedRegions(DateTime.Now.Ticks + (TimeSpan.TicksPerSecond * (secondsTimeToLiveForRegions + 1)));
            }

        }



        public Chunk getChunk(World world, Point chunkCordinates)
        {
                Chunk alreadyLoadedChunk;
                Tuple<int, int> chunkCordinatesTuple = Tuple.Create(chunkCordinates.X, chunkCordinates.Y);
                if (chunkList.TryGetValue(chunkCordinatesTuple, out alreadyLoadedChunk))
                {
                    return alreadyLoadedChunk;
                }
                //Console.WriteLine("Regions in InternalServer regionList: "+regionList.Count);

                Point regionValue = Translation.chunkCoordsToRegionCoords(chunkCordinates);
                Region fetchedRegion;
                bool regionInDictionary = regionList.TryGetValue(Tuple.Create(regionValue.X, regionValue.Y), out fetchedRegion);
                if (regionInDictionary)
                {
                    //get chunk
                    Chunk newChunk = fetchedRegion.readChunk(chunkCordinates);
                    if (newChunk.terrainPopulated == 1)
                    {
                        chunkList.Add(chunkCordinatesTuple, newChunk);
                        
                        return newChunk;
                    }
                    else
                    {
                        return null; //continue until found
                    }
                }
                else
                {
                    //load region
                    Region newRegion = FileLoader.loadRegionFile(world, regionValue);
                    if (newRegion != null)
                    {
                        regionList.Add(Tuple.Create(regionValue.X, regionValue.Y), newRegion);
                        Chunk newChunk = newRegion.readChunk(chunkCordinates);
                        if (newChunk != null)
                        {
                            if (newChunk.terrainPopulated == 1)
                            {
                                chunkList.Add(chunkCordinatesTuple, newChunk);
                                return newChunk;
                            }
                            else
                            {
                                return null;
                                throw new Exception("Could not load chunk.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Could not load region file.");
                    }
                }
                return null;
        }


        public void unloadChunksOfRegion(Point regionID, Region regionToUnload)
        {
            Point firstChunkCoord = Translation.regionCoordsToFirstChunkCoords(regionID);
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    //Point currentChunkCoord = new Point(firstChunkCoord.X + j, firstChunkCoord.Y + i);
                    Tuple<int, int> currentChunkCoord = Tuple.Create(firstChunkCoord.X + j, firstChunkCoord.Y + i);
                    Chunk currentChunk;
                    chunkList.TryGetValue(currentChunkCoord, out currentChunk);
                    if (currentChunk != null)
                    {
                        if (currentChunk.terrainPopulated == 0)
                        {
                            throw new Exception("Trying to unload unfinished Chunk");
                        }
                        else
                        {

                            //save chunk to region object
                            regionToUnload.writeChunk(currentChunk);
                            //remove from chunkList
                            chunkList.Remove(currentChunkCoord);
                        }
                    }
                }
            }
        }

        public bool unloadUnusedChunks()
        {
            bool removedChunk = false;
            foreach (KeyValuePair<Tuple<int, int>, Chunk> chunkPair in chunkList)
            {
                Point currentChunkID = new Point(chunkPair.Key.Item1, chunkPair.Key.Item2);

                bool totalWithinReach = false;
                foreach (KeyValuePair<String, InternalPlayer> playerPair in playerList)
                {
                    if (totalWithinReach)
                    {
                        break;
                    }
                    InternalPlayer currentPlayer = playerPair.Value;
                    Point playerPosition = currentPlayer.getPosition();
                    Point playerChunkID = Translation.exactPosToChunkCoords(playerPosition);
                    totalWithinReach = Translation.withinReach(playerChunkID, currentChunkID, 3);
                }

                if (!totalWithinReach)
                {
                    //Console.WriteLine("IS chunk {" + currentChunkID.X + "," + currentChunkID.Y+"} unloaded.");
                    Point regionID = Translation.chunkCoordsToRegionCoords(currentChunkID);
                    Region currentRegion;

                    regionList.TryGetValue(Tuple.Create(regionID.X, regionID.Y), out currentRegion);
                    if (currentRegion != null)
                    {
                        if (chunkPair.Value.terrainPopulated == 0)
                        {
                            int test = 1;
                        }
                        else
                        {
                            currentRegion.writeChunk(chunkPair.Value);
                            removedChunk = true;
                            chunkList.Remove(chunkPair.Key);
                        }
                    }
                    break;
                }
            }
            return removedChunk;
        }

        public bool unloadUnusedRegions(long nowTicks)
        {
            bool removedRegion = false;
            foreach (KeyValuePair<Tuple<int, int>, Region> regionPair in regionList)
            {
                Region region = regionPair.Value;
                Point regionID = new Point(regionPair.Key.Item1, regionPair.Key.Item2);
                Point regionFirstChunkID = Translation.regionCoordsToFirstChunkCoords(regionID);
                if (nowTicks > (region.lastUsedTick + (TimeSpan.TicksPerSecond * secondsTimeToLiveForRegions)))
                {
                    bool shouldBeUnloaded = true;
                    foreach (KeyValuePair<String, InternalPlayer> playerPair in playerList)
                    {
                        InternalPlayer currentPlayer = playerPair.Value;
                        Point playerPosition = currentPlayer.getPosition();
                        Point playerChunkID = Translation.exactPosToChunkCoords(playerPosition);
                        Point playerRegionID = Translation.chunkCoordsToRegionCoords(playerChunkID);
                        Point playerFirstChunkID = Translation.regionCoordsToFirstChunkCoords(playerRegionID);
                        if (Translation.withinReach(regionFirstChunkID, playerFirstChunkID, 32))
                        {
                            region.lastUsedTick = DateTime.Now.Ticks;
                            shouldBeUnloaded = false;
                            break;
                        }

                    }

                    if (shouldBeUnloaded)
                    {
                        removedRegion = true;
                        unloadChunksOfRegion(regionID, region);
                        region.unloadRegion();
                        regionList.Remove(regionPair.Key); //temporary not saving lel
                        break;
                    }
                }
            }
            return removedRegion;
        }

        public void unloadChunk(Tuple<int, int> chunkToRemoveID)
        {
            Chunk chunkToRemove;

            chunkList.TryGetValue(chunkToRemoveID, out chunkToRemove);

            if (chunkToRemove != null)
            {
                Point chunkCoords = new Point(chunkToRemoveID.Item1, chunkToRemoveID.Item2);

                Point regionID = Translation.chunkCoordsToRegionCoords(chunkCoords);
                Region currentRegion;

                regionList.TryGetValue(Tuple.Create(regionID.X, regionID.Y), out currentRegion);
                if (currentRegion != null)
                {
                    if (chunkToRemove.terrainPopulated == 0)
                    {
                        int test = 1; //throw exception
                    }
                    else
                    {
                        currentRegion.writeChunk(chunkToRemove);
                        chunkList.Remove(chunkToRemoveID);
                    }
                }
            }
        }
    }
}
