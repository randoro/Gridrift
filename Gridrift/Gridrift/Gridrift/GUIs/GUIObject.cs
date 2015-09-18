using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    abstract class GUIObject
    {
        protected Vector2 position { set; get; }
        protected float layerDepth;
        protected Rectangle surfaceArea;

        public abstract void update(GameTime gameTime);

        public abstract void draw(SpriteBatch spriteBatch);

    }
}
