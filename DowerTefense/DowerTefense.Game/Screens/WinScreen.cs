using DowerTefense.Game.Managers;
using DowerTefense.Game.Screens;
using LibrairieTropBien.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownerTefense.Game.Screens
{
    public class WinScreen : Screen
    {
        /// <summary>
        /// Constructeur de base
        /// </summary>
        public WinScreen()
        {

        }

        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="_graphics"></param>
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            Button leaveButton = new Button(50, 100, 80, 30)
            {
                Name = "leave",
                Tag = "leave",
                Text = "Quitter",
                TextColor = Color.White,
                BackgroundColor = Color.DarkRed,
                font = CustomContentManager.Fonts["font"],
                GreyedOut = false,
            };

            leaveButton.OnReleaseLeft += Btn_OnClickLeft;
            leaveButton.OnReleaseRight += Btn_OnClickRight;
            UIElementsList.Add(leaveButton);


        }

        /// <summary>
        /// Handler de clic sur les boutons de l'interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OnClickLeft(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button)
              && System.Windows.Forms.Form.ActiveForm != null
              && System.Windows.Forms.Form.ActiveForm.Text.Equals("DowerTefense"))
                if (sender.GetType() == typeof(Button))
                {
                    Button btn = (Button)sender;
                    switch (btn.Tag.ToString())
                    {
                        case "leave":
                            ScreenManager.SetBackGroundScreen(null);
                            ScreenManager.SelectScreen("MenuScreen");
                            break;
                        default:
                            break;
                    }
                }
        }
        private void Btn_OnClickRight(object sender, EventArgs e) { }

        /// <summary>
        /// Mise à jour de tous les éléments d'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            foreach (GuiElement element in UIElementsList)
            {
                element.Update();
            }
        }

        /// <summary>
        /// Affihage de tous les éléments d'interfaces
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            foreach (GuiElement element in UIElementsList)
            {
                element.Draw(_spriteBatch);
            }
            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.Textures["cursor"];
            _spriteBatch.Draw(fap, lol, Color.White);
        }
    }
}