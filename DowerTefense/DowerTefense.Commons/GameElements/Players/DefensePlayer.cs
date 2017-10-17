using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Game.Players
{
    [Serializable]
    public class DefensePlayer
    {
        public int startingGold=200;
        public int totalGold { get; set; }
        public int lives;

        public DefensePlayer()
        {
            this.totalGold = startingGold;
            this.lives = 20;
        }
        public void Update(GameTime gameTime)
        {

        }
    }
}
