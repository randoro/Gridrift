using Gridrift.Rendering;
using Microsoft.Xna.Framework;
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
    }
}
