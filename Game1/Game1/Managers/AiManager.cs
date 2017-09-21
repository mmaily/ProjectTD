using DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings;
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
        private static AiManager instance;
        public AiManager()
        {
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
        public void Update(GameTime _gameTime, bool _newWave)
        {
            if (_newWave)
            {
                BasicSpawner sp = new BasicSpawner()
                {
                    NbreOfInstantSpawn = 1
                };
            }

        }
    }
}
