using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.GUIs
{
    class ToolBar : GUI
    {
        public ToolBar()
            : base(false)
        {
            addUpdateObject(new IconButton(new Vector2(220, 220), 0.9f));
            addUpdateObject(new HealthBar(new Vector2(200, 200), 400, 0.9f));
            
        }



    }
}
