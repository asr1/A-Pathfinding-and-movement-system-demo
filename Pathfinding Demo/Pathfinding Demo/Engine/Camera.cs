using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Pathfinding_Demo.Engine.MapContent.Graphics;

namespace Pathfinding_Demo.Engine
{
    /// <summary>
    /// Camera class is responsible for moving, rotating and zooming the seen, using the transformation matrix.
    /// </summary>
    class Camera
    {
        #region Camera class variables.
        Viewport viewPort;

        /// <summary>
        /// Camera transformation matrix.
        /// </summary>
        public Matrix Transformation;

        /// <summary>
        /// Camera position and size.
        /// </summary>
        public static Rectangle rectangle;

        /// <summary>
        /// Position from top left corner of the camera.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Center of the camera.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Rotation of the camera.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Zoom in and out of the camera.
        /// </summary>
        public float Zoom;
        #endregion

        /// <summary>
        /// Camera constructor.
        /// </summary>
        public Camera()
        {
            Rotation = 0f;
            Zoom = 1.0f;
        }

        /// <summary>
        /// Initilize the camera.
        /// </summary>
        /// <param name="viewPort">Graphic device viewport.</param>
        public void Initilize(Viewport viewPort)
        {
            this.viewPort = viewPort;
            Center = new Vector2((int)(viewPort.Width / 2), (int)(viewPort.Height / 2));
        }

        /// <summary>
        /// Setting the minimum and maximmum distance the camera can move in any given direction.
        /// </summary>
        /// <param name="Min">The minimum distance the camera can move in any given direction.</param>
        /// <param name="Max">The maximum distance the camera can move in any given direction.</param>
        public void SetBounds(Vector2 Min, Vector2 Max)
        {
            if (Position.X > Max.X)
                Position = new Vector2((int)Max.X, (int)Position.Y);

            if (Position.X < Min.X)
                Position = new Vector2((int)Min.X, (int)Position.Y);


            if (Position.Y > Max.Y)
                Position = new Vector2((int)Position.X, (int)Max.Y);

            if (Position.Y < Min.Y)
                Position = new Vector2((int)Position.X, (int)Min.Y);
        }

        /// <summary>
        /// Setting how far can the camrea zoom in or out. 
        /// </summary>
        /// <param name="Min">The minimum distance the camera can zoom in.</param>
        /// <param name="Max">The maximmum distance the camera can zoom out.</param>
        public void SetZoomBounds(float Min, float Max)
        {
            if (Zoom < Min)
                Zoom = Min;

            if (Zoom > Max)
                Zoom = Max;
        }

        /// <summary>
        /// Calculate the camrea size (width and height) after applying zoom.
        /// </summary>
        /// <param name="map">Map class.</param>
        /// <returns></returns>
        Vector2 CalculateSize(Map map)
        {
            Vector2 TileSizeAfterZoom = new Vector2((float)Math.Ceiling(map.tileBank.TileSize.X * Zoom), (float)Math.Ceiling(map.tileBank.TileSize.Y * Zoom));

            Vector2 NumberOfNewTileSizeToFitViewport = new Vector2((float)Math.Ceiling(viewPort.Width / TileSizeAfterZoom.X), (float)Math.Ceiling(viewPort.Height / TileSizeAfterZoom.Y));

            Vector2 CameraSize = new Vector2(NumberOfNewTileSizeToFitViewport.X * map.tileBank.TileSize.X, NumberOfNewTileSizeToFitViewport.Y * map.tileBank.TileSize.Y);

            return CameraSize;
        }

        /// <summary>
        /// Update the camera position, size, bounds and zoom.
        /// </summary>
        /// <param name="gameTime">MonoGame GameTime.</param>
        /// <param name="map">Map class.</param>
        public void Update(GameTime gameTime, Map map)
        {
            Vector2 CameraSize = CalculateSize(map);

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)CameraSize.X, (int)CameraSize.Y);

            SetBounds(new Vector2(0, 0), new Vector2(map.Size.X - viewPort.Width, map.Size.Y - viewPort.Height));
            SetZoomBounds(0.1f, 2);

            Transformation = Matrix.CreateTranslation(new Vector3((int)-Position.X, (int)-Position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom);
        }
    }
}
