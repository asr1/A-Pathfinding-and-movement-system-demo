using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pathfinding_Demo.Engine
{
    public static class MouseCursor
    {
        /// <summary>
        /// Mouse position.
        /// </summary>
        public static Vector2 Position;

        /// <summary>
        /// Mouse collision rectangle size.
        /// </summary>
        public static Vector2 Size;

        /// <summary>
        /// Mouse collision rectangle.
        /// </summary>
        public static Rectangle rectangle;

        /// <summary>
        /// Previous mouse button action state.
        /// </summary>
        public static MouseState LastMouseState;

        /// <summary>
        /// Cureent mouse button action state.
        /// </summary>
        public static MouseState CurrentMouseState;

        /// <summary>
        /// Update the mouse cursor position.
        /// </summary>
        public static void Update()
        {
            LastMouseState = CurrentMouseState;

            CurrentMouseState = Mouse.GetState();

            Position = new Vector2((int)CurrentMouseState.X, (int)CurrentMouseState.Y);

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}