using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using C3.MonoGame;
using Microsoft.Xna.Framework.Input;

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
        public Boolean pressedRight = false;
        public Boolean pressedLeft = false;
        // Bouton sous la souris
        private Boolean hovered = false;
        public bool Hovered { get => hovered; set => hovered = value; }
        #endregion
        #region === Propriétés du bouton
        /// <summary>
        /// Action à réaliser
        /// </summary>
        public string Action { get; set; }
        public InfoPopUp info;
        public Boolean canBeSelected = false; //On peut séléctionner un bouton
        public Boolean Selected=false;
        #endregion
        #region === Affichage ===
        /// <summary>
        /// Si sous la souris
        /// </summary>
        public Color HoveredColor { get; set; }

        /// <summary>

        #endregion
        /// <summary>
        /// Handler des clics
        /// </summary>
        public event EventHandler OnReleaseRight;
        public event EventHandler OnReleaseLeft;

        /// <summary>
        /// Bouton de base
        /// </summary>
        public Button(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            ElementColor = Color.DarkBlue;
            HoveredColor = Color.White;
            BackgroundColor = Color.Transparent;
            TextColor = Color.Black;
            Disabled = false;
        }

        /// <summary>
        /// Mise à jour de la boîte de délimitation
        /// </summary>
        /// <param name="buttonBoundingbox">the new bounding box</param>
        protected void UpdateBoundingbox(Rectangle _buttonBox)
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
                if (info != null)
                {
                    info.Enabled = false;
                }
                return;
            }
            if (info != null)
            {
                info.Enabled = true;
            }
            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();

            // Si on écoute le clic, on regarde si la souris est sur le bouton
            if ((this.OnReleaseLeft!=null|| this.OnReleaseRight != null) 
                &&( this.OnReleaseRight.GetInvocationList().Length > 0|| this.OnReleaseLeft.GetInvocationList().Length > 0))
            {

                // Si la souris est sur le bouton
                if (this.elementBox.Contains(mouseState.Position))
                {
                    // On l'enregistre
                    Hovered = true; 
                    //Gestion du clic gauche
                    if(this.OnReleaseLeft != null)
                    {
                        // Si on clique gauche une première fois bouton gauche
                        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            // Etat appuyé
                            pressedLeft = true;
                        }
                        // Si on relâche alors qu'on avait appuyé sur le bouton droit
                        else if (pressedLeft && mouseState.LeftButton == ButtonState.Released)
                        {
                            // On lance l'évènement
                            pressedLeft = false;
                            this.OnReleaseHandleLeft(new EventArgs());
                            if (canBeSelected) { Selected = !Selected; }
                        }
                    }
                    if (this.OnReleaseRight != null)
                    {
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            // Etat appuyé
                            pressedRight = true;
                        }
                        // Si on relâche alors qu'on avait appuyé sur le bouton droit
                        else if (pressedRight && mouseState.RightButton == ButtonState.Released)
                        {
                            // On lance l'évènement
                            pressedRight = false;
                            this.OnReleaseHandleRight(new EventArgs());
                        }
                    }
                       
                }
                // Sinon si on n'est pas sous la souris
                else
                {
                    // On l'enregistre
                    Hovered = false;

                    // On oublie qu'on l'on a cliqué
                    pressedRight = false;
                    pressedLeft = false;
                }

            }
        }

        /// <summary>
        /// Handler de clic
        /// </summary>
        /// <param name="e">Evenement du clic</param>
        protected virtual void OnReleaseHandleLeft(EventArgs e)
        {
            EventHandler handler = OnReleaseLeft;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnReleaseHandleRight(EventArgs e)
        {
            // TODO : autofix
            EventHandler handler = OnReleaseRight;
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

            float opacity = this.GreyedOut ? this.Opacity : 1f;
            // Si la texture est définie
            if (texture != null)
            {
                _spriteBatch.Draw(texture, elementBox, Color.White* opacity);
            }
            else
            {
                // Texture non définie, on affiche un rectangle
                _spriteBatch.DrawRectangle(this.elementBox, ElementColor);

                // Affichage de la couleur du fond
                _spriteBatch.FillRectangle(this.elementBox, BackgroundColor);
            }

            // Si on est sous la souris ou séléctionné
            if ((Hovered||Selected) && !Disabled )
            {
                _spriteBatch.DrawRectangle(elementBox, HoveredColor * opacity, 2);
            }

            // Si le bouton possède du texte
            if (HasText && font!=null)
            {
                // Taille du texte avec la police en cours
                Vector2 stringSize = font.MeasureString(Text);
                // Affichage du texte au milieu du bouton
                _spriteBatch.DrawString(font, Text, 
                    elementBox.Center.ToVector2() - stringSize / 2,
                    TextColor);
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

        /// <summary>
        /// Permet de modifier texte et police
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        public void SetText(String _text, SpriteFont _font)
        {
            this.Text = _text;
            this.font= _font;
            HasText = true;
        }
        public void SetInfoPopUp(InfoPopUp _info)
        {
            this.info = _info;
        }
        public void CanBeSelected()
        {
            this.canBeSelected = true;
        }
    }
}