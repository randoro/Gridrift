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
        ///  Takes in an chunk coordinates as a Point and converts it to region coordinates in Point
        /// </summary>
        public static Point chunkCoordsToRegionCoords(Point chunkCords)
        {

            Point returnPoint = new Point(0, 0);
            if (chunkCords.X >= 0)
            {
                returnPoint.X = (int)chunkCords.X / 32;
            }
            else
            {
                returnPoint.X = (int)chunkCords.X / 33 - 1;
            }

            if (chunkCords.Y >= 0)
            {
                returnPoint.Y = (int)chunkCords.Y / 32;
            }
            else
            {
                returnPoint.Y = (int)chunkCords.Y / 33 - 1;
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
    }
}
