using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DowerTefense.Game.GameElements;
using DowerTefense.Game.GameElements.Projectiles;
using DowerTefense.Game.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Game.Units.Buildings;
using Microsoft.Xna.Framework;

namespace DowerTefense.Game.Managers
{

    /// <summary>
    /// Gestionnaire de contenu
    /// </summary>
    public static class CustomContentManager
    {
        // Instance du gestionnaire de contenu
        private static CustomContentManager instance;

        // Dictionnaire de textures
        public Dictionary<String, Texture2D> Textures;
        // Dictionnaire de polices
        public Dictionary<String, SpriteFont> Fonts;
        //Dictionnaire des couleurs
        public Dictionary<String, Texture2D> Colors;
        // Gestionnaire de contenu (parent)
        private ContentManager contentManager;
        public byte textureSize;

        /// <summary>
        /// Constructeur du gestionnaire de contenu
        /// </summary>
        private CustomContentManager()
        {
            // Initialisation des dictionnaires
            Textures = new Dictionary<String, Texture2D>();
            Fonts = new Dictionary<String, SpriteFont>();
            Colors = new Dictionary<String, Texture2D>();
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire de contenu
        /// Création si n'existe pas encore
        /// </summary>
        /// <returns></returns>
        public static CustomContentManager GetInstance()
        {
            // Si l'instance n'est pas encore crée
            if (instance == null)
            {
                instance = new CustomContentManager();
            }
            // On retourne l'instance
            return instance;
        }

        /// <summary>
        /// Chargement des textures
        /// </summary>
        /// <param name="_content"></param>
        public void LoadTextures(ContentManager _content, GraphicsDevice _graphicsDevice)
        {

            this.contentManager = _content;

            // Ajout de la texture d'unité
            AddTexture("Pawn", "unit");

            // Pour tous les types de tuile
            foreach (Tile.TileTypeEnum tileType in Enum.GetValues(typeof(Tile.TileTypeEnum)))
            {
                AddTexture("Maps/" + tileType.ToString(), tileType.ToString());
            }
           
            // Texture pour tuile sélectionnée
            AddTexture("Maps/Mouseover", "Mouseover");
            // Texture du curseur
            AddTexture("Cursors/Banana", "cursor");
            // Texture des tours
            foreach (Tower.NameEnum Name in Enum.GetValues(typeof(Tower.NameEnum)))
            {
                AddTexture("Maps/"+Name.ToString(), Name.ToString());
            }
            foreach (SpawnerBuilding.NameEnum Name in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
            {
                AddTexture("Maps/" + Name.ToString(), Name.ToString());
            }
            //Texture des projectiles
            foreach (Projectile.NameEnum Name in Enum.GetValues(typeof(Projectile.NameEnum)))
            {
                AddTexture(Name.ToString(), "");
            }
            

            // Police par défaut
            AddFonts("defaultFont", "font");


            textureSize = (byte)Textures["BasicTower"].Height;
            #region Catalogue de couleur
            // Make a 1x1 texture named pixel.  
            Texture2D pixel = new Texture2D(_graphicsDevice, 1, 1);

            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = {
                        Color.Gray,
                    };

            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);
            AddColors(pixel,"pixel");
            #endregion
        }

        /// <summary>
        /// Ajout d'une nouvelle texture
        /// </summary>
        /// <param name="_file"></param>
        /// <param name="_name"></param>
        public void AddTexture(String _file, String _name = "")
        {
            // Chargement de la texture demandée
            Texture2D newTexture = contentManager.Load<Texture2D>(_file);
            // Le nom est il renseigné ?
            if (_name == "")
            {
                // Si le fichier est renseigné, on utilise ce critère
                Textures.Add(_file, newTexture);
            }
            else
            {
                // Sinon, on utilise le nom
                Textures.Add(_name, newTexture);
            }
        }

        /// <summary>
        /// Ajout d'une nouvelle police
        /// </summary>
        /// <param name="_font"></param>
        /// <param name="_fontName"></param>
        public void AddFonts(String _font, String _fontName)
        {
            // Chargement de la police demandée
            SpriteFont newFont = contentManager.Load<SpriteFont>(_font);
            // Le nom est il renseigné ?
            if (_fontName == "")
            {
                // Si le ficihier est renseigné, on utilise ce critère
                Fonts.Add(_font, newFont);
            }
            else
            {
                // Sinon, on utilise le nom
                Fonts.Add(_fontName, newFont);
            }
        }
        public void AddColors(Texture2D _color, String _colorName = "")
        {

            // Le nom est il renseigné ?
            if (_colorName == "")
            {
                // Si le ficihier est renseigné, on utilise ce critère
                Colors.Add(_color.ToString(), _color);
            }
            else
            {
                // Sinon, on utilise le nom
                Colors.Add(_colorName, _color);
            }
        }
    }


}