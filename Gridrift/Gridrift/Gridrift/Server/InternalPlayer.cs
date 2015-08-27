using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Server
{
    public class InternalPlayer
    {
        private Point position;
        private long locationTimeStamp;

        public InternalPlayer(Point position, long locationTimeStamp) 
        {
            this.position = position;
            this.locationTimeStamp = locationTimeStamp;
        }

        public Point getPosition()
        {
            return position;
        }

        public void setPosition(Point newPosition)
        {
            position = newPosition;
        }

        public long getLocationTimeStamp()
        {
            return locationTimeStamp;
        }

        public void setLocationTimeStamp(long locationTimeStamp)
        {
            this.locationTimeStamp = locationTimeStamp;
        }
    }
}
