using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    abstract class Button : GUIObject
    {
        //action

        public void doClickAction()
        {
            Game1.debuggingActive = !Game1.debuggingActive;
            Console.WriteLine("Button Clicked");
        }
    }
}
