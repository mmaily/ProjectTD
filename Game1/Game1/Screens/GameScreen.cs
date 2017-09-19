using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Managers;
using DowerTefenseGame.GameElements;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;

namespace DowerTefenseGame.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        // Carte en cours
        private Map map;

        // Empêche le multiple clic
        private bool leftClicked = false;

        /// <summary>
        /// Constructeur principal
        /// </summary>
        public GameScreen()
        {
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
        /// Mise à jour de l'écran
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            // Mise à jour du gestionnaire de carte
            MapManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire d'unités
            UnitsManager.GetInstance().Update(_gameTime);
            // Mise à jour du gestionnaire de bâtiments
            BuildingsManager.GetInstance().Update(_gameTime);
            //Mise à jour des tâche de l'IA
            AiManager.GetInstance().Update(_gameTime);

            #region === Sélection d'une tuile ===

            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();
            // Récupération de la position de la souris
            Point mousePosition = mouseState.Position;
            //On check si la souris est dans la zone map
            if (MapManager.GetInstance().GetMapZone().Contains(mousePosition))
            {
                // On récupère la tuile visée
                Tile selectedTile = map.Tiles[mousePosition.Y / map.tileSize, mousePosition.X / map.tileSize];
                // On marque la tuile comme sélectionnée
                selectedTile.overviewed = true;

                // Si le clic gauche est enclenché et que cela n'a pas encore été traité
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && leftClicked == false)
                {
                    // On signale le clic gauche
                    leftClicked = true;

                    // Récupération de l'ancienne tuile sélectionnée
                    Tile oldSelectedTile = UIManager.GetInstance().SelectedTile;
                    // Si c'est la même tuile qu'auparavant
                    if (selectedTile.Equals(oldSelectedTile))
                    {
                        // On désélectionne la tuile
                        selectedTile.selected = false;
                        // On annule la tuile sélectionnée
                        UIManager.GetInstance().SelectedTile = null;
                    }
                    else
                    {
                        // Sinon, on déselectionne l'ancienne si elle existe et on sélectionne la nouvelle
                        if (oldSelectedTile != null)
                        {
                            UIManager.GetInstance().SelectedTile.selected = false;
                        }
                        selectedTile.selected = true;
                        // On remplace l'ancienne par la nouvelle
                        UIManager.GetInstance().SelectedTile = selectedTile;
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && leftClicked == true)
                {
                    // Le bouton a été relâché, on peut écouter à nouveau cette information
                    leftClicked = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    //Créé une tour, qui d'ajoute d'elle même à la liste de construction de bâtiment de défense
                    BasicTower basicTower = new BasicTower(UIManager.GetInstance().SelectedTile);
                }
            }
            //}
            #endregion

            // Mise à jour du gestionnaire d'interface
            UIManager.GetInstance().Update(_gameTime);
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
