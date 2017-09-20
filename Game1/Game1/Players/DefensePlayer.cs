using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGame.Players
{
    class DefensePlayer
    {
        public int startingGold=200;
        public int totalGold { get; set; }

        public DefensePlayer()
        {
            this.totalGold = startingGold;
        }
        public void Update(GameTime gameTime)
        {

        }
    }
}
