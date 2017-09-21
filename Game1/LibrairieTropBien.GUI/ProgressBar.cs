using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using Microsoft.Xna.Framework;

namespace LibrairieTropBien.GUI
{
    /// <summary>
    /// Classe d'affichage d'un rectange se remplissant
    /// </summary>
    public class ProgressBar : GuiElement
    {
        // Maximum de remplissage
        public float Max { get; set; }
        // Etat actuel
        public float State { get; set; }
        // Mode vertical ou horizontal
        public bool HorizontalMode { get; set; }
        // Couleur de remplissage
        public Color FillColor { get; set; }


        /// <summary>
        /// Barre de progression positionnée et avec un maximum (facultatif)
        /// </summary>
        /// <param name="_x">Position X</param>
        /// <param name="_y">Position Y</param>
        /// <param name="_width">Largeur</param>
        /// <param name="_height">Hauteur</param>
        /// <param name="_max">Maximum (défaut 1f)</param>
        public ProgressBar(int _x, int _y, int _width, int _height, float _max = 1f) : base (_x, _y, _width, _height)
        {
            this.Max = _max;
            this.State = 0f;
            this.HorizontalMode = true;
            this.ElementColor = Color.Black;
            this.FillColor = Color.Red;
        }


        /// <summary>
        /// Affichage de l'élément
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            // Si le bouton n'est pas activé, sortie rapide
            if (!this.Enabled)
            {
                return;
            }

            // Affichage du contour
            _spriteBatch.DrawRectangle(this.elementBox, this.ElementColor);

            // Rectangle de remplissage
            Rectangle fillRectangle = this.elementBox;
            // Ratio de remplissage
            float ratio = this.State / this.Max;
            // Selon le mode
            if (HorizontalMode)
            {
                fillRectangle.Width = (int)(ratio * elementBox.Width);
            } else {
                fillRectangle.Height = (int)(ratio * elementBox.Height);
            }

            // Affichage du remplissage
            _spriteBatch.FillRectangle(fillRectangle, FillColor);

        }

        /// <summary>
        /// Mise à jour de l'élément
        /// </summary>
        public override void Update()
        {
        }
    }
}
