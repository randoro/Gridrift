using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Utility
{
    public static class Translation
    {
        /// <summary>
        ///  Takes in an exact position Point and converts it to chunk coordinates in Point
        /// </summary>
        public static Point exactPosToChunkCoords(Point position)
        {

            Point returnPoint = new Point(0, 0);
            if (position.X >= 0)
            {
                returnPoint.X = (int)position.X / 512;
            }
            else
            {
                returnPoint.X = (int)((position.X + 1) / 512) - 1;
                //returnPoint.X = (int)position.X / 512 - 1;
            }

            if (position.Y >= 0)
            {
                returnPoint.Y = (int)position.Y / 512;
            }
            else
            {
                returnPoint.Y = (int)((position.Y + 1) / 512) - 1;
                //returnPoint.Y = (int)position.Y / 512 - 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in an exact position Point and converts it to block coordinates in Point
        /// </summary>
        public static Point exactPosToBlockCoords(Point position)
        {

            Point returnPoint = new Point(0, 0);
            if (position.X >= 0)
            {
                returnPoint.X = (int)position.X / 32;
            }
            else
            {
                returnPoint.X = (int)((position.X + 1) / 32) - 1;
                //returnPoint.X = (int)position.X / 512 - 1;
            }

            if (position.Y >= 0)
            {
                returnPoint.Y = (int)position.Y / 32;
            }
            else
            {
                returnPoint.Y = (int)((position.Y + 1) / 32) - 1;
                //returnPoint.Y = (int)position.Y / 512 - 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in an block position Point and converts it to chunk coordinates in Point
        /// </summary>
        public static Point blockPosToChunkCoords(Point blockPos)
        {

            Point returnPoint = new Point(0, 0);
            if (blockPos.X >= 0)
            {
                returnPoint.X = (int)blockPos.X / 16;
            }
            else
            {
                returnPoint.X = (int)((blockPos.X + 1) / 16) - 1;
                //returnPoint.X = (int)position.X / 512 - 1;
            }

            if (blockPos.Y >= 0)
            {
                returnPoint.Y = (int)blockPos.Y / 16;
            }
            else
            {
                returnPoint.Y = (int)((blockPos.Y + 1) / 16) - 1;
                //returnPoint.Y = (int)position.Y / 512 - 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in chunk coordinates as a Point and converts it to region coordinates in Point
        /// </summary>
        public static Point chunkCoordsToRegionCoords(Point chunkCoords)
        {

            Point returnPoint = new Point(0, 0);
            if (chunkCoords.X >= 0)
            {
                returnPoint.X = (int)chunkCoords.X / 32;
            }
            else
            {
                returnPoint.X = (int)((chunkCoords.X + 1) / 32) - 1;
            }

            if (chunkCoords.Y >= 0)
            {
                returnPoint.Y = (int)chunkCoords.Y / 32;
            }
            else
            {
                returnPoint.Y = (int)((chunkCoords.Y + 1) / 32) - 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in region coordinates as a Point and converts it to the first chunk's coordinates {0,0} in Point
        /// </summary>
        public static Point regionCoordsToFirstChunkCoords(Point regionCoords)
        {
             return new Point(regionCoords.X * 32, regionCoords.Y * 32);

        }

        /// <summary>
        ///  Takes in chunk coordinates as a Point and converts it to the first block's coordinates {0,0} in Point
        /// </summary>
        public static Point chunkCoordsToFirstBlockCoords(Point chunkCoords)
        {
            return new Point(chunkCoords.X * 16, chunkCoords.Y * 16);

        }

        /// <summary>
        ///  Takes in an chunk coordinates as a Point and converts it to internal chunk coordinates within a region 0-15 in Point
        /// </summary>
        public static Point chunkCoordsToInternalRegionChunkCoords(Point chunkCords)
        {
            Point regionID = Translation.chunkCoordsToRegionCoords(chunkCords);
            Point firstChunk = Translation.regionCoordsToFirstChunkCoords(regionID);
            
            return new Point(chunkCords.X - firstChunk.X, chunkCords.Y - firstChunk.Y);

        }


        /// <summary>
        ///  Takes in block coordinates as a Point and converts it to internal block coordinates within a chunk 0-15 in Point
        /// </summary>
        public static Point blockCoordsToInternalChunkBlockCoords(Point blockCords)
        {
            Point chunkID = Translation.blockPosToChunkCoords(blockCords);
            Point firstBlock = Translation.chunkCoordsToFirstBlockCoords(chunkID);

            return new Point(blockCords.X - firstBlock.X, blockCords.Y - firstBlock.Y);
        }

        /// <summary>
        ///  Takes in two points and checks if the distance from one of the points to the other is valid within the reach parameter. True = difference between position1 and position2 is equal or less than reach
        /// </summary>
        public static bool withinReach(Point position1, Point position2, int reach)
        {
            int xDifference;
            int yDifference;

                xDifference = position1.X - position2.X;
                int absX = Math.Abs(xDifference);
                if (absX > reach)
                {
                    //x out of reach
                    return false;
                }

                yDifference = position1.Y - position2.Y;
                int absY = Math.Abs(yDifference);
                if (absY > reach)
                {
                    //y out of reach
                    return false;
                }

                //both x and y are within reach
                return true;
        }
    }
}
