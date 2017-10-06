using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefense.Game.Screens;
using System.Collections.Generic;
using LibrairieTropBien.Network.Game;

namespace DowerTefense.Game.Screens
{

    /// <summary>
    /// Gestionnaire d'écran
    /// </summary>
    public static class ScreenManager
    {

        /// <summary>
        /// Initialisation du contenu
        /// </summary>
        static GraphicsDeviceManager Graphics;
        // Écran courant
        static Screen currentScreen;
        //Ecran à charger
        static Screen loadingScreen;
        //Dictionnaire des Screen
        static Dictionary<String, Screen> Screens;
        public static void Initialize()
        {
            // Init de l'écran
            //loadingScreen.Initialize(Graphics);
            //Initialisation des Screens
            // Instance du gestionnaire d'écran
        //Dictionnaire des Screen
        Dictionary<String, Screen> Screens;
        Screens = new Dictionary<String, Screen>();
            Screens.Add("EntranceScreen", new EntranceScreen());
            Screens.Add("GameScreen", new GameScreen());
            Screens.Add("Editor", new Editor());
            Screens.Add("Lobby", new LobbyScreen());
            currentScreen = (Screen)Screens["EntranceScreen"];
        }
        /// <summary>
        /// Chargement d'un écran spécifique (temporaire)
        /// </summary>
        /// <param name="_id">Identifiant de l'écran à charger</param>
        public static void SelectScreen(String name)
        {
            loadingScreen = (Screen)Screens[name];
            Initialize();
            LoadContent();
            currentScreen = loadingScreen;
        }

        /// <summary>
        /// Chargement du contenu de l'écran
        /// </summary>
        public static void LoadContent()
        {
            // On charge le contenu de l'écran actuel
           loadingScreen.LoadContent();
        }

        /// <summary>
        /// Affichage de l'écran actuel
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        /// <summary>
        /// Affichage de l'écran actuel
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="pos"></param>
        /// <param name="col"></param>
        public static void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col)
        {
            currentScreen.Draw(spriteBatch,pos,col);
        }

        /// <summary>
        /// Mise à jour de l'écran actuel
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public static void SetGraphics(GraphicsDeviceManager _graphics)
        {
            Graphics = _graphics;
        }


        public static void UpdateGameScreenMode(bool _vsAI)
        {
            ((GameScreen)Screens["GameScreen"]).VsAI = _vsAI;
            //TODO : Enelver ce mode de débug et mettre défenseur
            ((GameScreen)Screens["GameScreen"]).role = PlayerRole.Debug;
        }
    }   

}
