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
using System.Collections.Generic;
using DowerTefense.Game.Translator;

namespace DowerTefense.Game.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        #region Jeu
        private GameEngine game;
        private Map map;
        #endregion
        #region Liste des ordres Serveur
        private List<Message> orders;
        #endregion
        #region Interface Visuelle
        private float imageRatio;
        private Vector2 marginOffset;
        private UIManager uiManager;
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
        }
        
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            #region Initialise la liste des ordres du serveur
            orders = new List<Message>();
            #endregion
            #region Initialisation du jeu et des variables associée
            game = new GameEngine();
            game.Initialize();
            map = game.map;
            #endregion
            #region Calcul de l'échelle de scaling selon taille des Tile
            marginOffset = new Vector2(ScreenManager.Screens["GameScreen"].leftMargin, ScreenManager.Screens["GameScreen"].topMargin);
            #endregion
            //Récupération de l'écran et instancition du spriteBatch
            this.Graphics = _graphics;
            spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            // Init de l'UI
            //Graphics.PreferredBackBufferHeight = (MapManager.GetInstance().CurrentMap.mapHeight) * MapManager.GetInstance().CurrentMap.tileSize+topMargin*2;
            //Graphics.PreferredBackBufferWidth = (MapManager.GetInstance().CurrentMap.mapWidth ) * MapManager.GetInstance().CurrentMap.tileSize+UIManager.GetInstance().zoneUi.Width+leftMargin*2;
            //Graphics.ApplyChanges();
            uiManager = new UIManager(game);
            uiManager.SetRole(role);
            uiManager.Initialize(_graphics);

            // Abonnement aux mises à jour du jeu
            MultiplayerManager.GameUpdate += this.GameUpdate;
        }

        /// <summary>
        /// Traitement d'un message reçu pour le jeu
        /// </summary>
        /// <param name="message"></param>
        private void GameUpdate(Message message)
        {
            lock (orders)
            {
                orders.Add(message);
            }
            //switch (message.Subject)
            //{
            //    case "towerUpdate":

            //        BuildingsManager.GetInstance().WaitingForConstruction.Add((Tower)message.received);
            //        break;
            //    case "spawnerUpdate":

            //        BuildingsManager.GetInstance().WaitingForConstruction.Add((SpawnerBuilding)message.received);
            //        break;
            //}
        }



        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void LoadContent()
        {
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
            //Update en fonction des ordres du serveur, et clear automatique de la liste d'ordres
            ClientTranslator.UpdateGame(ref game, ref orders);
            //Update du jeu en interne
            game.Update(_gameTime);
            //Envoie des changements au serveur
            ClientTranslator.SendGameUpdate(game.Changes);
            //if (VsAI) { 
            ////Mise à jour des tâche de l'IA
            //AiManager.GetInstance().Update(_gameTime, newWave);
            //}
            // Mise à jour du gestionnaire d'interface
            uiManager.Update(_gameTime);

        }

        public override void Draw(SpriteBatch _spriteBatch)
        {         
            spriteBatch.Begin();

            // Si le jeu n'est pas encore chargé
            if (!loaded)
            {
                _spriteBatch.DrawString(CustomContentManager.Fonts["font"], "Chargement...", new Vector2(50, 50), Color.White);
            }

            //spriteBatch.Draw(CustomContentManager.GetInstance().Colors["grey"],Graphics.);
            // Affichage de la carte
            //MapManager.GetInstance().Draw(spriteBatch);
            //// Affichage des bâtiments
            //BuildingsManager.GetInstance().Draw(spriteBatch);
            //// Affichage des unités
            //UnitsManager.GetInstance().Draw(spriteBatch);
            // Affichage de l'interface
            uiManager.Draw(spriteBatch);

            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.Textures["cursor"];
            spriteBatch.Draw(fap, lol, Color.White);
            spriteBatch.End();
            
        }
    }
        

}
