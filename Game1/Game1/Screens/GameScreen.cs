using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Managers;
using DowerTefenseGame.GameElements;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;
using DowerTefenseGame.Players;
using System;

namespace DowerTefenseGame.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        // Carte en cours
        private Map map;
        // Variables liées aux vagues
        private double lastWaveTick;
        private int waveCount;
        public static int waveLength = 10000;
        public double millisecPerFrame=1000;
        public double time;
        //Joueur (défenseur pour l'instant)
        public DefensePlayer Player;

        /// <summary>
        /// Constructeur principal
        /// </summary>
        public GameScreen()
        {
            Player = new DefensePlayer();

            // Init des vagues
            lastWaveTick = 0;
            waveCount = 0;
        }
        
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            // Init de l'UI
            UIManager.GetInstance().Initialize();
            Graphics.PreferredBackBufferHeight = (MapManager.GetInstance().CurrentMap.mapHeight) * MapManager.GetInstance().CurrentMap.tileSize;
            Graphics.PreferredBackBufferWidth = (MapManager.GetInstance().CurrentMap.mapWidth + 4) * MapManager.GetInstance().CurrentMap.tileSize;
            Graphics.ApplyChanges();
        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void LoadContent()
        {
            // Chargement du gestionnaire de carte
            MapManager mapManager = MapManager.GetInstance();
            //Récupération de la carte en cours
            map = mapManager.CurrentMap;
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {

            millisecPerFrame = _gameTime.TotalGameTime.TotalMilliseconds - time;

            time = _gameTime.TotalGameTime.TotalMilliseconds;
            #region === Calcul des vagues ===

            // Calcul du cycle de 30 secondes
            bool newWave = false;
            // Durée depuis ancien tic
            int timeSince = (int)(_gameTime.TotalGameTime.TotalMilliseconds - lastWaveTick);
            // Si le tic est vieux de 30 secondes
            if(timeSince > waveLength)
            {
                // Vague suivante
                waveCount++;
                // Sauvegarde horodatage
                lastWaveTick = _gameTime.TotalGameTime.TotalMilliseconds;
                // Nouvelle vague
                newWave = true;
            }
            #endregion

            // Mise à jour du gestionnaire de carte
            MapManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire d'unités
            UnitsManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire de bâtiments
            BuildingsManager.GetInstance().Update(_gameTime);
            //Mise à jour des tâche de l'IA
            AiManager.GetInstance().Update(_gameTime, newWave);
            // Mise à jour du gestionnaire d'interface
            UIManager.GetInstance().Update(_gameTime, newWave, timeSince);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (millisecPerFrame != 0)
            {
                int offset = 340;
                _spriteBatch.DrawString(CustomContentManager.GetInstance().Fonts["font"], Math.Ceiling(1000 / (millisecPerFrame)).ToString(), new Vector2(UIManager.GetInstance().leftUIOffset, offset), Color.White);
            }
           
            SpriteBatch spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            spriteBatch.Begin();
            // Affichage de la carte
            MapManager.GetInstance().Draw(spriteBatch);
            // Affichage des bâtiments
            BuildingsManager.GetInstance().Draw(spriteBatch);
            // Affichage des unités
            UnitsManager.GetInstance().Draw(spriteBatch);
            // Affichage de l'interface
            UIManager.GetInstance().Draw(spriteBatch);

            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.GetInstance().Textures["cursor"];
            spriteBatch.Draw(fap, lol, Color.White);
            spriteBatch.End();
            
        }
    }
        

}
