﻿using Gridrift.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Server
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
