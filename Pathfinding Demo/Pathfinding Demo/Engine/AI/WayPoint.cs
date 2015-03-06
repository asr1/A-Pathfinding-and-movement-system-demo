using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Pathfinding_Demo.Engine.AI
{
    class WayPoint
    {
        public int WayPointIndex;
        public bool ReachedDestination;

        public void MoveTo(GameTime gameTime, Unit unit, List<Vector2> DestinationWaypoint, float Speed)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    float Distance = Vector2.Distance(unit.Position, DestinationWaypoint[WayPointIndex]);
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - unit.Position;
                    Direction.Normalize();

                    if (Distance > Direction.Length())
                        unit.Position += Direction * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            unit.Position += Direction;
                            ReachedDestination = true;
                        }
                        else
                            WayPointIndex++;
                    }
                }
            }
        }
    }
}