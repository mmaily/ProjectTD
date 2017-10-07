using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefense.Game.Managers;
using Microsoft.Xna.Framework.Input;
using DowerTefense.Game.Players;
using System;
using LibrairieTropBien.Network.Game;
using DowerTefense.Game.Multiplayer;
using LibrairieTropBien.Network;
using DowerTefense.Commons.GameElements;
using DowerTefense.Commons;

namespace DowerTefense.Game.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        #region===Jeu===
        private GameEngine game;
        private Map map;
        #endregion
        //Role adopté par ce GameScreen
        public PlayerRole role = PlayerRole.Debug;

        //Faire jouer l'AI
        private Boolean vsAI = false;
        public bool VsAI { get => vsAI; set => vsAI = value; }

        // Chargement terminé (réception de toutes les infos joueurs)
        private bool loaded = false;

        /// <summary>
        /// Constructeur principal
        /// </summary>
        public GameScreen()
        {
            defenseplayer = new DefensePlayer();


        }
        
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            #region Initialisation du jeu et des variables associée===
            game = new GameEngine();
            map = game.map;
            #endregion

            //Récupération de l'écran et instancition du spriteBatch
            this.Graphics = _graphics;
            spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            // Init de l'UI
            //Graphics.PreferredBackBufferHeight = (MapManager.GetInstance().CurrentMap.mapHeight) * MapManager.GetInstance().CurrentMap.tileSize+topMargin*2;
            //Graphics.PreferredBackBufferWidth = (MapManager.GetInstance().CurrentMap.mapWidth ) * MapManager.GetInstance().CurrentMap.tileSize+UIManager.GetInstance().zoneUi.Width+leftMargin*2;
            //Graphics.ApplyChanges();
            UIManager.GetInstance().SetRole(role);
            UIManager.GetInstance().Initialize(_graphics);

            // Abonnement aux mises à jour du jeu
            MultiplayerManager.GameUpdate += this.GameUpdate;
        }

        /// <summary>
        /// Traitement d'un message reçu pour le jeu
        /// </summary>
        /// <param name="message"></param>
        private void GameUpdate(Message message)
        {
            switch (message.Subject)
            {
                case "towerUpdate":

                    BuildingsManager.GetInstance().WaitingForConstruction.Add((Tower)message.received);
                    break;
                case "spawnerUpdate":

                    BuildingsManager.GetInstance().WaitingForConstruction.Add((SpawnerBuilding)message.received);
                    break;
            }
        }



        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void LoadContent()
        {

            map = mapManager.CurrentMap;
            loaded = true;
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            // Si le jeu n'est pas encore chargé
            if (!loaded)
            {
                return;
            }
            //Update du jeu en interne
            game.Update(_gameTime);

            millisecPerFrame = _gameTime.TotalGameTime.TotalMilliseconds - time;

            time = _gameTime.TotalGameTime.TotalMilliseconds;


            // Mise à jour du gestionnaire de carte
            MapManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire d'unités
            UnitsManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire de bâtiments
            BuildingsManager.GetInstance().Update(_gameTime);
            if (VsAI) { 
            //Mise à jour des tâche de l'IA
            AiManager.GetInstance().Update(_gameTime, newWave);
            }
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
           
            spriteBatch.Begin();

            // Si le jeu n'est pas encore chargé
            if (!loaded)
            {
                _spriteBatch.DrawString(CustomContentManager.GetInstance().Fonts["font"], "Chargement...", new Vector2(50, 50), Color.White);
            }

            //spriteBatch.Draw(CustomContentManager.GetInstance().Colors["grey"],Graphics.);
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
