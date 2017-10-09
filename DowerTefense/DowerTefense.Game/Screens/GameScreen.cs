using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefense.Game.Managers;
using System;
using DowerTefense.Game.Multiplayer;
using LibrairieTropBien.Network.Game;
using LibrairieTropBien.Network;
using DowerTefense.Commons;
using DowerTefense.Commons.GameElements;
using DowerTefense.Game.Translator;

namespace DowerTefense.Game.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        // Jeu
        private GameEngine game;
        private Map map;

        // Liste des ordres Serveur
        private List<Message> orders;

        // Interface Visuelle
        private Vector2 marginOffset;
        private UIManager uiManager;

        //Role adopté par ce GameScreen
        public PlayerRole role = PlayerRole.Debug;

        //Faire jouer l'AI
        private Boolean vsAI = false;
        public bool VsAI { get => vsAI; set => vsAI = value; }

        /// <summary>
        /// Constructeur principal
        /// </summary>
        public GameScreen()
        {
        }
        
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            // Initialise la liste des ordres du serveur
            orders = new List<Message>();
           
            // Initialisation du jeu et des variables associée
            game = new GameEngine();
            game.Initialize();
            map = game.map;

            // Calcul de l'échelle de scaling selon taille des Tile
            marginOffset = new Vector2(ScreenManager.Screens["GameScreen"].leftMargin, ScreenManager.Screens["GameScreen"].topMargin);

            //Récupération de l'écran et instancition du spriteBatch
            this.Graphics = _graphics;

            // Création de l'interface utilisateur
            uiManager = new UIManager(game);
            uiManager.SetRole(role);
            uiManager.Initialize(_graphics);

            // Init de l'UI
            Graphics.PreferredBackBufferHeight = (map.mapHeight) * map.tileSize+topMargin*2;
            Graphics.PreferredBackBufferWidth = (map.mapWidth ) * map.tileSize + uiManager.zoneUi.Width+leftMargin*2;
            Graphics.ApplyChanges();

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
        }



        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void LoadContent()
        {
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            //Update en fonction des ordres du serveur, et clear automatique de la liste d'ordres
            ClientTranslator.UpdateGame(ref game, ref orders);
            
            //Update du jeu en interne
            game.Update(_gameTime);

            // Mise à jour du gestionnaire d'interface
            uiManager.Update(_gameTime);

            // Envoi des modifications
            ClientTranslator.SendGameUpdate(game.Changes);

        }

        public override void Draw(SpriteBatch _spriteBatch)
        {         
            // Affichage de l'interface
            uiManager.Draw(_spriteBatch);

            base.Draw(_spriteBatch);
        }
    }
        

}
