using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUI
{
    class Menu
    {
        List<MenuObject> updatedObjects;
        List<MenuObject> staticObjects;

        public Menu()
        {
            updatedObjects = new List<MenuObject>();
            staticObjects = new List<MenuObject>();
        }

        public void update(GameTime gameTime)
        {
            foreach (MenuObject e in updatedObjects)
            {
                e.update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (MenuObject e in updatedObjects)
            {
                e.draw(spriteBatch);
            }

            foreach (MenuObject e in staticObjects)
            {
                e.draw(spriteBatch);
            }

        }
    }
}
