using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Screens;
using System.Collections.Generic;

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
        // Écran courant
        Screen currentScreen;
        //Dictionnaire des Screen
        public Dictionary<String, Screen> Screens;

        /// <summary>
        /// Constructeur du gestionnaire d'écrans
        /// </summary>
        private ScreenManager()
        {
            Screens = new Dictionary<String, Screen>();
            Screens.Add("EntranceScreen", new EntranceScreen());
            Screens.Add("GameScreen", new GameScreen());
            Screens.Add("Editor", new Editor());
            Screens.Add("Lobby", new LobbyScreen());
            currentScreen = (Screen)Screens["EntranceScreen"];
            

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
        public void SelectScreen(String _screenName)
        {
            currentScreen = (Screen)Screens[_screenName];
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


        public void UpdateGameScreenMode(bool _vsAI)
        {
            ((GameScreen)Screens["GameScreen"]).VsAI = _vsAI;
        }
    }   

}
