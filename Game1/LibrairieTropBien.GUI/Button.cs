using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using C3.MonoGame;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
/// Bouton de base. Par ce que les autres libraires m'ont fait CH§D%SM-Perdre du temps.
/// Adapté de http://xf9.de/code/buttons-in-monogame
/// </summary>
namespace LibrairieTropBien.GUI
{
    
    /// <summary>
    /// Classe de base de tous les boutons
    /// </summary>
    public class Button : GuiElement
    {
        #region === Etat du bouton ===

        // Bouton cliqué
        public Boolean pressed = false;
        // Bouton sous la souris
        private Boolean hovered = false;



        #endregion

        #region === Propriétés du bouton

        public String text = null;
        public SpriteFont font = null;
        private Boolean HasText = false;
        /// <summary>
        /// Action à réaliser
        /// </summary>
        public string Action { get; set; }

        #endregion


        /// <summary>
        /// Handler des clics
        /// </summary>
        public event EventHandler OnRelease;

        #region === Affichage ===

        private Texture2D texture;

        /// <summary>
        /// Défaut
        /// </summary>
        private Microsoft.Xna.Framework.Color defaultColor = new Color(140, 140, 140);

        /// <summary>
        /// Si sous la souris
        /// </summary>
        private Color defaultHover = new Microsoft.Xna.Framework.Color(0, 133, 188);

        /// <summary>
        /// Si actif
        /// </summary>
        private Color defaultActive = Microsoft.Xna.Framework.Color.LightBlue;

        #endregion

        /// <summary>
        /// Bouton de base
        /// </summary>
        public Button(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            this.ElementColor = Microsoft.Xna.Framework.Color.White;
        }

        /// <summary>
        /// Mise à jour de la boîte de délimitation
        /// </summary>
        /// <param name="buttonBoundingbox">the new bounding box</param>
        protected void UpdateBoundingbox(Microsoft.Xna.Framework.Rectangle _buttonBox)
        {
            this.elementBox = _buttonBox;
        }

        /// <summary>
        /// Mise à jour du bouton
        /// </summary>
        /// <param name="mouseState">Etat actuel de la souris</param>
        public override void Update()
        {
            // Si le bouton n'est pas activé, ce n'est pas la peine
            if (!this.Enabled)
            {
                return;
            }

            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();

            // Si on écoute le clic, on regarde si la souris est sur le bouton
            if (this.OnRelease != null && this.OnRelease.GetInvocationList().Length > 0)
            {

                // Si la souris est sur le bouton
                if (this.elementBox.Contains(mouseState.Position))
                {
                    // On l'enregistre
                    hovered = true;

                    // On change la couleur
                    this.ElementColor = defaultHover;

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
                // Sinon si on n'est pas sous la souris
                else
                {
                    // On l'enregistre
                    hovered = false;

                    // On oublie qu'on l'on a cliqué
                    pressed = false;

                    if (this.Enabled)
                        this.ElementColor = defaultActive;
                    else
                        this.ElementColor = defaultColor;
                }

            }
            if (NeedDim)
            {
                Dim = 0.5f;
            }
            else
            {
               Dim = 1f;
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
        public override void Draw(SpriteBatch _spriteBatch)
        {
            // Si le bouton n'est pas activé, sortie rapide
            if (!this.Enabled)
            {
                return;
            }

            // Si la texture est définie
            if (texture != null)
            {
                _spriteBatch.Draw(texture, elementBox, Color.White*Dim);

                // Si on est sous la souris
                if (hovered)
                {
                    _spriteBatch.DrawRectangle(elementBox, Color.White*Dim, 2);
                }

            }
            else
            {
                _spriteBatch.DrawRectangle(this.elementBox, ElementColor);
            }
            if (HasText)
            {


                //// Measure string.
                //System.Drawing.SizeF stringSize = new System.Drawing.SizeF();
                //stringSize = this.Graphics.MeasureString(text, font);
                _spriteBatch.DrawString(font, text, new Vector2(this.elementBox.X, this.elementBox.Y) + 
                                        new Vector2(20,40), Color.Azure);
            }

        }


        /// <summary>
        /// Permet de modifier la texture du bouton
        /// </summary>
        /// <param name="_texture">Texture à appliquer</param>
        /// <param name="_resize">Recadrage du bouton ou non</param>
        public void SetTexture(Texture2D _texture, bool _resize = true)
        {
            this.texture = _texture;

            // Si on doit recadrer le bouton et si la textur est définie
            if (_resize && _texture != null)
            {
                this.elementBox.Width = _texture.Width;
                this.elementBox.Height = _texture.Height;
            }
        }
        public void SetText(String _text, SpriteFont _font)
        {
            this.text = _text;
            
            this.font= _font;
            HasText = true;
        }

    }
}