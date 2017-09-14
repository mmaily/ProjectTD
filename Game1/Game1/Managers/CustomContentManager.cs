using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DowerTefenseGame.GameElements;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire de contenu
    /// </summary>
    class CustomContentManager
    {
        // Instance du gestionnaire de contenu
        private static CustomContentManager instance;

        // Dictionnaire de textures
        public Dictionary<String, Texture2D> Textures;
        // Dictionnaire de polices
        public Dictionary<String, SpriteFont> Fonts;
        // Gestionnaire de contenu (parent)
        private ContentManager contentManager;

        /// <summary>
        /// Constructeur du gestionnaire de contenu
        /// </summary>
        private CustomContentManager()
        {
            // Initialisation des dictionnaires
            Textures = new Dictionary<String, Texture2D>();
            Fonts = new Dictionary<String, SpriteFont>();
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
        public void LoadTextures(ContentManager _content)
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
            // Texture de la tour
            AddTexture("Maps/BasicTower", "BasicTower");

            // Police par défaut
            AddFonts("defaultFont", "font");

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
    }


}