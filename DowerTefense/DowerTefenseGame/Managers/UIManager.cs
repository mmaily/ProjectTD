﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.GameElements;
using C3.MonoGame;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework.Input;
using DowerTefenseGame.Units.Buildings;
using System;
using DowerTefenseGame.Units;
using DowerTefenseGame.Players;
using System.Collections.Generic;
using DowerTefenseGame.Screens;
using System.Threading.Tasks;
using DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings;

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
        private bool leftClicked = false;

        #region Paramètres d'affichage
        private GraphicsDeviceManager Graphics;
        // Mode attaque ou défense
        private string mode = "defense";

        // Police par défaut
        private SpriteFont deFaultFont;
        // Décalage de l'interface
        public int leftUIOffset;
        //Rectangle-zone pour l'ui
        public Rectangle zoneUi;
        // Taille des boutons
        private byte btnSize = 35;

        #endregion

        // Carte en cours
        private Map currentMap;

        // Éléments d'interface
        private List<GuiElement> UIElementsList;
        //Liste des boutons de la liste locked
        private List<Button> lockedButton;
        // Exception pour la barre de progression
        private ProgressBar progressBarWaves;


        //Joueur (défenseur pour l'instant)
        public DefensePlayer defensePlayer;
        //Joueur attaquand
        public AttackPlayer attackPlayer;
        #region Gestion du lock des spawn
        private Dictionary<Button, SpawnerBuilding> ActiveList;
        private Dictionary<Button, SpawnerBuilding> LockedList;
        #endregion
        /// <summary>
        /// Constructeur du gestionnaire d'unité
        /// </summary>
        private UIManager()
        {
            // Récupération du décalage gauche de l'interface
            currentMap = MapManager.GetInstance().CurrentMap;


            leftUIOffset = currentMap.mapWidth * currentMap.tileSize + 5;
            //Création d'une zone pour l'ui
            zoneUi = new Rectangle(leftUIOffset, 0, 300, currentMap.mapHeight * currentMap.tileSize);

            // Récupération de la police par défaut
            deFaultFont = CustomContentManager.GetInstance().Fonts["font"];
            //Instanciation des joueurs
            defensePlayer = new DefensePlayer();
            attackPlayer = new AttackPlayer();

            // Initialisation de la liste des éléments d'interface
            UIElementsList = new List<GuiElement>();
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

        public void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            #region Interface de construction de défense
            // Bouton de contruction de tour basique
            Button btnBuild = new Button(leftUIOffset + 30, 100, btnSize, btnSize)
            {
                Name = "BasicTower",
                Tag = "defenseBuild"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            //Bouton de construction de tour rapide
            btnBuild = new Button(30+leftUIOffset+btnBuild.elementBox.Width, 100, btnSize, btnSize)
            {
                Name = "RapidFireTower",
                Tag = "defenseBuild"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion
            #region Interface de construction en attaquee
            // Bouton de contruction de tour basique
            btnBuild = new Button(leftUIOffset + 30, 100, btnSize, btnSize)
            {
                Name = "BasicSpawner",
                Tag = "attackBuild"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion
            #region Listes des bâtiments actifs et locked
            ActiveList = new Dictionary<Button, SpawnerBuilding>();
            LockedList = new Dictionary<Button, SpawnerBuilding>();
            lockedButton = new List<Button>();
            #endregion

            // Bouton de changement de mode
            Button btnMode = new Button(Graphics.PreferredBackBufferWidth-btnSize, 0, btnSize, btnSize)
            {
                Name = "ModeSwitch",
                Tag = "UI",
                Enabled = true
            };
            btnMode.OnRelease += Btn_OnClick;

            UIElementsList.Add(btnMode);

            // Barre de progression des vagues
            progressBarWaves = new ProgressBar(15, 15, (int)(currentMap.mapWidth * currentMap.tileSize * 0.90) - 15, 15)
            {
                Name = "ProgressBarWave",
                Max = GameScreen.waveLength,
                Tag = "UI",
            };

            UIElementsList.Add(progressBarWaves);
        }

        private void Btn_OnClick(object sender, System.EventArgs e)
        {

            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;
                    if (btn.Tag.Equals("defenseBuild") && SelectedTile != null && BuildingsManager.GetInstance().Price[btn.Name] <= defensePlayer.totalGold)
                    {
                            if (SelectedTile.TileType == Tile.TileTypeEnum.Free && SelectedTile.building == null)
                            {
                                // On créé un bâtiment de ce type
                                Building building = (Building)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings." + btn.Name));
                                // Que l'on place sur une tuile
                                building.SetTile(SelectedTile);
                                InfoPopUp info = new InfoPopUp(new Rectangle((int)((SelectedTile.getTilePosition().X - 0.5) * currentMap.tileSize),
                                                                    (int)((SelectedTile.getTilePosition().Y - 0.5) * currentMap.tileSize),
                                                                    currentMap.tileSize, currentMap.tileSize), 100, 50)
                        {
                            Name = "TowerInfo",
                            Tag = "InfoPopUp",
                            font = CustomContentManager.GetInstance().Fonts["font"],
                            texture = CustomContentManager.GetInstance().Colors["pixel"]
                        };
                        UIElementsList.Add(info);
                        building.SetInfoPopUp(info);
                        defensePlayer.totalGold -= building.Cost;


                            }
                        
                    }
                    if(btn.Tag.Equals("attackBuild") && BuildingsManager.GetInstance().Price[btn.Name] <= attackPlayer.totalGold)
                    {
                    SpawnerBuilding building = (SpawnerBuilding)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings." + btn.Name));
                    attackPlayer.totalGold -= building.Cost;
                    UpdateBtnLists(building);
                    BuildingsManager.GetInstance().FreeBuildingsList.Add(building);
                    if (attackPlayer.totalEnergy - attackPlayer.usedEnergy >= (building.PowerNeeded))
                        {
                        building.powered = true;
                        attackPlayer.usedEnergy += building.PowerNeeded;
                        }
                    }
                #region ===Gestion de l'appui sur les boutons de l'attaquand sur sa liste
                    if (btn.Tag.Equals("ActiveList")&& attackPlayer.totalEnergy - attackPlayer.usedEnergy >= (ActiveList[btn].PowerNeeded)&& !(ActiveList[btn].powered))
                    {
                    ActiveList[btn].powered = true;
                    attackPlayer.usedEnergy += ActiveList[btn].PowerNeeded;
                    btn.NeedDim = false;
                    return;
                    }
                    if (btn.Tag.Equals("ActiveList") && (ActiveList[btn].powered))
                    {
                        ActiveList[btn].powered = false;
                        attackPlayer.usedEnergy -= ActiveList[btn].PowerNeeded;
                        btn.NeedDim = true;
                    return;
                    }
                #endregion
                else if (btn.Tag.Equals("UI"))
                    {
                        if (btn.Name.Equals("ModeSwitch"))
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

        /// <summary>
        /// Mise à jour de l'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime, bool _newWave, int _timeSince)
        {
            // Mise à jour de la sélection de tuile
            UpdateSelectedTile();

            // Mise à jour de l'état de la bar de vague
            progressBarWaves.State = _timeSince;

            // Mise à jour de tous les élémets d'interface
            Parallel.ForEach(UIElementsList, element =>
            {
            #region ===Grise les boutons de construction en attaque====
                if (element.GetType().Equals(typeof(Button)) && element.Tag=="defenseBuild" && BuildingsManager.GetInstance().Price[element.Name] <= defensePlayer.totalGold) element.NeedDim = false;
                    if (element.GetType().Equals(typeof(Button)) && element.Tag == "defenseBuild" && BuildingsManager.GetInstance().Price[element.Name] > defensePlayer.totalGold)
                    {
                        element.NeedDim = true;
                    }
            #endregion
                element.Update();
            });
        }

        /// <summary>
        /// Mise à jour de la tuile sélectionnée
        /// </summary>
        private void UpdateSelectedTile()
        {
            #region === Sélection d'une tuile ===

            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();
            // Récupération de la position de la souris
            Point mousePosition = mouseState.Position;
            //On check si la souris est dans la zone map
            if (MapManager.GetInstance().GetMapZone().Contains(mousePosition))
            {
                // On récupère la tuile visée
                Tile selectedTile = currentMap.Tiles[mousePosition.X / currentMap.tileSize, mousePosition.Y / currentMap.tileSize];
                // On marque la tuile comme sélectionnée
                selectedTile.overviewed = true;

                // Si le clic gauche est enclenché et que cela n'a pas encore été traité
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && leftClicked == false)
                {
                    // On signale le clic gauche
                    leftClicked = true;

                    // Récupération de l'ancienne tuile sélectionnée
                    Tile oldSelectedTile = this.SelectedTile;
                    // Si c'est la même tuile qu'auparavant
                    if (selectedTile.Equals(oldSelectedTile))
                    {
                        // On désélectionne la tuile
                        selectedTile.selected = false;
                        // On annule la tuile sélectionnée
                        this.SelectedTile = null;
                    }
                    else
                    {
                        // Sinon, on déselectionne l'ancienne si elle existe et on sélectionne la nouvelle
                        if (oldSelectedTile != null)
                        {
                            this.SelectedTile.selected = false;
                        }
                        selectedTile.selected = true;
                        // On remplace l'ancienne par la nouvelle
                        this.SelectedTile = selectedTile;
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && leftClicked == true)
                {
                    // Le bouton a été relâché, on peut écouter à nouveau cette information
                    leftClicked = false;
                }
            }
            #endregion
        }

        /// <summary>
        /// Affichage de l'interface
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            #region ==Rectangle droite contenant l'ui==
            _spriteBatch.Draw(CustomContentManager.GetInstance().Colors["pixel"],zoneUi, Color.Purple);
            #endregion 
            #region === Infos globales ===

            //Display le nombre de Spawner
            int offset = 360;
            _spriteBatch.DrawString(deFaultFont, "Vie du joueur : " + defensePlayer.lives, new Vector2(leftUIOffset, offset), Color.White);
            if (mode.Equals("defense"))
            {
                offset = 380;
                _spriteBatch.DrawString(deFaultFont, "Or du joueur : " + defensePlayer.totalGold, new Vector2(leftUIOffset, offset), Color.White);
            }
            if (mode.Equals("attack"))
            {
                offset = 380;
                _spriteBatch.DrawString(deFaultFont, "Or du joueur : " + attackPlayer.totalGold, new Vector2(leftUIOffset, offset), Color.White);
            }
            offset = 400;
            _spriteBatch.DrawString(deFaultFont, "Nombre de Spawner(s) : " + BuildingsManager.GetInstance().FreeBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);
            offset = 420;
            _spriteBatch.DrawString(deFaultFont, "Nombre de Tour(s) : " + BuildingsManager.GetInstance().DefenseBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);
            
            // Affichage du nom de la carte
            _spriteBatch.DrawString(deFaultFont, currentMap.Name + " -- Mode : " + mode, new Vector2(leftUIOffset, 5), Color.Wheat);

            #endregion

            #region === Infos tuile et construction ===


            //Display les info de bases en mode attaque
            if (mode.Equals("attack"))
            {
                offset = 340;
                _spriteBatch.DrawString(deFaultFont, "Energie du joueur : " + (attackPlayer.totalEnergy - attackPlayer.usedEnergy) + "/" + attackPlayer.totalEnergy, new Vector2(leftUIOffset, offset), Color.White);
            }
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
                        // On affiche les infos du batiment
                        DisplayBuildingInfo(_spriteBatch);
                    }

                }
            }
            // Booléen d'affichage de l'interface de construction
            bool drawButton = false;
            // Récupération de tous les éléments liés à la construction en défense
            List<GuiElement> buildElements = UIElementsList.FindAll(element => element.Tag.Equals("defenseBuild"));
            // Activation de tous les éléments d'inteface liés à la construction en défense
            Parallel.ForEach(buildElements, element =>
            {
                if (SelectedTile != null)
                {
                    if (mode.Equals("defense") && SelectedTile.TileType == Tile.TileTypeEnum.Free)
                    {
                        //element.Enabled = drawBuildUI;
                        drawButton=true;
                    }
                }//Si une tile est séléctionné+mode défense le booléen passe true
                element.Enabled = drawButton;

            });
            buildElements = UIElementsList.FindAll(element => element.Tag.Equals("attackBuild"));
            drawButton = false;
            Parallel.ForEach(buildElements, element =>
            {

                    if (mode.Equals("attack"))
                    {
                        //element.Enabled = drawBuildUI;
                        drawButton = true;
                    }
                element.Enabled = drawButton;

            });
            #endregion

            // Affichage de tous les éléments d'interface
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Draw(_spriteBatch);
            });
            Parallel.ForEach(lockedButton, element =>
            {
                element.Draw(_spriteBatch);
            });
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

        public void UpdateBtnLists(Building _building)
        {
            Button btnBuild = new Button(leftUIOffset + 30+ActiveList.Count*btnSize, Graphics.PreferredBackBufferHeight-btnSize*2, btnSize, btnSize)
            {
                Name = _building.name,
                Tag = "ActiveList"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            //Add la popUp qui va bien
            InfoPopUp info = new InfoPopUp(new Rectangle(btnBuild.elementBox.Left,
                                                         btnBuild.elementBox.Top,
            MapManager.GetInstance().CurrentMap.tileSize, currentMap.tileSize), 160, 60)
            {
                Name = "LockedSpawnInfo",
                Tag = "InfoPopUp",
                font = CustomContentManager.GetInstance().Fonts["font"],
                texture = CustomContentManager.GetInstance().Colors["pixel"]
            };
            UIElementsList.Add(info);
            _building.SetInfoPopUp(info);

            ActiveList.Add(btnBuild,(SpawnerBuilding)_building);

        }
        //Appelée depuis GameScreen pour créer la ligne de bâtiments bloqués
        public void CreateLockedList()
        {
            lockedButton.Clear();
            LockedList.Clear();
            foreach(SpawnerBuilding sp in BuildingsManager.GetInstance().LockedBuildingsList)
            {
                Button btnBuild = new Button(leftUIOffset + 30 + LockedList.Count * btnSize, Graphics.PreferredBackBufferHeight - btnSize * 4, btnSize, btnSize)
                {
                    Name = sp.name,
                    Tag = "LockedList"
                };
                btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
                btnBuild.OnRelease += Btn_OnClick;
                btnBuild.NeedDim = true;
                lockedButton.Add(btnBuild);
                //Add la popUp qui va bien
                InfoPopUp info = new InfoPopUp(new Rectangle(btnBuild.elementBox.Left,
                                                             btnBuild.elementBox.Top,
                MapManager.GetInstance().CurrentMap.tileSize, currentMap.tileSize), 160, 60)
                {
                    Name = "LockedSpawnInfo",
                    Tag = "InfoPopUp",
                    font = CustomContentManager.GetInstance().Fonts["font"],
                    texture = CustomContentManager.GetInstance().Colors["pixel"]
                };
                UIElementsList.Add(info);
                sp.SetInfoPopUp(info);
                LockedList.Add(btnBuild, (SpawnerBuilding)sp);
            }
        }
    }
}
