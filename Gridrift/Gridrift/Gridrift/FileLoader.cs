using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public static class FileLoader
    {

        public static Region loadRegionFile(World world, Point regionID)
        {
            checkAndCreateFolder(world.getRegionPath());

            try
            {
                FileStream fileStream = new FileStream(world.getRegionPath() + "\\x" + regionID.X + ".y" + regionID.Y + ".region", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete);
                return new Region(world, regionID, fileStream);
            }
            catch (IOException)
            {
                Console.WriteLine("region: x:" + regionID.X + " y:" + regionID.Y + " error");
            }
            return null;
        }

        public static void checkAndCreateFolder(String pathAndFolder)
        {
            if (!Directory.Exists(Globals.gamePath + pathAndFolder))
            {
                Directory.CreateDirectory(Globals.gamePath + pathAndFolder);
            }
        }



    }
}
