using Gridrift.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Rendering
{
    public static class Camera
    {
        private static bool lockedOnPlayer = true;

        public static Vector2 cameraPosition()
        {
            if (lockedOnPlayer)
            {
                Point tempPoint = Player.getPosition();
                return new Vector2(tempPoint.X - (Globals.currentWindowWidth / 2), tempPoint.Y - (Globals.currentWindowHeight / 2));
            }

            return Vector2.Zero;
        }
    }
}
