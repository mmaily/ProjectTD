﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Screens;
using DowerTefenseGame.Managers;

namespace DowerTefenseGame
{

    /// <summary>
    /// Classe principale du jeu
    /// </summary>
    public class DowerTefense : Game
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
            // Initialisation du gestionnaire d'écrans
            ScreenManager.GetInstance();
            // Initialisation du gestionnaire de contenu
            CustomContentManager.GetInstance();
        }

        /// <summary>
        /// Initialisation des composants
        /// </summary>
        protected override void Initialize()
        {
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
            CustomContentManager.GetInstance().LoadTextures(Content);

            // Réglage de la taille de l'écran selon la carte
            graphics.PreferredBackBufferHeight = (MapManager.GetInstance().GetMap().mapHeight) * MapManager.GetInstance().GetMap().tileSize;
            graphics.PreferredBackBufferWidth = (MapManager.GetInstance().GetMap().mapWidth+4) * MapManager.GetInstance().GetMap().tileSize;
            graphics.ApplyChanges();

            //selectionne le GameScreen (PROVISOIRE)
            ScreenManager.GetInstance().SelectScreen(0);
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
            ScreenManager.GetInstance().Update(gameTime);
        }


        /// <summary>
        /// Mise à jour de l'affichage
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Récupération du ScreenManager
            ScreenManager screenManager = ScreenManager.GetInstance();

            // Nettoyage de l'écran
            GraphicsDevice.Clear(Color.Black);

            // Début de l'affichage
            spriteBatch.Begin();

            // Affichage de l'écran en cours
            screenManager.Draw(spriteBatch);

            // Fin de l'affichage
            spriteBatch.End();

            // Affichage de la base
            base.Draw(gameTime);
        }
    }
    
}
