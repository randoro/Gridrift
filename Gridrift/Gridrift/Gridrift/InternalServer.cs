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

        public InternalServer(bool online)
        {
            isOnline = online;

            FileLoader.loadRegionFile(testWorld, new Point(0, 0));
        }


    }
}
