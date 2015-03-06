using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pathfinding_Demo.Engine.AI;
using Pathfinding_Demo.Engine.IO;
using Pathfinding_Demo.Engine.UI;
using Pathfinding_Demo.Engine.MapContent.Graphics;
using Pathfinding_Demo.Engine;

namespace Pathfinding_Demo
{
    class Unit
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Vector2 Position, Size;
        Rectangle rectangle;
        float Speed;

        public enum State { Normal, Hovered, Selected }

        Texture2D CurrentTexture, NormalTexture, HoveredTexture, SelectedTexture;
        public State UnitState;

        AstarThreadWorker astarThreadWorkerTemp, astarThreadWorker;
        List<Vector2> WayPointsList;

        WayPoint wayPoint;

        public Unit(Vector2 Position)
        {
            this.Position = Position;
            Size = new Vector2(16, 16);
            Speed = 0.1f;

            WayPointsList = new List<Vector2>();

            wayPoint = new WayPoint();
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void LoadContent()
        {
            NormalTexture = StreamTexture.LoadTextureFromStream(graphics, "UnitNormal.png");
            HoveredTexture = StreamTexture.LoadTextureFromStream(graphics, "UnitHovered.png");
            SelectedTexture = StreamTexture.LoadTextureFromStream(graphics, "UnitSelected.png");
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            CurrentTexture = NormalTexture;
        }

        void Astar(GameTime gameTime, MouseRectangle mouseRectangle, Map map, int UnitID, List<Unit> Units)
        {
            if (MouseCursor.CurrentMouseState.RightButton == ButtonState.Pressed && MouseCursor.LastMouseState.RightButton == ButtonState.Released && UnitState == State.Selected)
            {
                astarThreadWorker = null;
                AstarManager.AddNewThreadWorker(new Node(new Vector2((int)Position.X / 16, (int)Position.Y / 16)),
                                            new Node(new Vector2((int)MouseCursor.CurrentMouseState.X / 16, (int)MouseCursor.CurrentMouseState.Y / 16)), map, false, UnitID);
            }

            AstarManager.AstarThreadWorkerResults.TryPeek(out astarThreadWorkerTemp);

            if (astarThreadWorkerTemp != null)
                if (astarThreadWorkerTemp.WorkerIDNumber == UnitID)
                {
                    AstarManager.AstarThreadWorkerResults.TryDequeue(out astarThreadWorker);

                    if (astarThreadWorker != null)
                    {
                        wayPoint = new WayPoint();

                        WayPointsList = astarThreadWorker.astar.GetFinalPath();

                        for (int i = 0; i < WayPointsList.Count; i++)
                            WayPointsList[i] = new Vector2(WayPointsList[i].X * 16, WayPointsList[i].Y * 16);
                    }
                }

            if (WayPointsList.Count > 0)
            {
                Avoidence(gameTime, Units, UnitID);
                wayPoint.MoveTo(gameTime, this, WayPointsList, Speed);
            }
        }

        void Avoidence(GameTime gameTime, List<Unit> Units, int UnitID)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i].rectangle.Intersects(rectangle))
                {
                    float Distance1 = Vector2.Distance(Position, WayPointsList[WayPointsList.Count - 1]);
                    float Distance2 = Vector2.Distance(Units[i].Position, WayPointsList[WayPointsList.Count - 1]);

                    if (Distance1 > Distance2)
                    {
                        Vector2 OppositeDirection = Units[i].Position - Position;
                        OppositeDirection.Normalize();
                        Position -= OppositeDirection * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
            }
        }

        public void Update(GameTime gameTime, MouseRectangle mouseRectangle, Map map, int UnitID, List<Unit> Units)
        {
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            if (mouseRectangle.rectangle.Intersects(rectangle))
                UnitState = State.Hovered;
            else if (UnitState != State.Selected)
                UnitState = State.Normal;

            if (!mouseRectangle.rectangle.Intersects(rectangle) && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && UnitState == State.Selected)
                UnitState = State.Normal;

            if (mouseRectangle.rectangle.Intersects(rectangle) && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Released && UnitState == State.Hovered)
                UnitState = State.Selected;

            if (UnitState == State.Normal)
                CurrentTexture = NormalTexture;

            if (UnitState == State.Hovered)
                CurrentTexture = HoveredTexture;

            if (UnitState == State.Selected)
                CurrentTexture = SelectedTexture;

            Astar(gameTime, mouseRectangle, map, UnitID, Units);
            
        }

        public void Draw()
        {
            spriteBatch.Draw(CurrentTexture, Position, Color.White);
        }
    }
}