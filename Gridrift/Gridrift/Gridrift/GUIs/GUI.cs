using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    class GUI
    {
        GUIObject backGround;
        bool backGroundShown;

        List<GUIObject> updatedObjects;
        List<GUIObject> staticObjects;

        public GUI(bool backGroundShown)
        {
            this.backGroundShown = backGroundShown;
            updatedObjects = new List<GUIObject>();
            staticObjects = new List<GUIObject>();
        }

        public void update(GameTime gameTime)
        {
            foreach (GUIObject e in updatedObjects)
            {
                e.update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (backGroundShown)
            {
                backGround.draw(spriteBatch);
            }
            foreach (GUIObject e in updatedObjects)
            {
                e.draw(spriteBatch);
            }

            foreach (GUIObject e in staticObjects)
            {
                e.draw(spriteBatch);
            }

        }

        public void addUpdateObject(GUIObject newObject)
        {
            updatedObjects.Add(newObject);
        }

        public void addStaticObject(GUIObject newObject)
        {
            staticObjects.Add(newObject);
        }

        public void setBackGround(GUIObject newBackGround)
        {
            backGround = newBackGround;
        }


    }
}
