using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LibrairieTropBien.Network.Game;
using DownerTefense.Game.Screens;

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
        //Si on veut passer un écran en arrière plan (il se dessine juste mais n'agit plus, pas d'update)
        static Screen backGroundScreen;
        //Dictionnaire des Screen
        public static Dictionary<ScreenEnum, Screen> Screens;
        public enum ScreenEnum
        {
            MenuScreens, //Screen de menu principal
            GameScreen, // Screen du jeu
            LoseScreen, // Screen de perdu
            WinScreen, // Screen de gagné
            Editor, // Editeur
            Lobby,//Scrren de lobby pour matchmaking
        }
        public static void Initialize()
        {
            //Initialisation des Screens
            // Instance du gestionnaire d'écran
            //Dictionnaire des Screen
            Screens = new Dictionary<ScreenEnum, Screen>
            {
                { ScreenEnum.MenuScreens, new MenuScreen() },
                { ScreenEnum.GameScreen, new GameScreen() },
                { ScreenEnum.LoseScreen, new LoseScreen() },
                { ScreenEnum.WinScreen, new WinScreen() },
                { ScreenEnum.Editor, new Editor() },
                { ScreenEnum.Lobby, new LobbyScreen() }
            };
            currentScreen = (Screen)Screens[ScreenEnum.GameScreen];
        }
        /// <summary>
        /// Chargement d'un écran spécifique (temporaire)
        /// </summary>
        /// <param name="_id">Identifiant de l'écran à charger</param>
        public static void SelectScreen(ScreenEnum type)
        {
            loadingScreen = (Screen)Screens[type];
            loadingScreen.Initialize(Graphics);
            LoadContent();
            currentScreen = loadingScreen;
        }
        public static void SetBackGroundScreen(ScreenEnum type)
        {
                backGroundScreen = (Screen)Screens[type];
        }
        public static void UnsetBackGroundScreen()
        {
            backGroundScreen = null;
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
            if (backGroundScreen != null)
            {
                backGroundScreen.Draw(spriteBatch);
            }
            currentScreen.Draw(spriteBatch);

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


        public static void UpdateGameScreenMode(bool _vsAI, PlayerRole _role)
        {
            ((GameScreen)Screens[ScreenEnum.GameScreen]).VsAI = _vsAI;
            //TODO : Enelver ce mode de débug et mettre défenseur

            ((GameScreen)Screens[ScreenEnum.GameScreen]).role = _role;

        }
    }   

}
