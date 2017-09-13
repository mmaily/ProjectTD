using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Screens
{

    class ScreenManager
    {
        
        private static ScreenManager instance=null;//L'instance est privée pour empêcher d'autre classe de la modifier. Utiliser le getter GetInstance()
        ArrayList Screens; //Stockage de nos différent screen
        Screen currentScreen;

        private ScreenManager()
        {
            Screens = new ArrayList();
            Screens.Add(new GameScreen());
            currentScreen = (Screen)Screens[0];
        }

        //Créé une seule instance du ScreenManager même si il est appelé plusieurs fois
        public static ScreenManager GetInstance()
        {
            if (instance == null)
            {

            instance = new ScreenManager();
           
            }
            return instance;
        }
        public void SelectScreen(int id)
        {
            currentScreen = (Screen)Screens[id];
            LoadContent();

        }
        public virtual void LoadContent()
        {
            currentScreen.LoadContent();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col)
        {
            currentScreen.Draw(spriteBatch,pos,col);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col, string _what)
        {
            currentScreen.Draw(spriteBatch, pos, col, _what);
        }
        public virtual void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

    }   

}
