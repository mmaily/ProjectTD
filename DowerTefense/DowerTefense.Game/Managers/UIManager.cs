using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework.Input;
using System;
using DowerTefense.Game.Players;
using System.Collections.Generic;
using DowerTefense.Game.Screens;
using System.Threading.Tasks;
using LibrairieTropBien.Network.Game;
using DowerTefense.Game.Multiplayer;
using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.Units;
using DowerTefense.Commons;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Commons.Managers;
using System.Reflection;
using DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings;

namespace DowerTefense.Game.Managers
{

    /// <summary>
    /// Gestionnaire d'unité
    /// </summary>
    public class UIManager
    {
        UIManager instance = null;
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
        public Vector2 marginOffset = Vector2.One * 5;
        //Rectangle-zone pour l'ui
        public Rectangle zoneUi;
        private float imageRatio;
        // Taille des boutons
        private byte btnSize = 35;
        //Offset de la map par rapport au coin sup droit
        private Vector2 mapOffset;
        #endregion
        #region Copie des éléments du jeu
        private GameEngine game;
        // Carte en cours
        public Map currentMap { get; set; }
        #endregion
        private double millisecPerFrame;
        private double time;
        #region ==Element d'interface (boutons, bar de progression)
        // Éléments d'interface
        public List<GuiElement> UIElementsList;
        //Liste des boutons de la liste locked
        private List<Button> lockedButton;
        private Dictionary<Button, InfoPopUp> PopUp;
        // Exception pour la barre de progression
        private ProgressBar progressBarWaves;
        #endregion
        //Role
        protected PlayerRole role;
        //Joueur (défenseur pour l'instant)
        public DefensePlayer defensePlayer;
        //Joueur attaquand
        public AttackPlayer attackPlayer;
        //Point de vue adoptée par l'interface
        #region Gestion du lock des spawn
        private Dictionary<Button, SpawnerBuilding> ActiveList;
        private Dictionary<Button, SpawnerBuilding> LockedList;
        #endregion

        #region Gestion des bâtiments Dummies 
        //Catalogue des bâtiment de bases (utile pour display les info de construcion par exemple)
        public List<Building> Dummies;

        #endregion
        /// <summary>
        /// Constructeur du gestionnaire d'unité
        /// </summary>
        public UIManager(GameEngine game)
        {
            this.game = game;
            currentMap = game.map;
            // Récupération du décalage gauche de l'interface
            currentMap = currentMap;
            mapOffset = new Vector2(ScreenManager.Screens["GameScreen"].leftMargin, ScreenManager.Screens["GameScreen"].topMargin);
            leftUIOffset = currentMap.mapWidth * currentMap.tileSize + (int)mapOffset.Y * 2;
            //Création d'une zone pour l'ui
            zoneUi = new Rectangle(leftUIOffset, (int)mapOffset.Y, 300, currentMap.mapHeight * currentMap.tileSize);
            //Calcule le facteur d'échelle entre les texture (en général 64px) sur la taille des Tiles
            this.imageRatio = (float)game.map.tileSize / (float)CustomContentManager.textureSize;
            // Récupération de la police par défaut
            deFaultFont = CustomContentManager.Fonts["font"];
            //Instanciation des joueurs

            defensePlayer = game.defensePlayer;
            attackPlayer = game.attackPlayer;


            // Initialisation de la liste des éléments d'interface
            UIElementsList = new List<GuiElement>();
            PopUp = new Dictionary<Button, InfoPopUp>();
            #region === Remplir le catalogue des unités de base==
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
            instance = this;
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'interface
        /// </summary>
        /// <returns></returns>
        public UIManager GetInstance()
        {
            return instance;
        }

        public void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            Button btnBuild;
            GuiElement panel;
            //Chargement des éléments selon le role adopté
            if (role == PlayerRole.Defender || role == PlayerRole.Debug)
            {
                #region Interface de construction de défense
                // Boutons de contruction des tours
                int j = 0;
                foreach (Tower.NameEnum tower in Enum.GetValues(typeof(Tower.NameEnum)))
                {

                    btnBuild = new Button(leftUIOffset + 30 + j * btnSize, 100, btnSize, btnSize)
                    {
                        Name = tower.ToString(),
                        Tag = "defenseBuild",
                        PopUpAttached = true
                    };
                    btnBuild.SetTexture(CustomContentManager.Textures[btnBuild.Name], false);
                    btnBuild.OnRelease += Btn_OnClick;
                    UIElementsList.Add(btnBuild);
                    //Add la popUp qui va bien
                    InfoPopUp info = new InfoPopUp(btnBuild.elementBox)
                    {
                        Name = tower.ToString() + "Info",
                        Tag = "InfoPopUp",
                        font = CustomContentManager.Fonts["font"],
                        texture = CustomContentManager.Colors["pixel"]
                    };
                    PopUp.Add(btnBuild, info);
                    Dummies.Find(b => b.name.Equals(btnBuild.Name)).SetInfoPopUp(info);
                    j++;
                }
                #endregion
            }
            if (role == PlayerRole.Attacker || role == PlayerRole.Debug)
            {
                #region Interface de construction en attaque
                // Bouton de contruction de spawner
                int n = 0;
                foreach (SpawnerBuilding.NameEnum spawner in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
                {

                    btnBuild = new Button(leftUIOffset + 30 + n * btnSize, 100, btnSize, btnSize)
                    {
                        Name = spawner.ToString(),
                        Tag = "attackBuild",
                        PopUpAttached = true
                    };
                    btnBuild.SetTexture(CustomContentManager.Textures[btnBuild.Name], false);
                    btnBuild.OnRelease += Btn_OnClick;
                    UIElementsList.Add(btnBuild);
                    //Add la popUp qui va bien
                    InfoPopUp info = new InfoPopUp(btnBuild.elementBox)
                    {
                        Name = spawner.ToString() + "Info",
                        Tag = "InfoPopUp",
                        font = CustomContentManager.Fonts["font"],
                        texture = CustomContentManager.Colors["pixel"]
                    };
                    PopUp.Add(btnBuild, info);
                    Dummies.Find(b => b.name.Equals(btnBuild.Name)).SetInfoPopUp(info);
                    n++;
                }
                #endregion
                #region Interface de composition d'armée
                panel = new GuiElement(leftUIOffset, 600, zoneUi.Width, 300)
                {
                    Name = "ArmyCompo",
                    Tag = "attackBuild",
                    font = deFaultFont,
                    texture = CustomContentManager.Colors["pixel"]
                };
                panel.setText("Lol");
                UIElementsList.Add(panel);
                #endregion
            }
            if (role == PlayerRole.Debug)
            {
                // Bouton de changement de mode
                Button btnMode = new Button(Graphics.PreferredBackBufferWidth - btnSize, 0, btnSize, btnSize)
                {
                    Name = "ModeSwitch",
                    Tag = "UI",
                    Enabled = true
                };
                btnMode.OnRelease += Btn_OnClick;

                UIElementsList.Add(btnMode);
            }
            #region Listes des bâtiments actifs et locked
            ActiveList = new Dictionary<Button, SpawnerBuilding>();
            LockedList = new Dictionary<Button, SpawnerBuilding>();
            lockedButton = new List<Button>();
            #endregion



            // Barre de progression des vagues
            progressBarWaves = new ProgressBar(15, 15, (int)(currentMap.mapWidth * currentMap.tileSize * 0.90) - 15, 15)
            {
                Name = "ProgressBarWave",
                Max = game.waveLength,
                Tag = "UI",
            };

            UIElementsList.Add(progressBarWaves);
        }

        private void Btn_OnClick(object sender, System.EventArgs e)
        {

            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;
                // Si le bouton est une construction et qu'on l'on sélectionne une tuile
                if (btn.Tag.Equals("defenseBuild") && SelectedTile != null)
                {
                    // On récupère le bâtiment à construire
                    Building building = Dummies.Find(b => b.name.Equals(btn.Name));
                    // Si la tuile est libre, n'a pas de bâtiment dessus et le joueur a assez d'argent
                    if (SelectedTile.TileType == Tile.TileTypeEnum.Free
                        && SelectedTile.building == null
                        && (building.Cost <= defensePlayer.totalGold))
                    {
                        building = (Tower)building.DeepCopy();
                        building.SetTile(SelectedTile, game.map);
                        // Ajout à la liste des bâtiments
                        game.WaitingForConstruction.Add(building);
                        //On signale le changement dans le log
                        //TODO : A un moment il faut clear la liste WaitingFrConstruction
                        game.Changes[game.DWaitingForConstruction] = true;
                    }
                }
                // Si le bouton est une construction attaque
                if (btn.Tag.Equals("attackBuild"))
                {
                    // On récupère le bâtiment à construire
                    Building building = Dummies.Find(b => b.name.Equals(btn.Name));
                    if (building.Cost <= attackPlayer.totalGold)
                    {
                        SpawnerBuilding spawnbuilding = (SpawnerBuilding)building.DeepCopy();
                        MultiplayerManager.Send("spawnerUpdate", building);
                    }
                }
                #region ===Gestion de l'appui sur les boutons de l'attaquant sur sa liste active===
                if (btn.Tag.Equals("ActiveList") && attackPlayer.totalEnergy - attackPlayer.usedEnergy >= (ActiveList[btn].PowerNeeded) && !(ActiveList[btn].powered))
                {
                    ActiveList[btn].powered = true;
                    attackPlayer.usedEnergy += ActiveList[btn].PowerNeeded;
                    btn.GreyedOut = false;
                    return;
                }
                if (btn.Tag.Equals("ActiveList") && (ActiveList[btn].powered))
                {
                    ActiveList[btn].powered = false;
                    attackPlayer.usedEnergy -= ActiveList[btn].PowerNeeded;
                    btn.GreyedOut = true;
                    return;
                }
                #endregion
                else if (btn.Tag.Equals("UI"))
                {
                    if (btn.Name.Equals("ModeSwitch"))
                    {
                        if (mode.Equals("defense") || mode.Equals("both"))
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
        public void Update(GameTime _gameTime)
        {
            // Mise à jour de la sélection de tuile
            UpdateSelectedTile();

            // Mise à jour de l'état de la bar de vague
            progressBarWaves.State = game.timeSince;

            // Mise à jour de tous les élémets d'interface
            Parallel.ForEach(UIElementsList, element =>
            {
                if (element.GetType().Equals(typeof(InfoPopUp)))
                {
                    element.Update();
                }
                // Si l'élément est de type bouton
                if (element.GetType().Equals(typeof(Button)) && element.Tag == role + "Build")
                {
                    // On récupère le bâtiment associé
                    Building building = Dummies.Find(b => b.name.Equals(element.Name));
                    // On regarde le role adopté, et grise les boutons soit
                    if (role.Equals("defense") || role.Equals("both"))
                    {
                        element.GreyedOut = (building.Cost <= defensePlayer.totalGold) ? false : true;
                    }
                    if (role.Equals("attack") || role.Equals("both"))
                    {
                        element.GreyedOut = (building.Cost <= attackPlayer.totalGold) ? false : true;
                    }
                }                // Si l'élément est de type bouton
                //On update le bouton
                element.Update();
                //On update la popUp associée
                if (element.PopUpAttached == true)
                {
                    PopUp[(Button)element].Enabled = element.Enabled;
                    PopUp[(Button)element].Update();

                }
            });
            Parallel.ForEach(lockedButton, element =>
            {
                PopUp[(Button)element].Enabled = element.Enabled;
                PopUp[(Button)element].Update();
            });
            //Lock de la liste si newWave et attaquant
            if (role == PlayerRole.Attacker && game.newWave == true)
            {
                CreateLockedList();
            }
            #region Calcul des frame/seconde
            millisecPerFrame = _gameTime.TotalGameTime.TotalMilliseconds - time;
            time = _gameTime.TotalGameTime.TotalMilliseconds;
            #endregion

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
            if (MapEngine.GetMapZone(currentMap).Contains(mousePosition))
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
            #region === Affichage map ===


            // Pour chaque tuile de la carte
            foreach (Tile tile in game.map.Tiles)
            {
                // On affiche la texture correspondant à la nature de la carte
                _spriteBatch.Draw(CustomContentManager.Textures[tile.TileType.ToString()], new Vector2(tile.line * game.map.tileSize, tile.column * game.map.tileSize) + marginOffset, null, null, null, 0f, Vector2.One * imageRatio, Color.White);
                // Si cette tuile est sélectionnée ou sous le curseur
                if (tile.selected || tile.overviewed)
                {
                    // On affiche la texture "sélectionnée" sur cette tuile
                    _spriteBatch.Draw(CustomContentManager.Textures["Mouseover"], new Vector2(tile.line * game.map.tileSize, tile.column * game.map.tileSize) + marginOffset, null, null, null, 0f, Vector2.One * imageRatio, Color.White);
                    // On reset le boolée "sous le curseur"
                    tile.overviewed = false;
                }

            }
            #endregion
            #region ==Rectangle droite contenant l'ui==
            _spriteBatch.Draw(CustomContentManager.Colors["pixel"], zoneUi, Color.Purple);
            #endregion
            #region==Frames/seconde===
            int offset = 340;
            if (millisecPerFrame != 0)
            {

                _spriteBatch.DrawString(CustomContentManager.Fonts["font"], Math.Ceiling(1000 / (millisecPerFrame)).ToString(), new Vector2(leftUIOffset, offset), Color.White);
            }
            #endregion
            #region === Infos globales ===

            //Display le nombre de Spawner
            offset = 360;
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
            _spriteBatch.DrawString(deFaultFont, "Nombre de Spawner(s) : " + game.FreeBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);
            offset = 420;
            _spriteBatch.DrawString(deFaultFont, "Nombre de Tour(s) : " + game.DefenseBuildingsList.Count, new Vector2(leftUIOffset, offset), Color.White);

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
            if (SelectedTile != null)
            {
                // On affiche les infos de la tuile
                this.DisplaySelectedTile(_spriteBatch);

                // Si la tuile possède un bâtiment
                if (SelectedTile.building != null)
                {
                    // Si le bâtiment possède une portée non nulle
                    if (SelectedTile.building.Range > 0)
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
                        drawButton = true;
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
            //Affichage des bouttons de la liste vérouillée qui part en guerre
            Parallel.ForEach(lockedButton, element =>
            {
                element.Draw(_spriteBatch);
            });
            //Affichage des popUp en dernier
            Parallel.ForEach(PopUp.Values, element =>
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
            Button btnBuild = new Button(leftUIOffset + 30 + ActiveList.Count * btnSize, Graphics.PreferredBackBufferHeight - btnSize * 2, btnSize, btnSize)
            {
                Name = _building.name,
                Tag = "ActiveList",
                PopUpAttached = true
            };
            btnBuild.SetTexture(CustomContentManager.Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            //Add la popUp qui va bien
            InfoPopUp info = new InfoPopUp(btnBuild.elementBox)
            {
                Name = "ActiveSpawnInfo",
                Tag = "InfoPopUp",
                font = CustomContentManager.Fonts["font"],
                texture = CustomContentManager.Colors["pixel"]
            };
            PopUp.Add(btnBuild, info);
            _building.SetInfoPopUp(info);

            ActiveList.Add(btnBuild, (SpawnerBuilding)_building);

        }
        //Appelée depuis GameScreen pour créer la ligne de bâtiments bloqués
        public void CreateLockedList()
        {
            lockedButton.Clear();
            LockedList.Clear();
            foreach (SpawnerBuilding sp in game.LockedBuildingsList)
            {
                Button btnBuild = new Button(leftUIOffset + 30 + LockedList.Count * btnSize, Graphics.PreferredBackBufferHeight - btnSize * 4, btnSize, btnSize)
                {
                    Name = sp.name,
                    Tag = "LockedList",
                    PopUpAttached = true
                };
                btnBuild.SetTexture(CustomContentManager.Textures[btnBuild.Name], false);
                btnBuild.OnRelease += Btn_OnClick;
                btnBuild.GreyedOut = true;
                lockedButton.Add(btnBuild);
                //Add la popUp qui va bien
                InfoPopUp info = new InfoPopUp(btnBuild.elementBox)
                {
                    Name = "LockedSpawnInfo",
                    Tag = "InfoPopUp",
                    font = CustomContentManager.Fonts["font"],
                    texture = CustomContentManager.Colors["pixel"]
                };
                PopUp.Add(btnBuild, info);
                sp.SetInfoPopUp(info);
                LockedList.Add(btnBuild, (SpawnerBuilding)sp);
            }
        }
        public void SetRole(PlayerRole _role)
        {
            // Rôle du joueur en cours
            this.role = _role;
            // Mode d'affichage (pour debug / spectator ?)
            this.mode = _role == PlayerRole.Attacker ? "attack" : "defense";
        }
    }
}
