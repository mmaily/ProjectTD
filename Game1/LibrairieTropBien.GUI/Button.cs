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

        public Boolean pressed = false;
        public String Text;

        /// <summary>
        /// Bouton à dessiner ou pas
        /// </summary>
        public Boolean Enabled { get; set; }

        /// <summary>
        /// Action à réaliser
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Tag du bouton
        /// </summary>
        public string Tag { get; set; }

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
            this.Enabled = true;
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
            // Si le bouton n'est pas activé, ce n'est pas la peine
            if (!this.Enabled)
            {
                return;
            }

            // Si on écoute le clic, on regarde si la souris est sur le bouton
            if (this.OnRelease != null && this.OnRelease.GetInvocationList().Length > 0)
            {

                // Si la souris est sur le bouton
                if (this.buttonBox.Contains(mouseState.Position))
                {
                    // On change la couleur
                    this.color = defaultHover;

                    // Si on clique gauche une première fois
                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        // Etat appuyé
                        pressed = true;
                    }
                    // Si on relâche alors qu'on avait appuyé
                    else if (pressed && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        // On lance l'évènement
                        pressed = false;
                        this.OnReleaseHandle(new EventArgs());
                    }

                }
                else
                {
                    // On oublie qu'on l'on a cliqué
                    pressed = false;

                    if (this.Enabled)
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
            if (this.Enabled)
            {
                _spriteBatch.DrawRectangle(this.buttonBox, color);
            }
        }
    }
}