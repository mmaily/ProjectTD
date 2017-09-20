using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.GameElements;
using C3.MonoGame;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;
using System;
using DowerTefenseGame.Units;
using DowerTefenseGame.Players;

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

        #region Paramètres d'affichage

        // Mode attaque ou défense
        private string mode = "defense";

        // Police par défaut
        private SpriteFont deFaultFont;
        // Décalage de l'interface
        private int leftUIOffset;
        // Taille des boutons
        private byte btnSize = 35;

        #endregion

        // Carte en cours
        private Map currentMap;

        private Button btnBuild;
        private Button btnMode;

        //Joueur (défenseur pour l'instant)
        public DefensePlayer Player;

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
            Player = new DefensePlayer();
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
            btnBuild = new Button(leftUIOffset + 30, 100, btnSize, btnSize)
            {
                Tag = "BasicTower",
                Action = "build"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Tag], false);
            btnBuild.OnRelease += Btn_OnClick;

            btnMode = new Button(leftUIOffset + 30, currentMap.mapHeight * currentMap.tileSize - btnSize - 10, btnSize, btnSize)
            {
                Tag = "ModeSwitch",
                Action = "UI",
                Enabled = true
            };
            btnMode.OnRelease += Btn_OnClick;
            //Créé l'instance de joueur

        }

        private void Btn_OnClick(object sender, System.EventArgs e)
        {

            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;
                //On check l'or du joueur
                if (BuildingsManager.GetInstance().Price[btn.Tag] <= Player.totalGold)
                { 

                    if (btn.Action.Equals("build") && SelectedTile != null)
                    {
                        if (SelectedTile.TileType == Tile.TileTypeEnum.Free && SelectedTile.building == null)
                        {
                            // On créé un bâtiment de ce type
                            Building building = (Building)Activator.CreateInstance(Type.GetType("DowerTefenseGame.Units.Buildings." + btn.Tag));
                            // Que l'on place sur une tuile
                            building.SetTile(SelectedTile);
                            Player.totalGold -= building.Cost;


                        }
                    }
                    else if (btn.Action.Equals("UI"))
                    {
                        if (btn.Tag.Equals("ModeSwitch"))
                        {
                            if (mode.Equals("defense"))
                            {
                                mode = "attack";
                            }
                            else if (mode.Equals("attack"))
                            {
                                mode = "defense";
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Mise à jour de l'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {
            btnBuild.Update(Mouse.GetState());
            btnMode.Update(Mouse.GetState());
        }

        /// <summary>
        /// Affichage de l'interface
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            //Display le nombre de Spawner
            int offset = 380;
            _spriteBatch.DrawString(deFaultFont, "Or du joueur : " + Player.totalGold, new Vector2(leftUIOffset, offset), Color.White);
            offset = 400;
            _spriteBatch.DrawString(deFaultFont, "Nombre de Spawner(s) : " + BuildingsManager.GetInstance().FreeBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);
            offset = 420;
            _spriteBatch.DrawString(deFaultFont, "Nombre de Tour(s) : " + BuildingsManager.GetInstance().DefenseBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);
            
            // Affichage du nom de la carte
            _spriteBatch.DrawString(deFaultFont, currentMap.Name + " -- Mode : " + mode, new Vector2(leftUIOffset, 5), Color.Wheat);
            
            // Si une tuile est sélectionnée
            if(SelectedTile != null)
            {
                // On affiche les infos de la tuile
                this.DisplaySelectedTile(_spriteBatch);

                // Si la tuile possède un bâtiment
                if(SelectedTile.building != null)
                {
                    // Si le bâtiment possède une portée non nulle
                    if(SelectedTile.building.Range > 0)
                    {
                        _spriteBatch.DrawCircle(SelectedTile.building.Position, SelectedTile.building.Range, 50, Color.Green, 5);
                    }

                    if (mode.Equals("defense"))
                    {
                        // On cache le bouton Construire
                        btnBuild.Enabled = false;

                        // On affiche les infos du batiment
                        DisplayBuildingInfo(_spriteBatch);
                    }

                }
                else if (SelectedTile.TileType == Tile.TileTypeEnum.Free)
                {
                    if (mode.Equals("defense"))
                    {
                        // Sinon si la tuile est libre
                        btnBuild.Enabled = true;
                    }
                }
                else
                {
                    // On cache le bouton Construire
                    btnBuild.Enabled = false;
                }

            }
            else
            {
                // On cache le bouton Construire
                btnBuild.Enabled = false;
            }


            // Draw GUI on top of everything
            btnBuild.Draw(_spriteBatch);
            btnMode.Draw(_spriteBatch);
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
