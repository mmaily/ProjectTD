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

    class Content_Manager
    {
        //    public Texture2D unit;
        //    public SpriteFont font;
        private static Content_Manager instance;//L'instance est privée pour empêcher d'autre classe de la modifier. Utiliser le getter GetInstance()

       public Dictionary<String, Texture2D> Textures;
       public Dictionary<String, SpriteFont> Fonts;
        public ContentManager CM;

        private Content_Manager()
        {
            Textures = new Dictionary<String, Texture2D>();
            Fonts = new Dictionary<String, SpriteFont>();

        }

        //Créé une seule instance du Content_Manager même si il est appelé plusieurs fois
        public static Content_Manager GetInstance()
        {
            if (instance == null)
            {
                instance = new Content_Manager();

            }
            return instance;
        }

        public void LoadTextures(ContentManager Content)
        {
            CM = Content;
            AddTexture("Pawn", "unit");
            AddFonts("defaultFont", "");

        }

        public void AddTexture(String file, String name = "")
        {
            Texture2D newTexture = CM.Load<Texture2D>(file);
                if(name == "")
                {
                Textures.Add(file, newTexture);
                }
                else
                {
                Textures.Add(name,newTexture);
                }
        }
        public void AddFonts(String font,String fontName)
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