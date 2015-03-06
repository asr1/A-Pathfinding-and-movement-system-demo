using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Pathfinding_Demo.Engine.MapContent.Graphics;
using Pathfinding_Demo.Engine.UI;

namespace Pathfinding_Demo
{
    class UnitManager
    {
        public List<Unit> Units;

        public UnitManager()
        {
            Units = new List<Unit>();
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].Initialize(graphics);
        }

        public void LoadContent()
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].LoadContent();
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].UpdateOnce(spriteBatch);
        }

        public void Update(GameTime gameTime, MouseRectangle mouseRectangle, Map map)
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].Update(gameTime, mouseRectangle, map, i, Units);
        }

        public void Draw()
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].Draw();
        }
    }
}