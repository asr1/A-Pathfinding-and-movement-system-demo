using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Pathfinding_Demo.Engine.MapContent.Graphics
{
    /// <summary>
    /// This class is responsible for storing all the tile sheets.
    /// </summary>
    class TileSheet
    {
        /// <summary>
        /// Tile sheet texture.
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// Tile sheet path or location on the hard drive.
        /// </summary>
        public string Path;

        /// <summary>
        /// TileSheet constructor that stors the texture and path.
        /// </summary>
        /// <param name="Texture">Tile sheet texture.</param>
        /// <param name="Path">Tile sheet path in the hard drive.</param>
        public TileSheet(Texture2D Texture, string Path)
        {
            this.Texture = Texture;
            this.Path = Path;
        }
    }
}