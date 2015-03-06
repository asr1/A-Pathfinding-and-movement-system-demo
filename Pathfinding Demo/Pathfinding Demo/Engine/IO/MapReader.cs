using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;

using Pathfinding_Demo.Engine.MapContent.Graphics;

namespace Pathfinding_Demo.Engine.IO
{
    class MapReader
    {
        GraphicsDeviceManager Graphics;

        Map map;

        public MapReader(Map map)
        {
            this.map = map;
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        /// <summary>
        /// Load from the Content folder.
        /// </summary>
        /// <param name="MapFilePath"></param>
        public void LoadContent(string MapFilePath)
        {
            MapFilePath = "./Content/" + MapFilePath;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MapFilePath);

            LoadMapData(xmlDoc);
            LoadCollitionLayer(xmlDoc);
            LoadLayers(xmlDoc);
            LoadTileSheets(xmlDoc, false);
        }

        void LoadMapData(XmlDocument xmlDoc)
        {
            map.Name = xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("Name").Value.ToString();

            map.tileBank.TileSize.X = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("TileWidth").Value);
            map.tileBank.TileSize.Y = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("TileHight").Value);

            map.ArraySize.X = xmlDoc.SelectSingleNode("MapData/Layers/Layer").ChildNodes.Item(0).InnerText.Split(',').Length;
            map.ArraySize.Y = xmlDoc.SelectSingleNode("MapData/Layers/Layer").ChildNodes.Count;

            if (xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("RenderMode").Value == "SingleTile")
                map.renderMode = Map.RenderMode.SingleTile;
            else if (xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("RenderMode").Value == "LargeTexture")
                map.renderMode = Map.RenderMode.LargeTexture;

            map.Size = map.ArraySize * map.tileBank.TileSize;
        }

        void LoadCollitionLayer(XmlDocument xmlDoc)
        {
            int[,] TempArray = new int[(int)map.ArraySize.X, (int)map.ArraySize.Y];
            int y = 0;

            foreach (XmlNode node in xmlDoc.SelectNodes("MapData/Layers/CollisionLayer").Item(0).ChildNodes)
            {
                string[] temp = node.InnerText.Split(',');

                for (int x = 0; x < temp.Length; x++)
                {
                    TempArray[x, y] = int.Parse(temp[x]);

                    if (TempArray[x, y] == 1)
                        map.CollisionObjects.Add(new Rectangle(x, y, (int)map.tileBank.TileSize.X, (int)map.tileBank.TileSize.Y));
                }
                y++;
            }
        }

        void LoadLayers(XmlDocument xmlDoc)
        {
            int LayerIndex = 0;

            foreach (XmlNode node in xmlDoc.SelectNodes("MapData/Layers/Layer"))
            {
                map.Layers.Add(new Layer(map));

                int y = 0;

                foreach (XmlNode InnerNode in node.ChildNodes)
                {
                    string[] temp = InnerNode.InnerText.Split(',');

                    for (int x = 0; x < temp.Length; x++)
                        map.Layers[LayerIndex].Array[x, y] = int.Parse(temp[x]);

                    y++;
                }
                LayerIndex++;
            }
        }

        /// <summary>
        /// Load map textures from XML file like tile path, size, ID and location.
        /// </summary>
        void LoadTileSheets(XmlDocument xmlDoc, bool LoadFromArchive)
        {
            foreach (XmlNode node in xmlDoc.SelectNodes("MapData/TileSheets").Item(0).ChildNodes)
            {
                string TileSheetPath = node.Attributes.GetNamedItem("Path").Value;

                try { map.tileBank.TileSheets.Add(new TileSheet(StreamTexture.LoadTextureFromStream(Graphics, TileSheetPath), TileSheetPath)); }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}