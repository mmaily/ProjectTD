using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.GUI
{
    /// <summary>
    /// Classe mère de tous les éléments d'affichage
    /// </summary>
    public abstract class GuiElement
    {
        /// <summary>
        /// Rectange définissant l'élément
        /// </summary>
        public Rectangle elementBox;
        /// <summary>
        /// Etat du bouton : désactivé ou non
        /// </summary>
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
        {

        }

    }
}
