using C3.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LibrairieTropBien.GUI
{
    /// <summary>
    /// Classe mère de tous les éléments d'affichage
    /// </summary>
    public class GuiElement
    {

        /// <summary>
        /// Rectange définissant l'élément
        /// </summary>
        public Rectangle elementBox;
        /// <summary>
        /// Etat du bouton : désactivé ou non
        /// </summary>
        #region Affichage
        public Texture2D texture;
        protected string text = "";
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                HasText = text != null ? true : false;
            }
        }
        protected Boolean HasText = false;
        public SpriteFont font;
        public Vector2 stringSize;
        public Color TextColor { get; set; }
        private bool disabled;
        public bool Disabled
        {
            get { return disabled; }
            set
            {
                disabled = value;
                // Si désactivé, on grise le bouton aussi
                GreyedOut = disabled;
            }
        }
        public int leftMargin = 5;
        public int topMargin = 5;
        /// Couleur d'arrière plan
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Grisage de l'élément
        /// </summary>
        public bool GreyedOut { get => greyeyOut; set => greyeyOut = value; }
        private Boolean greyeyOut = false;
        /// <summary>
        /// Intensité du grisage
        /// </summary>
        public float Opacity { get => opacity; set => opacity = value; }
        private float opacity = 0.5f;
        /// <summary>
        /// Element à dessiner ou pas
        /// </summary>
        public Boolean Enabled { get; set; }
        #endregion
        /// <summary>
        /// Nom de l'élément
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Tag de l'élément (catégorie)
        /// </summary>
        public string Tag { get; set; }
        public Boolean PopUpAttached { get; set; }
        /// <summary>
        /// Couleur de l'élément
        /// </summary>
        protected Color ElementColor { get; set; }


        /// <summary>
        /// Contructeur
        /// </summary>
        public GuiElement()
        {
            //this.Graphics = new GraphicsDeviceManager();
        }

        /// <summary>
        /// Constructeur avec position
        /// </summary>
        public GuiElement(int _x, int _y, int _width, int _height)
        {
            this.elementBox = new Rectangle(_x, _y, _width, _height);
            this.Enabled = true;

        }

        /// <summary>
        /// Mise à jour de l'élément
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Affichage de l'élément
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public virtual void Draw(SpriteBatch _spriteBatch)
        {            // Si le bouton n'est pas activé, sortie rapide
            if (!this.Enabled)
            {
                return;
            }

            float opacity = this.GreyedOut ? this.Opacity : 1f;
            // Si la texture est définie
            if (texture != null)
            {
                _spriteBatch.Draw(texture, this.elementBox, ElementColor * opacity);
            }
            else
            {
                // Texture non définie, on affiche un rectangle
                _spriteBatch.DrawRectangle(this.elementBox, ElementColor * opacity);

                // Affichage de la couleur du fond
                _spriteBatch.FillRectangle(this.elementBox, BackgroundColor * opacity);
            }

            // Si le bouton possède du texte
            if (HasText && font != null)
            {
                // Taille du texte avec la police en cours
                _spriteBatch.DrawString(font, text, elementBox.Location.ToVector2() + new Vector2(leftMargin, topMargin), Color.Azure);

            }
        }
        public virtual void setText(String _text)
        {
            HasText = true;
            text = _text;
            stringSize = font.MeasureString(text);
        }
    }
}
