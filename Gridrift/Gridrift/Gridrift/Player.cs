using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    static class Player
    {
        private static Vector2 position;

        private static Vector2 velocity;

        public static Point getPosition()
        {
            return new Point((int)position.X, (int)position.Y);
        }

        public static void changeVelocity(Vector2 addedVelocity)
        {
            velocity += addedVelocity;
            if (velocity.X > 0)
            {
                velocity.X = Math.Min(velocity.X, 4);
            }
            else
            {
                velocity.X = Math.Max(velocity.X, -4);
            }
            if (velocity.Y > 0)
            {
                velocity.Y = Math.Min(velocity.Y, 4);
            }
            else
            {
                velocity.Y = Math.Max(velocity.Y, -4);
            }
        }

        public static void updatePosition()
        {
            position += velocity;
            decreaseVelocity();
        }

        private static void decreaseVelocity()
        {
            if (velocity.X > 0.1f)
            {
                velocity.X -= 0.2f;
            }
            else if (velocity.X < -0.1f)
            {
                velocity.X += 0.2f;
            }
            else
            {
                velocity.X = 0;
            }

            if (velocity.Y > 0.1f)
            {
                velocity.Y -= 0.2f;
            }
            else if (velocity.Y < -0.1f)
            {
                velocity.Y += 0.2f;
            }
            else
            {
                velocity.Y = 0;
            }
        }
    }
}
