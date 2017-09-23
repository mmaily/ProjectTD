using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using LibrairieTropBien.GUI;
using DowerTefenseGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DowerTefenseGame.Screens
{
    class EntranceScreen : Screen
    {
        // Etat de la connexion au serveur d'authentification
        private bool connected = false;

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
            Button newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2, width, height)
            {
                Name = "BasicSpawner",
                Tag = "horsLigne"

            };
            newButton.SetText("JOUER", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion
            #region Création du se connecter au compte
            newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2 + height, width, height)
            {
                Name = "BasicSpawner",
                Tag = "connexion"

            };
            newButton.SetText("SE CONNECTER", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion
            #region Création du bouton pour aller sur l'éditeur
            // Bouton pour aller à l'éditeur
            newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2 + 2 * height, width, height)
            {
                Name = "BasicSpawner",
                Tag = "editor"

            };
            newButton.SetText("EDITEUR MAP", CustomContentManager.GetInstance().Fonts["font"]);
            newButton.SetTexture(CustomContentManager.GetInstance().Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion

            #region Bouton de connexion
            newButton = new Button(10, 10, 40, 10)
            {
                Name = "loginButton",
                Tag = "tryConnect",
            };
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);

            #endregion

        }

        private void Btn_OnClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;

                switch (btn.Tag.ToString())
                {
                    case "horsLigne":
                        ScreenManager.GetInstance().SelectScreen(0);
                        break;
                    case "connexion":
                        ScreenManager.GetInstance().SelectScreen(0);
                        break;
                    case "editor":
                        ScreenManager.GetInstance().SelectScreen(2);
                        break;
                    case "tryConnect":
                        if (Multiplayer.MultiplayerManager.TryConnect("lolFap"))
                        {
                            btn.text = "Ouais !";
                        }
                        break;
                    default:
                        break;
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
