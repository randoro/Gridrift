﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUI
{
    abstract class Button : MenuObject
    {

        public abstract void update(GameTime gameTime);
    }
}
