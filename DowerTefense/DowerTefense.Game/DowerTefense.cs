using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefense.Game.Screens;
using DowerTefense.Game.Managers;

namespace DowerTefense.Game
{

    /// <summary>
    /// Classe principale du jeu
    /// </summary>
    public class DowerTefense : Microsoft.Xna.Framework.Game
    {
        // Gestionnaire des graphiques
        GraphicsDeviceManager graphics;
        // Gestionnaire des sprites
        SpriteBatch spriteBatch;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DowerTefense()
        {
            // Initialisation des graphiques
            graphics = new GraphicsDeviceManager(this);
            // Dossier racine du contenu
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initialisation des composants
        /// </summary>
        protected override void Initialize()
        {
            //Initialisation du content
            CustomContentManager.Initialize();
            //Envoie le graphic au screenManager
            ScreenManager.Initialize();
            ScreenManager.SetGraphics(this.graphics);

            // Initialisation des composants
            base.Initialize();



        }

        /// <summary>
        /// Chargement des contenus
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Use the Content_manager to load all the content, the different "Screens" can then acces them
            CustomContentManager.LoadTextures(Content,graphics.GraphicsDevice);

            // Réglage de la taille de l'écran
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();

            ////Demande l'affichage du premier écran
            ScreenManager.SelectScreen("MenuScreen");


        }

        /// <summary>
        /// Déchargement des contenus
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Mise à jour du temps de jeu
            base.Update(gameTime);

            // Mise à jour de l'écran actif
            ScreenManager.Update(gameTime);

        }


        /// <summary>
        /// Mise à jour de l'affichage
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // Nettoyage de l'écran
            GraphicsDevice.Clear(Color.Black);

            // Début de l'affichage
            spriteBatch.Begin();

            // Affichage de l'écran en cours
            ScreenManager.Draw(spriteBatch);

            // Fin de l'affichage
            spriteBatch.End();

            // Affichage de la base
            base.Draw(gameTime);
        }
    }
    
}
