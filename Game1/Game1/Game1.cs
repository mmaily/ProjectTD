using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.Screens;
using Game1.Managers;

namespace Game1
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 unitPosition;
        Vector2 unitSpeed;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenManager.GetInstance();
            Content_Manager.GetInstance();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() =>
            // TODO: Add your initialization logic here

            base.Initialize();

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Use the Content_manager to load all the content, the different "Screens" can then acces them
            Content_Manager.GetInstance().LoadTextures(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keystate = Keyboard.GetState();
            // TODO: Add your update logic here
            unitSpeed = Vector2.Zero;
            if (keystate.IsKeyDown(Keys.Up)){
                unitSpeed.Y += -1;
            }
            if (keystate.IsKeyDown(Keys.Down))
            {
                unitSpeed.Y += 1;
            }
            if (keystate.IsKeyDown(Keys.Left))
            {
                unitSpeed.X += -1;
            }
            if (keystate.IsKeyDown(Keys.Right))
            {
                unitSpeed.X += 1;
            }

            base.Update(gameTime);
            if (unitSpeed != Vector2.Zero) { unitSpeed.Normalize(); }
            unitPosition += unitSpeed;
            
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(Content_Manager.GetInstance().Textures["unit"], unitPosition, Color.White);
            ScreenManager.GetInstance().Draw(spriteBatch, unitPosition, Color.White);
            //  spriteBatch.Draw(unit, unitPosition, Color.White);
            // spriteBatch.DrawString(font,unitPosition.ToString(), Vector2.Zero, Color.YellowGreen);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
