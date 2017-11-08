﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Game.Players
{
    [Serializable]
    public class AttackPlayer
    {
        public int totalEnergy;
        public int usedEnergy;
        public int totalGold;
        public int betGold;

        public AttackPlayer()
        {
            this.totalEnergy = 10;
            this.usedEnergy = 0;
            this.totalGold = 1000;
            this.betGold = 1000;

        }

    }
}
