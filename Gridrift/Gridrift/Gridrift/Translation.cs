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
    }
}
