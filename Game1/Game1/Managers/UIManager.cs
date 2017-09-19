﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.GameElements;
using C3.MonoGame;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework.Input;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire d'unité
    /// </summary>
    class UIManager
    {

        // Instance du gestionnaire d'unité
        private static UIManager instance = null;
        /// <summary>
        /// Tuile sélectionnée
        /// </summary>
        public Tile SelectedTile { get; set; }

        // SpriteBatch
        private SpriteBatch spriteBatch;
        // Police par défaut
        private SpriteFont deFaultFont;
        // Décalage de l'interface
        private int leftUIOffset;
        // Carte en cours
        private Map currentMap;

        private Button btnBuild;

        /// <summary>
        /// Constructeur du gestionnaire d'unité
        /// </summary>
        private UIManager()
        {
            // Récupération du décalage gauche de l'interface
            currentMap = MapManager.GetInstance().CurrentMap;
            leftUIOffset = currentMap.mapWidth * currentMap.tileSize + 5;

            // Récupération de la police par défaut
            deFaultFont = CustomContentManager.GetInstance().Fonts["font"];
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'interface
        /// </summary>
        /// <returns></returns>
        public static UIManager GetInstance()
        {
            if (instance == null)
            {

                instance = new UIManager();

            }
            return instance;
        }

        public void Initialize()
        {
            btnBuild = new Button(35, 35, 35, 35);
            btnBuild.OnClick += BtnBuild_OnClick;
        }

        private void BtnBuild_OnClick(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mise à jour de l'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {
            btnBuild.Update(Mouse.GetState());

        }

        /// <summary>
        /// Affichage de l'interface
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            // Affichage du nom de la carte
            _spriteBatch.DrawString(deFaultFont, currentMap.Name, new Vector2(leftUIOffset, 5), Color.Wheat);
            
            // Si une tuile est sélectionnée
            if(SelectedTile != null)
            {
                // On affiche les infos de la tuile
                this.DisplaySelectedTile(_spriteBatch);

                // Si la tuile possède un bâtiment
                if(SelectedTile.building != null)
                {
                    // On cache le bouton Construire
                    btnBuild.IsActive = false;

                    DisplayBuildingInfo(_spriteBatch);

                    // Si le bâtiment possède une portée non nulle
                    if(SelectedTile.building.Range > 0)
                    {
                        _spriteBatch.DrawCircle(SelectedTile.building.Position, SelectedTile.building.Range, 50, Color.Green, 5);
                    }
                }
                // Sinon si la tuile est libre
                else if (SelectedTile.TileType == Tile.TileTypeEnum.Free)
                {
                    // On ajoute le bouton construire si il n'est pas déjà là
                    btnBuild.IsActive = true;
                }
                else
                {
                    // Sinon, on cache le bouton
                    btnBuild.IsActive = false;
                }
            }


            // Draw GUI on top of everything
            btnBuild.Draw(_spriteBatch);
        }

        /// <summary>
        /// Affichage des infos du bâtiment
        /// </summary>
        /// <param name="_spriteBatch"></param>
        private void DisplayBuildingInfo(SpriteBatch _spriteBatch)
        {
            int offset = 100;
            _spriteBatch.DrawString(deFaultFont, "Batiment : " + SelectedTile.building.name, new Vector2(leftUIOffset, offset), Color.White);

        }

        /// <summary>
        /// Affichage de la tuile sélectionnée et de ses infos
        /// </summary>
        /// <param name="_spriteBatch"></param>
        private void DisplaySelectedTile(SpriteBatch _spriteBatch)
        {
            int offset = 50;
            _spriteBatch.DrawString(deFaultFont, "Tuile en (" + SelectedTile.line + ", " + SelectedTile.column + ").", new Vector2(leftUIOffset, offset), Color.Wheat);
            _spriteBatch.DrawString(deFaultFont, "Type : " + SelectedTile.TileType.ToString(), new Vector2(leftUIOffset, offset + 15), Color.Wheat);


        }


    }
}
