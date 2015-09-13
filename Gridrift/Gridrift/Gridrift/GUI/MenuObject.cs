using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUI
{
    abstract class MenuObject
    {
        public extern void update(GameTime gameTime);

        public abstract void draw(SpriteBatch spriteBatch);

    }
}
