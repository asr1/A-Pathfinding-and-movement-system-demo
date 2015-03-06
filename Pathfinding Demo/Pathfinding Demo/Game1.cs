using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pathfinding_Demo.Engine;
using Pathfinding_Demo.Engine.MapContent.Graphics;
using Pathfinding_Demo.Engine.UI;

namespace Pathfinding_Demo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map map;
        Camera camera;

        MouseRectangle mouseRectangle;
        UnitManager unitManager;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            map = new Map();
            camera = new Camera();

            mouseRectangle = new MouseRectangle();
            unitManager = new UnitManager();

            unitManager.Units.Add(new Unit(new Vector2(0, 0)));
            unitManager.Units.Add(new Unit(new Vector2(0, 32)));
            unitManager.Units.Add(new Unit(new Vector2(0, 64)));
            unitManager.Units.Add(new Unit(new Vector2(0, 96)));
            unitManager.Units.Add(new Unit(new Vector2(0, 128)));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            //this.IsFixedTimeStep = false;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            map.Initialize(graphics);
            camera.Initilize(graphics.GraphicsDevice.Viewport);

            mouseRectangle.Initialize(graphics);
            unitManager.Initialize(graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map.LoadContent("Map.map");
            map.UpdateOnce(spriteBatch);

            mouseRectangle.LoadContent();
            mouseRectangle.UpdateOnce(spriteBatch);

            unitManager.LoadContent();
            unitManager.UpdateOnce(spriteBatch);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseCursor.Update();

            camera.Update(gameTime, map);

            mouseRectangle.Update(gameTime);
            unitManager.Update(gameTime, mouseRectangle, map);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.Transformation);
            map.Draw();
            mouseRectangle.Draw();
            unitManager.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
