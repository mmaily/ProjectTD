using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Managers;
using DowerTefenseGame.GameElements;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;
using DowerTefenseGame.Players;

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
        
        public override void Initialize()
        {

            // Init de l'UI
            UIManager.GetInstance().Initialize();
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
            // Affichage de la carte
            MapManager.GetInstance().Draw(_spriteBatch);
            // Affichage des unités
            UnitsManager.GetInstance().Draw(_spriteBatch);
            // Affichage des bâtiments
            BuildingsManager.GetInstance().Draw(_spriteBatch);
            // Affichage de l'interface
            UIManager.GetInstance().Draw(_spriteBatch);

            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.GetInstance().Textures["cursor"];
            _spriteBatch.Draw(fap, lol, Color.White);

            
        }
    }
        

}
