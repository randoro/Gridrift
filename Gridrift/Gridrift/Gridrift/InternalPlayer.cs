using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    class InternalPlayer
    {
        private Point position;

        public InternalPlayer(Point position) 
        {
            this.position = position;
        }

        public Point getPosition()
        {
            return position;
        }

        public void setPosition(Point newPosition)
        {
            position = newPosition;
        }
    }
}
