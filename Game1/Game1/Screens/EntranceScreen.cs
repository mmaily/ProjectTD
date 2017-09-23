﻿using DowerTefenseGame.Screens;
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
    class EntranceScreen : Screen
    {

        public EntranceScreen()
        {

        }
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            UIElementsList = new List<GuiElement>();
            #region Création du bouton JOUER
            // Bouton de contruction de tour basique
            int height = 100;
            int width = 150;
            Button btnBuild = new Button((_graphics.PreferredBackBufferWidth-width) / 2, (_graphics.PreferredBackBufferHeight/2-height) / 2, width, height)
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
            btnBuild = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight/2 - height) / 2+height, width, height)
            {
                Name = "BasicSpawner",
                Tag = "connexion"

            };
            btnBuild.SetText("SE CONNECTER", CustomContentManager.GetInstance().Fonts["font"]);
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion
            #region Création du bouton pour aller sur l'éditeur
            // Bouton pour aller à l'éditeur
            btnBuild = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight/2 - height) / 2 + 2*height, width, height)
            {
                Name = "BasicSpawner",
                Tag = "editor"

            };
            btnBuild.SetText("EDITEUR MAP", CustomContentManager.GetInstance().Fonts["font"]);
            btnBuild.SetTexture(CustomContentManager.GetInstance().Textures[btnBuild.Name], false);
            btnBuild.OnRelease += Btn_OnClick;
            UIElementsList.Add(btnBuild);
            #endregion

        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;



                if (btn.Tag.Equals("horsLigne") )
                {
                    ScreenManager.GetInstance().SelectScreen(0);
                }
                if (btn.Tag.Equals("connexion"))
                {
                    ScreenManager.GetInstance().SelectScreen(0);
                }
                if (btn.Tag.Equals("editor"))
                {
                    ScreenManager.GetInstance().SelectScreen(2);
                }

            }

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
