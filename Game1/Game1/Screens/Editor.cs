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

namespace DowerTefenseGame.Screens
{
    class Editor : Screen
    {
        public Editor()
        {

        }
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            UIElementsList = new List<GuiElement>();
            #region Création du bouton JOUER
            // Bouton de contruction de tour basique
            Button btnBuild = new Button(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 100, 100)
            {
                Name = "BasicSpawner",
                Tag = "horsLigne"

            };
            btnBuild.SetText("JOUER", CustomContentManager.GetInstance().Fonts["font"]);
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion
            #region Création du se connecter au compte
            // Bouton de contruction de tour basique
            btnBuild = new Button(0, 0, 150, 100)
            {
                Name = "BasicSpawner",
                Tag = "horsLigne"

            };
            btnBuild.SetText("SE CONNECTER", CustomContentManager.GetInstance().Fonts["font"]);
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
   
