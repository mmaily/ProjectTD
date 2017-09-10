using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game1.Managers
{

    class CustomContentManager
    {
        //    public Texture2D unit;
        //    public SpriteFont font;

        private static CustomContentManager instance;

        public Dictionary<String, Texture2D> Textures;
        public Dictionary<String, SpriteFont> Fonts;
        public ContentManager CM;

        private CustomContentManager()
        {
            Textures = new Dictionary<String, Texture2D>();
            Fonts = new Dictionary<String, SpriteFont>();

        }

        //Créé une seule instance du Content_Manager même si il est appelé plusieurs fois
        public static CustomContentManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CustomContentManager();

            }
            return instance;
        }

        public void LoadTextures(ContentManager Content)
        {
            CM = Content;
            AddTexture("Pawn", "unit");

            // Pas bon je pense mais bon
            AddTexture("Maps/Blocked", "Blocked");
            AddTexture("Maps/Path", "Path");
            AddTexture("Maps/Free", "Free");


            AddFonts("defaultFont", "");

        }

        public void AddTexture(String file, String name = "")
        {
            Texture2D newTexture = CM.Load<Texture2D>(file);
            if (name == "")
            {
                Textures.Add(file, newTexture);
            }
            else
            {
                Textures.Add(name, newTexture);
            }
        }
        public void AddFonts(String font, String fontName)
        {
            SpriteFont newFont = CM.Load<SpriteFont>(font);
            if (fontName == "")
            {
                Fonts.Add(font, newFont);
            }
            else
            {
                Fonts.Add(fontName, newFont);
            }
        }
    }



}