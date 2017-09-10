using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.Screens;
using Game1.Managers;
using Game1.GameElements;

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

        // Carte actuelle
        private Map map;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenManager.GetInstance();
            CustomContentManager.GetInstance();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            //Use the Content_manager to load all the content, the different "Screens" can then acces them
            CustomContentManager.GetInstance().LoadTextures(Content);

            // Chargement de la carte
            map = new Map();


            // Réglage de la taille de l'écran selon la carte
            graphics.PreferredBackBufferHeight = map.mapHeight * map.tileSize;
            graphics.PreferredBackBufferWidth = map.mapWidth * map.tileSize;
            graphics.ApplyChanges();
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

            // Début de l'affichage
            spriteBatch.Begin();

            // Affichage de la carte
            map.Draw(spriteBatch, CustomContentManager.GetInstance());

            //ScreenManager.GetInstance().Draw(spriteBatch, unitPosition, Color.White);

            // Fin de l'affichage
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
