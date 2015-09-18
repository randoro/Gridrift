using Gridrift.Rendering;
using Gridrift.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    class IconButton : Button
    {
        int frame = 0;
        int frameTimer = 0;

        public IconButton(Vector2 position, float layerDepth)
        {
            this.layerDepth = layerDepth;
            this.position = position;
            surfaceArea = new Rectangle((int)position.X, (int)position.Y, 38, 38);
        }

        public override void update(GameTime gameTime)
        {
            frameTimer++;
            if (frameTimer > 1)
            {
                frame++;
                frame %= 32;
                frameTimer = 0;
            }
            if(surfaceArea.Contains(KeyMouseReader.MousePosition())) 
            {
                if(KeyMouseReader.LeftClick()) 
                {
                    doClickAction();
                }
            }

            if (KeyMouseReader.KeyPressed(Keys.F))
            {
                doClickAction();
            }
        }


        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 cameraPos = Camera.cameraPosition();
            //left end
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X, cameraPos.Y + position.Y), new Rectangle(1, 1, 38, 38), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);

            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 3, cameraPos.Y + position.Y + 3), new Rectangle(74 + (33 * (frame % 16)), 1 + (33 * (frame / 16)), 32, 32), new Color(0f, 0f, 0f, 0.7f), 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth + 0.001f);
            

        }
    }
}
