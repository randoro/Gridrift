using Gridrift.Rendering;
using Gridrift.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    class ValueBar : GUIObject
    {
        int totalLength;
        int barLength;
        int valueToPaint;
        protected float percentValue;

        public ValueBar(Vector2 position, int totalLength, float layerDepth)
        {
            this.totalLength = totalLength;
            this.layerDepth = layerDepth;
            this.position = position;
            surfaceArea = new Rectangle((int)position.X, (int)position.Y, totalLength, 11);

            barLength = totalLength - 4;
            valueToPaint = (int)((float)barLength * percentValue);
        }

        public override void update(GameTime gameTime)
        {
            valueToPaint = (int)((float)barLength * percentValue);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 cameraPos = Camera.cameraPosition();
            //left end
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X, cameraPos.Y + position.Y), new Rectangle(1, 49, 2, 11), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            for (int i = 0; i < barLength; i++)
            {
                //middlebackground
                spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 2 + i, cameraPos.Y + position.Y), new Rectangle(4, 49, 1, 11), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);         
            }
            //right end
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 2 + barLength, cameraPos.Y + position.Y), new Rectangle(6, 49, 2, 11), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);

            for (int i = 0; i < valueToPaint; i++)
            {
                //valuepainted
                spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 2 + i, cameraPos.Y + position.Y + 2), new Rectangle(9, 49, 1, 7), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth + 0.001f);
            }

        }
    }
}
