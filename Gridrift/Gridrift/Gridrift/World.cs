using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public class World
    {
        private String worldName;

        public World(String worldName)
        {
            this.worldName = worldName;

        }

        public String getRegionPath()
        {
            return Globals.gamePath + "\\" + worldName + "\\region";
        }
    }
}
