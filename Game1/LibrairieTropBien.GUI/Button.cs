using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using C3.MonoGame;

/// <summary>
/// Bouton de base. Par ce que les autres libraires m'ont fait CH§D%SM-Perdre du temps.
/// Adapté de http://xf9.de/code/buttons-in-monogame
/// </summary>
namespace LibrairieTropBien.GUI
{
    public class Button
    {

        /// <summary>
        /// Bords du bouton
        /// </summary>
        protected Rectangle buttonBox;

        private Boolean isActive;
        /// <summary>
        /// Activation du bouton
        /// </summary>
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// Handler des clics
        /// </summary>
        public event EventHandler OnClick;

        /// <summary>
        /// Couleur du bouton
        /// </summary>
        protected Color color;

        /// <summary>
        /// Défaut
        /// </summary>
        private Color defaultColor = new Color(140, 140, 140);

        /// <summary>
        /// Si sous la souris
        /// </summary>
        private Color defaultHover = new Color(0, 133, 188);

        /// <summary>
        /// Si actif
        /// </summary>
        private Color defaultActive = Microsoft.Xna.Framework.Color.LightBlue;

        /// <summary>
        /// Bouton de base
        /// </summary>
        public Button(int _x, int _y, int _width, int _height)
        {
            this.buttonBox = new Rectangle(_x, _y, _width, _height);
            this.isActive = false;
            this.color = Color.White;
        }

        /// <summary>
        /// Mise à jour de la boîte de délimitation
        /// </summary>
        /// <param name="buttonBoundingbox">the new bounding box</param>
        protected void UpdateBoundingbox(Rectangle _buttonBox)
        {
            this.buttonBox = _buttonBox;
        }

        /// <summary>
        /// Mise à jour du bouton
        /// </summary>
        /// <param name="mouseState">Etat actuel de la souris</param>
        public void Update(Microsoft.Xna.Framework.Input.MouseState mouseState)
        {

            // Si on écoute le clic, on regarde si la souris est sur le bouton
            if (this.OnClick != null && this.OnClick.GetInvocationList().Length > 0)
            {

                if (this.buttonBox.Contains(mouseState.Position))
                {
                    this.color = defaultHover;

                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        this.OnClickHandle(new EventArgs());
                    }
                }
                else
                {
                    if (this.isActive)
                        this.color = defaultActive;
                    else
                        this.color = defaultColor;
                }

            }
        }

        /// <summary>
        /// Handler de clic
        /// </summary>
        /// <param name="e">Evenement du clic</param>
        protected virtual void OnClickHandle(EventArgs e)
        {
            this.IsActive = !this.IsActive;

            EventHandler handler = OnClick;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Affichage du bouton
        /// </summary>
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            if (this.isActive)
            {
                _spriteBatch.DrawRectangle(new Vector2(35, 35), new Vector2(35, 35), color);
            }
        }
    }
}