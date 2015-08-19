using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
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
                returnPoint.X = (int)position.X / 512 - 1;
            }

            if (position.Y >= 0)
            {
                returnPoint.Y = (int)position.Y / 512;
            }
            else
            {
                returnPoint.Y = (int)position.Y / 512 - 1;
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
                returnPoint.X = (int)chunkCoords.X / 33 - 1;
            }

            if (chunkCoords.Y >= 0)
            {
                returnPoint.Y = (int)chunkCoords.Y / 32;
            }
            else
            {
                returnPoint.Y = (int)chunkCoords.Y / 33 - 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in region coordinates as a Point and converts it to the first chunk's coordinates {0,0} in Point
        /// </summary>
        public static Point regionCoordsToFirstChunkCoords(Point regionCoords)
        {

            Point returnPoint = new Point(0, 0);
            if (regionCoords.X >= 0)
            {
                returnPoint.X = (int)regionCoords.X * 32;
            }
            else
            {
                returnPoint.X = (int)regionCoords.X * 33 + 1;
            }

            if (regionCoords.Y >= 0)
            {
                returnPoint.Y = (int)regionCoords.Y * 32;
            }
            else
            {
                returnPoint.Y = (int)regionCoords.Y * 33 + 1;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in an chunk coordinates as a Point and converts it to internal chunk coordinates within a region 0-15 in Point
        /// </summary>
        public static Point chunkCoordsToInternalRegionChunkCoords(Point chunkCords)
        {
            Point returnPoint = new Point(0, 0);
            if (chunkCords.X >= 0)
            {
                returnPoint.X = (int)chunkCords.X  % 32;
            }
            else
            {
                returnPoint.X = (32 + ((int)chunkCords.X) % 32) % 32;
            }
            if (chunkCords.Y >= 0)
            {
                returnPoint.Y = (int)chunkCords.Y % 32;
            }
            else
            {
                returnPoint.Y = (32 + ((int)chunkCords.Y) % 32) % 32;
            }

            return returnPoint;
        }

        /// <summary>
        ///  Takes in two points and checks if the distance from one of the points to the other is valid within the reach parameter. True = difference between position1 and position2 is equal or less than reach
        /// </summary>
        public static bool withinReach(Point position1, Point position2, int reach)
        {
            int xDifference;
            int yDifference;

            //if (position1.X <= position2.X)
            //{
                //xDifference = position2.X - position1.X;
            //}
            //else
           // {
                xDifference = position1.X - position2.X;
                int absX = Math.Abs(xDifference);
            //}
                if (absX > reach)
            {
                //x out of reach
                return false;
            }
            //if (position1.Y <= position2.Y)
            //{
            //    yDifference = position2.Y - position1.Y;
            //}
            //else
            //{
                yDifference = position1.Y - position2.Y;
                int absY = Math.Abs(yDifference);
            //}
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
