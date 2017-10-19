using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.GUI
{
    public class ButtonArray : GuiElement
    {
        Button[,] array;
        public ButtonArray(int _x, int _y, int _columns, int _rows, Rectangle buttonSize)
        {
            this.elementBox = new Rectangle(_x, _y, _columns * buttonSize.X, _rows * buttonSize.Y);
            array = new Button[_rows, _columns];
            this.Enabled = true;
        }

        public void Add(Button button)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == null)
                    {
                        //On place le bouton à la bonne position dans le tableau
                        button.elementBox = new Rectangle
                                (this.elementBox.X + j * button.elementBox.Width,
                                 this.elementBox.Y + i * button.elementBox.Height,
                                 button.elementBox.Width, button.elementBox.Height);
                        array[i, j] = button;
                        return;
                    }
                }
            }
        }
        public Button[,] GetArray()
        {
            return array;
        }
        public override void Draw(SpriteBatch _sp)
        {
            if (!this.Enabled)
            {
                return;
            }
            foreach(Button b in array)
            {
                if (b != null)
                {
                    b.Draw(_sp);
                }
                else
                {
                    //Fin du tableau
                    return;
                }
            }
        }
        public override void Update()
        {
            foreach (Button b in array)
            {
                if (b != null)
                {
                    b.Update();
                }
                else
                {
                    //Fin du tableau
                    return;
                }
            }
        }
    }

}
