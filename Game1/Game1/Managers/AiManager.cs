using DowerTefenseGame.Units.Buildings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGame.Managers
{
    class AiManager
    {
        public int WaveNumber = 0;
        public double WaveTime;
        private static AiManager instance;
        public AiManager()
        {
            this.WaveTime = 15;
        }
        /// <summary>
        /// Récupération de l'instance du gestionnaire d'Ai
        /// </summary>
        /// <returns></returns>
        public static AiManager GetInstance()
        {
            if (instance == null)
            {

                instance = new AiManager();

            }
            return instance;
        }
        public void Update(GameTime _gameTime)
        {
            if ((int)_gameTime.TotalGameTime.TotalSeconds / WaveTime > WaveNumber)
            {
                SpawnerBuilding sp = new SpawnerBuilding();
                sp.NbreOfInstantSpawn = 1;
                WaveNumber++;
            }

        }
    }
}
