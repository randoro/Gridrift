﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public static class RenderTarget
    {

        public static void drawChunk(SpriteBatch spriteBatch, Vector2 position, ref byte[] blocks)
        {
            for (int i = 0; i < Globals.chunkLength; i++)
            {
                for (int j = 0; j < Globals.chunkLength; j++)
                {
                    spriteBatch.Draw(Globals.testPigTexture, new Vector2(position.X + j * Globals.tileLength, position.Y + i * Globals.tileLength), new Rectangle(200, 200, 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }

            }
        }
    }
}