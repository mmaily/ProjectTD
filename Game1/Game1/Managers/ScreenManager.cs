﻿using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Screens;

namespace DowerTefenseGame.Screens
{

    /// <summary>
    /// Gestionnaire d'écran
    /// </summary>
    class ScreenManager
    {
        
        // Instance du gestionnaire d'écran
        private static ScreenManager instance=null;
        private GraphicsDeviceManager Graphics;
        // Liste des différents écrans
        ArrayList Screens;
        // Écran courant
        Screen currentScreen;

        /// <summary>
        /// Constructeur du gestionnaire d'écrans
        /// </summary>
        private ScreenManager()
        {
            Screens = new ArrayList();
            Screens.Add(new EntranceScreen());
            Screens.Add(new GameScreen());
            Screens.Add(new Editor());
            currentScreen = (Screen)Screens[1];

        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'écran
        /// </summary>
        /// <returns></returns>
        public static ScreenManager GetInstance()
        {
            if (instance == null)
            {

            instance = new ScreenManager();
           
            }
            return instance;
        }

        /// <summary>
        /// Chargement d'un écran spécifique (temporaire)
        /// </summary>
        /// <param name="_id">Identifiant de l'écran à charger</param>
        public void SelectScreen(int _id)
        {
            currentScreen = (Screen)Screens[_id];
            Initialize();
            LoadContent();

        }

        /// <summary>
        /// Initialisation du contenu
        /// </summary>
        private void Initialize()
        {
            // Init de l'écran
            currentScreen.Initialize(Graphics);
        }

        /// <summary>
        /// Chargement du contenu de l'écran
        /// </summary>
        public virtual void LoadContent()
        {
            // On charge le contenu de l'écran actuel
            currentScreen.LoadContent();
        }

        /// <summary>
        /// Affichage de l'écran actuel
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        /// <summary>
        /// Affichage de l'écran actuel
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="pos"></param>
        /// <param name="col"></param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col)
        {
            currentScreen.Draw(spriteBatch,pos,col);
        }

        /// <summary>
        /// Mise à jour de l'écran actuel
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }
        public void SetGraphics(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
        }
    }   

}
