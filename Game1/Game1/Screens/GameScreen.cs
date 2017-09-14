using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.Managers;
using DowerTefenseGame.GameElements;
using Microsoft.Xna.Framework.Input;

namespace DowerTefenseGame.Screens
{
    /// <summary>
    /// Classe d'écran de jeu principal
    /// </summary>
    class GameScreen : Screen
    {
        // Texture à afficher
        Texture2D textureToDraw;
        // Carte en cours
        public Map map;
        
        /// <summary>
        /// Constructeur principal
        /// </summary>
        public GameScreen()
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
            map = mapManager.GetMap();
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

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    selectedTile.selected = !selectedTile.selected;
                }
            }
            //}
            #endregion
        }

        public override void Draw(SpriteBatch spritebatch)
        {

            // Affichage de la carte
            MapManager.GetInstance().Draw(spritebatch);
            //Draw the unit on the screen using the method in the UnitsManager
            UnitsManager.GetInstance().Draw(spritebatch);
            spritebatch.Draw(textureToDraw, Mouse.GetState().Position.ToVector2(), Color.White);


        }
        public override void Draw(SpriteBatch spritebatch, Vector2 pos, Color col, string _what)
        {

            textureToDraw = CustomContentManager.GetInstance().Textures[_what];




                //spritebatch.Draw(textureToDraw, pos - new Vector2(textureToDraw.Height / 2, textureToDraw.Width / 2), col);
            //spritebatch.DrawString(CustomContentManager.GetInstance().Fonts["defaultFont"], pos.ToString(), Vector2.Zero, Color.YellowGreen);

        }




    }
        

}
