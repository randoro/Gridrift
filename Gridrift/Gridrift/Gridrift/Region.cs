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

        public Region(World world, Point regionID, FileStream fileStream)
        {
            this.world = world;
            this.regionID = regionID;
            this.fileStream = fileStream;
        }
    }
}
