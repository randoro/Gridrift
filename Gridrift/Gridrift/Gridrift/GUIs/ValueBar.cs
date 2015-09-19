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
        protected int barLength;
        protected int valueToPaint;
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
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X, cameraPos.Y + position.Y), new Rectangle(1, 49, 4, 17), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            for (int i = 0; i < barLength / 2; i++)
            {
                //middlebackground
                spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 4 + i * 2, cameraPos.Y + position.Y), new Rectangle(6, 49, 2, 17), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);         
            }
            //right end
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 4 + barLength, cameraPos.Y + position.Y), new Rectangle(9, 49, 4, 17), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);

            

        }
    }
}
