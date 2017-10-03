using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Managers;
using DowerTefenseGame.GameElements;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;
using DowerTefenseGame.Players;
using System;
using LibrairieTropBien.Network.Game;
using DowerTefenseGame.Multiplayer;
using LibrairieTropBien.Network;

namespace DowerTefenseGame.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        //Role adopté par ce GameScreen
        public PlayerRole role = PlayerRole.Spectator;
        // Carte en cours
        private Map map;
        // Variables liées aux vagues
        private double lastWaveTick;
        private int waveCount;
        public static int waveLength = 10000;
        public double millisecPerFrame=1000;
        public double time;

        //Joueur (défenseur pour l'instant)
        public DefensePlayer defenseplayer;

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

            // Init des vagues
            lastWaveTick = 0;
            waveCount = 0;
        }
        
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            //Récupération de l'écran et instancition du spriteBatch
            this.Graphics = _graphics;
            spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            // Init de l'UI
            Graphics.PreferredBackBufferHeight = (MapManager.GetInstance().CurrentMap.mapHeight) * MapManager.GetInstance().CurrentMap.tileSize+topMargin*2;
            Graphics.PreferredBackBufferWidth = (MapManager.GetInstance().CurrentMap.mapWidth ) * MapManager.GetInstance().CurrentMap.tileSize+UIManager.GetInstance().zoneUi.Width+leftMargin*2;
            Graphics.ApplyChanges();
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
            // Si le jeu n'est pas encore chargé
            if (!loaded)
            {
                return;
            }

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
            if (VsAI) { 
            //Mise à jour des tâche de l'IA
            AiManager.GetInstance().Update(_gameTime, newWave);
            }
            // Mise à jour du gestionnaire d'interface
            UIManager.GetInstance().Update(_gameTime, newWave, timeSince);
            if (newWave == true)
            {
                BuildingsManager.GetInstance().lockBuildings();
                UIManager.GetInstance().CreateLockedList();
            }
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
