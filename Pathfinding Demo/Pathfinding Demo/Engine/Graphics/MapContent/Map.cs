using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Pathfinding_Demo.Engine.IO;

namespace Pathfinding_Demo.Engine.MapContent.Graphics
{
    class Map
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        public string MapFilePath;

        MapReader maploader;
        public TileBank tileBank;

        public List<Layer> Layers;

        public List<Rectangle> AStarCollisionObjects;
        public List<Rectangle> CollisionObjects;

        public Vector2 ArraySize;
        public Vector2 Size;
        public string Name;

        public enum RenderMode { SingleTile, LargeTexture }
        internal RenderMode renderMode;

        public Map()
        {
            maploader = new MapReader(this);
            tileBank = new TileBank();
            Layers = new List<Layer>();

            AStarCollisionObjects = new List<Rectangle>();
            CollisionObjects = new List<Rectangle>();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
            maploader.Initialize(Graphics);
        }

        public void LoadContent(string MapFilePath)
        {
            MapFilePath = MapFilePath.Replace('\\', '/');
            this.MapFilePath = MapFilePath;

            maploader.LoadContent(MapFilePath);
            tileBank.LoadContent();

            AStarCollisionObjects = CollisionObjects;
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Initialize(Graphics);
                Layers[i].UpdateOnce(spriteBatch);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Layers.Count; i++)
                Layers[i].Draw(Color.White);
        }
    }
}