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
using DowerTefense.Commons.Units;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using System.Reflection;
using DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings;

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
        private Boolean vsAI = true;
        private bool loaded=false;

        public bool VsAI { get => vsAI; set => vsAI = value; }
        public List<Building> Dummies { get; private set; }
        
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
            game = new GameEngine(vsAI);
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
            switch (message.Subject)
            {
                case "DummiesList":
                    game.Dummies = (List<Building>)message.received;
                    break;
                default:

                    //Evenements Ingame
                    lock (orders)
                    {
                        orders.Add(message);
                    }
                    break;

            }

        }



        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void LoadContent()
        {
            if (vsAI == true)
            {
                #region === Remplir le catalogue des unités de base OFFLINE==
                Dummies = new List<Building>();
                Building newBuilding;

                foreach (Tower.NameEnum tower in Enum.GetValues(typeof(Tower.NameEnum)))
                {
                    newBuilding = (Building)Activator.CreateInstance(Assembly.Load("DowerTefense.Commons").GetType("DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings." + tower.ToString()));
                    //newBuilding.DeleteOnEventListener();
                    Dummies.Add(newBuilding);
                }
                foreach (SpawnerBuilding.NameEnum spawn in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
                {
                    newBuilding = (Building)Activator.CreateInstance(Assembly.Load("DowerTefense.Commons").GetType("DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings." + spawn.ToString()));
                    //newBuilding.DeleteOnEventListener(); // On le "désactive" en le rendant désabonnant de son event listener d'action
                    Dummies.Add(newBuilding);
                }
                #endregion
            }
            else
            {
                MultiplayerManager.Send("DummiesRequest", "");
            }
        }



        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            
            if (Dummies != null && !loaded)
            {
                if (Dummies.Count != 0)
                {
                    uiManager.LoadContent(Dummies);
                    loaded = true;
                }
            }
            if (loaded)
            {
                //Update en fonction des ordres du serveur, et clear automatique de la liste d'ordres
                ClientTranslator.UpdateGame(ref game, ref orders, VsAI);

                //Update du jeu en interne
                game.Update(_gameTime);

                // Mise à jour du gestionnaire d'interface
                uiManager.Update(_gameTime);
                if (vsAI == false)
                {
                    // Envoi des modifications au serveur
                    ClientTranslator.SendGameUpdate(game.Changes);
                }
                else
                {
                    //Envoie des info a sois-même avec un méthode Translator spéciale
                    ClientTranslator.AutoGameUpdate(game.Changes, ref orders);
                }
            }
           
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (loaded)
            {
                // Affichage de l'interface
                uiManager.Draw(_spriteBatch);
            }


            //_spriteBatch.DrawString(CustomContentManager.Fonts["font"], "LOLILOL", new Vector2(lol++, lol++), Color.White);

            base.Draw(_spriteBatch);
        }
    }
        

}
