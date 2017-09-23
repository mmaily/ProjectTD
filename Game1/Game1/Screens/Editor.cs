using DowerTefenseGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using LibrairieTropBien.GUI;
using Microsoft.Xna;
using DowerTefenseGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;
using DowerTefenseGame.GameElements;

namespace DowerTefenseGame.Screens
{
    class Editor : Screen
    {
        public CustomContentManager contentManager;
        public Rectangle uiZone; //Zone de l'UI, avec les Tiles modèles, bouton pour vérifier la cohérence de la map
        public Rectangle tileZone; //Zone qui contient toutes les Tiles modèles
        public Vector2 tileZoneOffset;
        //public Tile[,] modelTiles;
        public List<Tile> modelTiles;
        public int numbreOfTiles = Enum.GetNames(typeof(Tile.TileTypeEnum)).Length;
        public int tileSize = Tile.tileSize;
        public int rowLength = 3; //Exprimé en nombre de Tile
        public Editor()
        {

            uiZone = new Rectangle(0, 0, 200, 400);

        }
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            //Ajuste la taille de la fenêtre
            Graphics.PreferredBackBufferHeight = 600;
            Graphics.PreferredBackBufferWidth = 600 + uiZone.Width;
            Graphics.ApplyChanges();
            //Ajuste la position de l'interface
            uiZone.Offset(Graphics.PreferredBackBufferWidth - uiZone.Width, 0);
            tileZone = new Rectangle(uiZone.Left + 5, uiZone.Top + 5, tileSize * rowLength, (int)Math.Ceiling((double)(numbreOfTiles / rowLength) + 1) * tileSize);
            tileZoneOffset = new Vector2(tileZone.Left,tileZone.Top);
            //Charge le manager
            this.contentManager = CustomContentManager.GetInstance();
            this.modelTiles = new List<Tile>();
            //this.modelTiles = new Tile[rowLength, (int)Math.Ceiling((double)(numbreOfTiles / rowLength) + 1)];
            //On rempli toutes les cases avec les type de Tile existantes
            int i = 0;
            foreach (Tile.TileTypeEnum tileType in Enum.GetValues(typeof(Tile.TileTypeEnum)))
            {
                Tile tile = new Tile(tileType);
                tile.line = (int)Math.Ceiling((double)(i / rowLength));
                tile.column = i % rowLength;
                modelTiles.Add(tile);
               i++;
            }

        }
        public override void LoadContent()
        {
            base.LoadContent();

        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            ScreenManager.GetInstance().SelectScreen(0);
        }
        public override void Update(GameTime _gameTime)
        {
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Update();
            });
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            SpriteBatch spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            spriteBatch.Begin();
            #region ===Affichage des Tiles modèles===
            foreach (Tile tile in modelTiles)
            {
                // On affiche la texture correspondant à la nature de la carte
                spriteBatch.Draw(contentManager.Textures[tile.TileType.ToString()], tileZoneOffset + new Vector2(tile.column, tile.line) * tileSize, null, null, null, 0f, Vector2.One * 0.5f, Color.White);
                // Si cette tuile est sélectionnée ou sous le curseur
                if (tile.selected || tile.overviewed)
                {
                    // On affiche la texture "sélectionnée" sur cette tuile
                    spriteBatch.Draw(contentManager.Textures["Mouseover"], tileZoneOffset + new Vector2(tile.column, tile.line) * tileSize, null, null, null, 0f, Vector2.One * 0.5f, Color.White);
                    // On reset le boolée "sous le curseur"
                    tile.overviewed = false;
                }
            }
            #endregion
            #region Affiche l'interface et le curseur
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Draw(spriteBatch);
            });
            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.GetInstance().Textures["cursor"];
            spriteBatch.Draw(fap, lol, Color.White);
            #endregion
            spriteBatch.DrawRectangle(uiZone, Color.Beige);
            spriteBatch.DrawRectangle(tileZone, Color.Beige);
            //Gestion des Tiles modèles

            spriteBatch.End();


        }
    }
}

   
