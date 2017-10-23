using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DownerTefense.Game.Managers
{
    public class UserInputManager
    {
        public event EventHandler KeyPressed;
        public Boolean keyKressed = false;
        KeyboardState state;

        public UserInputManager()
        {

        }
        public void Update()
        {
            // Poll for current keyboard state
            state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if((state.GetPressedKeys()).Length > 0)
            {
                this.OnKeyPressed(new EventArgs());
            }


        }
        protected virtual void OnKeyPressed(EventArgs e)
        {
            EventHandler handler = KeyPressed;
            if (handler != null)
                handler(state.GetPressedKeys(), e);
        }
    }
}
