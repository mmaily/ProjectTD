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

namespace owerTefenseGame.Screens
{
    class EntranceScreen : Screen
    {
        public List<GuiElement> UIElementsList;
        public EntranceScreen()
        {

        }
        public override void Initialize()
        {
            UIElementsList = new List<GuiElement>();
            #region Interface de construction en défense
            // Bouton de contruction de tour basique
            Button btnBuild = new Button(0,0,100,100)
            {
                Name = "BasicSpawner",
                Tag = "horsLigne"
            };
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion
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
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Draw(_spriteBatch);
            });
            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.GetInstance().Textures["cursor"];
            _spriteBatch.Draw(fap, lol, Color.White);
        }
    }
}
