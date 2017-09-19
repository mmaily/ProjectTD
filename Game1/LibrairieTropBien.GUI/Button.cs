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
        public Boolean pressed=false;
        public Boolean release = false;
        public String Text;
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
        public event EventHandler OnRelease;
        
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
            this.isActive = true;
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
            if (this.OnRelease != null && this.OnRelease.GetInvocationList().Length > 0)
            {

                

                if (this.buttonBox.Contains(mouseState.Position))
                {
                    //Méhode chiasse pour ne proc l'event qu'une fois par click..

                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        pressed = true;
                    }

                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released&&pressed)
                    {
                        release = true;
                    }
                    this.color = defaultHover;

                    if (pressed && release)
                    {                       
                        this.OnReleaseHandle(new EventArgs());


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
        protected virtual void OnReleaseHandle(EventArgs e)
        {
            EventHandler handler = OnRelease;
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
                _spriteBatch.DrawRectangle(this.buttonBox, color);
                //_spriteBatch.DrawString(Fonts["defaultFont"])
            }
        }
    }
}