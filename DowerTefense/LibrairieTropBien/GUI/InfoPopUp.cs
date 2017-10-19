using C3.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.GUI
{
    //TODO : Retravailler les pop Up pour qu'il n'y en ai qu'une qui s'actualise
    public class InfoPopUp : GuiElement
    {
        private Boolean hovered = false;
        Rectangle area;
        private Color defaultActive = Microsoft.Xna.Framework.Color.LightBlue;
        public InfoPopUp(int _x, int _y, int _width, int _height) : base(_x, _y, _width, _height)
        {
            this.ElementColor = Microsoft.Xna.Framework.Color.White;
        }
        public InfoPopUp(Rectangle _area )
        {
            this.ElementColor = Microsoft.Xna.Framework.Color.White;
            this.elementBox = new Rectangle();
            this.area = _area;
        }
        public override void Update()
        {
            // Récupération de l'état de la souris
            MouseState mouseState = Mouse.GetState();
            // Si la souris est sur le bouton
            if (this.area.Contains(mouseState.Position)&&Enabled)
            {
                // On l'enregistre
                hovered = true;
                elementBox.X = mouseState.X;
                elementBox.Y = mouseState.Y;
            }
            else
            {
                // On l'enregistre
                hovered = false;
            }
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            // Si la texture est définie
            if (hovered)
            {
                _spriteBatch.Draw(texture,this.elementBox, ElementColor);
                _spriteBatch.DrawString(font, text, elementBox.Location.ToVector2()+new Vector2(leftMargin,topMargin), Color.Azure);
            }

        }
        public override void setText(String _text)
        {
            HasText = true;
            text = _text;
            stringSize = font.MeasureString(text);
            this.elementBox = new Rectangle(0, 0, (int)stringSize.X+2*leftMargin, (int)stringSize.Y+2*topMargin);
        }
    }
}