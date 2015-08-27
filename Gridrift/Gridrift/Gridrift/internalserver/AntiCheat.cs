using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public static class AntiCheat
    {



        public static bool checkAndChangePlayerPosition(InternalPlayer player, Point newLocation, long newTimeStamp)
        {
            Point oldLocation = player.getPosition();
            long oldTimeStamp = player.getLocationTimeStamp();

            Point difference = new Point(Math.Abs(oldLocation.X - newLocation.X), Math.Abs(oldLocation.Y - newLocation.Y));

            long timeDifference = Math.Abs(oldTimeStamp - newTimeStamp);

            double seconds = (double)timeDifference / TimeSpan.TicksPerSecond;

            double tilesPerSecondX = ((double)difference.X) / seconds;
            double tilesPerSecondY = ((double)difference.Y) / seconds;

            if (tilesPerSecondX > 300)
            {
                //is traveling too fast
                Player.setPosition(oldLocation);
                player.setLocationTimeStamp(newTimeStamp);
                return true;
            }
            if (tilesPerSecondY > 300)
            {
                //is traveling too fast
                Player.setPosition(oldLocation);
                player.setLocationTimeStamp(newTimeStamp);
                return true;
            }
            player.setPosition(newLocation);
            player.setLocationTimeStamp(newTimeStamp);
            return false;
        }
    }
}
