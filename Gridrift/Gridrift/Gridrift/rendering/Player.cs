using Gridrift.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Rendering
{
    static class Player
    {
        private static Vector2 position = new Vector2(0, 0);

        private static Vector2 velocity;

        private static Direction direction = Direction.None;

        private static int animationCounter = 0;
        private static int walkingFrame = 0;

        public static Point getPosition()
        {
            return new Point((int)position.X, (int)position.Y);
        }
        public static void setPosition(Point newPosition)
        {
            position.X = (float)newPosition.X;
            position.Y = (float)newPosition.Y;
        }

        /// <summary>
        ///  Adds a direction velocity to the entity
        /// </summary>
        public static void changeVelocity(Vector2 addedVelocity)
        {
            int maxSpeed = 3;
            velocity += addedVelocity;
            if (velocity.X > 0)
            {
                velocity.X = Math.Min(velocity.X, maxSpeed);
            }
            else
            {
                velocity.X = Math.Max(velocity.X, -maxSpeed);
            }
            if (velocity.Y > 0)
            {
                velocity.Y = Math.Min(velocity.Y, maxSpeed);
            }
            else
            {
                velocity.Y = Math.Max(velocity.Y, -maxSpeed);
            }
        }

        /// <summary>
        ///  Calculate players new position with new velocity and friction.
        /// </summary>
        public static void updatePosition()
        {
            position += velocity;
            setDirection();
            decreaseVelocity();

        }

        /// <summary>
        ///  Adds friction to velocity movement and sets direction variable.
        /// </summary>
        private static void decreaseVelocity()
        {
            if (velocity.X > 0.2f)
            {
                velocity.X -= 0.2f;
            }
            else if (velocity.X < -0.2f)
            {
                velocity.X += 0.2f;
            }
            else
            {
                velocity.X = 0;
            }

            if (velocity.Y > 0.2f)
            {
                velocity.Y -= 0.2f;
            }
            else if (velocity.Y < -0.2f)
            {
                velocity.Y += 0.2f;
            }
            else
            {
                velocity.Y = 0;
            }
        }

        private static void setDirection()
        {
            if (velocity.X > 0)
            {
                if (velocity.Y > 0)
                {
                    //moving south east
                    direction = Direction.SouthEast;
                }
                else if (velocity.Y < 0)
                {
                    //moving north east
                    direction = Direction.NorthEast;
                }
                else
                {
                    //moving east
                    direction = Direction.East;
                }
            }
            else if (velocity.X < 0)
            {
                if (velocity.Y > 0)
                {
                    //moving south west
                    direction = Direction.SouthWest;
                }
                else if (velocity.Y < 0)
                {
                    //moving north west
                    direction = Direction.NorthWest;
                }
                else
                {
                    //moving west
                    direction = Direction.West;
                }
            }
            else
            {
                if (velocity.Y > 0)
                {
                    //moving south
                    direction = Direction.South;
                }
                else if (velocity.Y < 0)
                {
                    //moving north
                    direction = Direction.North;
                }
                else
                {
                    //not moving
                    direction = Direction.None;
                }
                
            }
        }

        public static void update(GameTime gameTime)
        {
            if (animationCounter >= 8)
            {
                if (direction.Equals(Direction.None))
                {
                    walkingFrame = 0;
                }
                else
                {
                    walkingFrame++;
                    walkingFrame = walkingFrame % 4;
                }
                animationCounter = 0;
            }
            else
            {
                animationCounter++;
            }

            updatePosition();


        }

        public static void draw(SpriteBatch spriteBatch)
        {
            switch (direction)
            {
                case Direction.None:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(0, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.North:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(64, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.NorthEast:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(224, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.East:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(96, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.SouthEast:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(128, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.South:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(0, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.SouthWest:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(160, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.West:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(32, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                case Direction.NorthWest:
                    spriteBatch.Draw(Globals.testPlayerTexture, new Vector2(Player.getPosition().X, Player.getPosition().Y), new Rectangle(192, walkingFrame * 32, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);    
            
                    break;
                default:
                    break;
            }
            
                
            
        }
    }
}
