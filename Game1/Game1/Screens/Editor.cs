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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DowerTefenseGame.Screens
{
    class Editor : Screen
    {
        public CustomContentManager contentManager;
        #region === Variables liées à l'interface ===
        public Rectangle uiZone; //Zone de l'UI, avec les Tiles modèles, bouton pour vérifier la cohérence de la map
        public Rectangle tileZone; //Zone qui contient toutes les Tiles modèles
        public Vector2 tileZoneOffset;
        public List<Tile> modelTiles;
        public int numbreOfTiles = Enum.GetNames(typeof(Tile.TileTypeEnum)).Length;
        public int tileSize = Tile.tileSize;
        public int rowLength = 3; //Exprimé en nombre de Tile
        #endregion
        public int leftMargin=5;
        public int topMargin=5;
        #region ===Variables liées à la map===
        public Rectangle mapZone;
        public Tile[,] EditedMap;
        public Vector2 mapZoneOffset;
        public int mapWidth = 20;
        public int mapHeight =20;
        public String mapName = "Belle";
        #endregion
        #region === Gestion de la selection===
        public Tile SelectedTile { get; set; }
        private bool leftClicked = false;
        public Tile.TileTypeEnum typeSaved;
        #endregion
        #region === Gestion de la sauvegarde ===
        private String path = Path.Combine("Content", "savedMaps");
        #endregion
        public Editor()
        {

            uiZone = new Rectangle(0, 0, 200, 400);

        }
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;

            //Charge le manager
            this.contentManager = CustomContentManager.GetInstance();

            #region ===Interface===
            //On rempli toutes les cases avec les type de Tile existantes
            this.modelTiles = new List<Tile>();
            int i = 0;
            foreach (Tile.TileTypeEnum tileType in Enum.GetValues(typeof(Tile.TileTypeEnum)))
            {
                Tile tile = new Tile(tileType);
                tile.line = (int)Math.Ceiling((double)(i / rowLength));
                tile.column = i % rowLength;
                modelTiles.Add(tile);
                i++;
            }
            #endregion
            #region  ===Map en train d'être editée===
            mapZone = new Rectangle(leftMargin,topMargin,mapWidth*tileSize,mapHeight*tileSize);
            EditedMap = new Tile[mapWidth,mapHeight];
            Tile blockedTile;
            for(int j =0; j < EditedMap.GetLength(0); j++)
            {
                for(int k = 0; k < EditedMap.GetLength(1); k++)
                {
                    blockedTile= new Tile(Tile.TileTypeEnum.Blocked);
                    EditedMap[j, k] = blockedTile;
                }
            }
            //Ajuste la position du cadre
            mapZoneOffset = new Vector2(mapZone.Left, mapZone.Top);
            #endregion
            //Ajuste la taille de la fenêtre
            Graphics.PreferredBackBufferHeight = mapZone.Height+topMargin*2; //*2 pour avoir aussi la marge en bas
            Graphics.PreferredBackBufferWidth = mapZone.Width + uiZone.Width+leftMargin*2;
            Graphics.ApplyChanges();
            //Ajuste la position de l'interface
            uiZone.Offset(Graphics.PreferredBackBufferWidth - uiZone.Width, 0);
            tileZone = new Rectangle(uiZone.Left + 5, uiZone.Top + 5, tileSize * rowLength, (int)Math.Ceiling((double)(numbreOfTiles / rowLength) + 1) * tileSize);
            tileZoneOffset = new Vector2(tileZone.Left, tileZone.Top);
            #region ===Les boutons !!!===
            UIElementsList = new List<GuiElement>();
            // Bouton de contruction de tour basique
            int height = 100;
            int width = 150;
            Button newButton = new Button(uiZone.Left+uiZone.Width/2-width/2, 2*uiZone.Height / 10, width, height)
            {
                Name = "BasicSpawner",
                Tag = "reset"

            };
            newButton.SetText("Reset", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            //On sauve la map ou bien ?
            newButton = new Button(uiZone.Left + uiZone.Width / 2 - width / 2, 4*uiZone.Height / 10, width, height)
            {
                Name = "BasicSpawner",
                Tag = "saveMap"

            };
            newButton.SetText("Save", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            //On charge une map ou bien ?
            newButton = new Button(uiZone.Left + uiZone.Width / 2 - width / 2, 6* uiZone.Height / 10, width, height)
            {
                Name = "BasicSpawner",
                Tag = "openMap"

            };
            newButton.SetText("Open", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion
        }
        public override void LoadContent()
        {
            base.LoadContent();

        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;

                switch (btn.Tag.ToString())
                {
                    case "reset":
                        resetMap();
                        break;
                    case "saveMap":
                        saveMap();
                        break;
                   case "openMap":
                        GenerateMap(openMap(mapName));
                        break;
                    default:
                        break;
                }
            }

        }
        public override void Update(GameTime _gameTime)
        {
            UpdateSelectedTileMap();
            UpdateSelectedTileModel();
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
            spriteBatch.DrawRectangle(uiZone, Color.Beige);
            spriteBatch.DrawRectangle(tileZone, Color.Beige);
            #region === Zone de la map ===
            spriteBatch.DrawRectangle(mapZone, Color.Beige);
            for (int j = 0; j < EditedMap.GetLength(0); j++)
            {
                for (int k = 0; k < EditedMap.GetLength(1); k++)
                {
                    // On affiche la texture correspondant à la nature de la carte
                    spriteBatch.Draw(contentManager.Textures[EditedMap[j,k].TileType.ToString()], mapZoneOffset + new Vector2(j, k) * tileSize, null, null, null, 0f, Vector2.One * 0.5f, Color.White);
                    // Si cette tuile est sélectionnée ou sous le curseur
                    if (EditedMap[j, k].selected || EditedMap[j, k].overviewed)
                    {   
                        if (SelectedTile != null)
                        {
                            // On affiche la texture "sélectionnée" sur cette tuile
                            spriteBatch.Draw(contentManager.Textures[SelectedTile.TileType.ToString()], mapZoneOffset + new Vector2(j, k) * tileSize, null, null, null, 0f, Vector2.One * 0.5f, Color.White);
                            // On reset le boolée "sous le curseur"
                        }
                        else
                        {
                            // On affiche la texture "sélectionnée" sur cette tuile
                            spriteBatch.Draw(contentManager.Textures["Mouseover"], mapZoneOffset + new Vector2(j, k) * tileSize, null, null, null, 0f, Vector2.One * 0.5f, Color.White);
                            // On reset le boolée "sous le curseur"
                        }

                        EditedMap[j, k].overviewed = false;
                    }
                }
            }
            #endregion

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
            //Gestion des Tiles modèles

            spriteBatch.End();


        }
        private void UpdateSelectedTileMap()
        {
            #region === Sélection d'une tuile ===

            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();
            // Récupération de la position de la souris
            Point mousePosition = mouseState.Position;
            //On check si la souris est dans la zone map
            if (mapZone.Contains(mousePosition))
            {
                // On récupère la tuile visée
                Tile selectedTile = EditedMap[(mousePosition.X-leftMargin) / tileSize, (mousePosition.Y-topMargin) / tileSize];
                // On marque la tuile comme sélectionnée
                selectedTile.overviewed = true;

                // Si le clic gauche est enclenché et que cela n'a pas encore été traité
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && SelectedTile!=null)
                {
                    // On signale le clic gauche
                    leftClicked = true;
                    selectedTile.TileType = SelectedTile.TileType;
                
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && leftClicked == true)
                {
                    // Le bouton a été relâché, on peut écouter à nouveau cette information
                    leftClicked = false;
                }
            }
            #endregion
        }
        private void UpdateSelectedTileModel()
        {
            #region === Sélection d'une tuile ===

            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();
            // Récupération de la position de la souris
            Point mousePosition = mouseState.Position;
            //On check si la souris est dans la zone map
            if (tileZone.Contains(mousePosition))
            {
                // On récupère la tuile visée
                Tile selectedTile = modelTiles.Find(tile => tile.line== (mousePosition.Y - tileZone.Top) / tileSize&& tile.column== (mousePosition.X - tileZone.Left) / tileSize);
                //EditedMap[(mousePosition.X - leftMargin) / tileSize, (mousePosition.Y - topMargin) / tileSize];
                // On marque la tuile comme sélectionnée
                if (selectedTile != null)
                {
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
                            this.typeSaved = Tile.TileTypeEnum.Blocked;
                        }
                        else
                        {
                            // Sinon, on déselectionne l'ancienne si elle existe et on sélectionne la nouvelle
                            if (oldSelectedTile != null)
                            {
                                this.SelectedTile.selected = false;
                            }
                            selectedTile.selected = true;
                            // On remplace l'ancienne par la nouvelle et on sauvegarde son type
                            this.SelectedTile = selectedTile;
                            this.typeSaved = selectedTile.TileType;
                        }
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Released && leftClicked == true)
                    {
                        // Le bouton a été relâché, on peut écouter à nouveau cette information
                        leftClicked = false;
                    }
                }
               
            }
            #endregion
        }
        public Rectangle GetMapZone()
        {
            Rectangle rec = new Rectangle(leftMargin, topMargin, this.mapWidth * tileSize, mapHeight * tileSize);
            return rec;
        }
        public void resetMap()
        {
            Tile blockedTile;
            for (int j = 0; j < EditedMap.GetLength(0); j++)
            {
                for (int k = 0; k < EditedMap.GetLength(1); k++)
                {
                    blockedTile = new Tile(Tile.TileTypeEnum.Blocked);
                    EditedMap[j, k] = blockedTile;
                }
            }
        }
        public void saveMap()
        {
            //On créé l'objet serializabe+ on l'instancie
            XmlMap map =new XmlMap(EditedMap,tileSize,mapHeight,mapWidth);
            //On fait le bordel avec le stream et tout ta race

            // Open a file and serialize the object into it in binary format.
            // EmployeeInfo.osl is the file that we are creating. 
            // Note: -you can give any extension you want for your file
            // If you use custom extensions, then the user will now
            //   that the file is associated with your program.
            Stream stream = File.Open(Path.Combine(path,"Map_" + mapName + ".osl"), FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, map);
            stream.Close();
        }
        public XmlMap openMap(String name)
        {
            //Clear mp for further usage.
            XmlMap mapObject = null;
            //Open the file written above and read values from it.
            Stream stream = File.Open(Path.Combine(path,"Map_" + mapName + ".osl"), FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            mapObject = (XmlMap)bformatter.Deserialize(stream);
            stream.Close();
            return mapObject;
        }
        public void GenerateMap(XmlMap TempMap)
        {
            for (int j = 0; j < TempMap.width; j++)
            {
                for (int k = 0; k < TempMap.height; k++)
                {
                    EditedMap[j, k] = TempMap.map[j,k];
                }
            }
        }
    }
}

   
