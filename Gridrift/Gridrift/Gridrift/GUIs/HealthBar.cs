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
    class HealthBar : ValueBar
    {
        public HealthBar(Vector2 position, int totalLength, float layerDepth)
            : base(position, totalLength, layerDepth)
        {

        }

        public override void update(GameTime gameTime)
        {
            percentValue = Player.healthPercent;

            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 cameraPos = Camera.cameraPosition();

            for (int i = 0; i < valueToPaint; i++)
            {
                //valuepainted
                spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + position.X + 4 + i, cameraPos.Y + position.Y + 4), new Rectangle(14, 49, 2, 9), new Color(1.0f - ((float)valueToPaint / (float)barLength), ((float)valueToPaint / (float)barLength), 0f), 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth + 0.001f);
            }

            base.draw(spriteBatch);
        }
    }
}
