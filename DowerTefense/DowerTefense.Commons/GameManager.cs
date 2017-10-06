using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.Units.Buildings;
using DowerTefense.Commons.Units;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Managers;
using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Game.Players;
using DowerTefense.Commons.GameElements.Projectiles;

namespace DowerTefense.Server.Servers
{
    /// <summary>
    /// Jeu à proprement dit, côté serveur
    /// </summary>
    public class GameManager : Microsoft.Xna.Framework.Game
    {

        #region === Buildings ===

        /// <summary>
        /// Liste de tous les bâtiments "locked"
        /// </summary>
        public List<SpawnerBuilding> LockedBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments libre
        /// </summary>
        public List<SpawnerBuilding> FreeBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments défensifs
        /// </summary>
        public List<Building> DefenseBuildingsList { get; set; }
        /// <summary>
        /// Liste de construction en attente pour le prochain update
        /// </summary>
        public List<Building> WaitingForConstruction { get; set; }

        #endregion
        #region===Projectiles===
        public List<Projectile> projectiles;
        #endregion
        #region ===Unités
        public List<Unit> mobs;
        #endregion
        #region===Waves===
        public double lastWaveTick;
        public byte waveCount;
        public int waveLength;
        #endregion
        #region===Map====
        public byte tileSize;
        public Map map;
        #endregion
        #region===Player===
        public DefensePlayer defensePlayer;
        public AttackPlayer attackPlayer;
        #endregion
        /// <summary>
        /// Initialisation du jeu
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            #region ===Initialisation des bâtiments===
            LockedBuildingsList = new List<SpawnerBuilding>();
            FreeBuildingsList = new List<SpawnerBuilding>();
            DefenseBuildingsList = new List<Building>();
            WaitingForConstruction = new List<Building>();
            #endregion
            #region===Initialisation des vagues===
            lastWaveTick = 0;
            waveCount = 0;
            tileSize = 8;
            map = new Map();
            #endregion
            #region===Initialisation des Joueurs===
            defensePlayer = new DefensePlayer();
            attackPlayer = new AttackPlayer();
            #endregion
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            #region === Calcul des vagues ===

            // Calcul du cycle de 30 secondes
            bool newWave = false;
            // Durée depuis ancien tic
            int timeSince = (int)(gameTime.TotalGameTime.TotalMilliseconds - lastWaveTick);
            // Si le tic est vieux de 30 secondes
            if (timeSince > waveLength)
            {
                // Vague suivante
                waveCount++;
                // Sauvegarde horodatage
                lastWaveTick = gameTime.TotalGameTime.TotalMilliseconds;
                // Nouvelle vague
                newWave = true;

            }
            #endregion
            #region ===Update des unités ===
            base.Update(gameTime);
            //Cette méthode renvoie les gold gagnés à cet update + fais bouger les unités
            defensePlayer.totalGold += UnitEngine.ProcessMobs(ref mobs, gameTime, map.tileSize);
            #endregion
            #region ===Update des tours et liste de projectile ===
            projectiles.Clear();
            Parallel.ForEach(DefenseBuildingsList, tower =>
            {
                Tower t = (Tower)tower;
                tower.Update();
                projectiles.AddRange(t.projectileList);
            });
            #endregion

            #region =====Construction des bâtiments en attente=====
            //Construire la liste des tours en attente
            foreach (Building bd in WaitingForConstruction)
            {
                // Retrait du coût du bâtiment
                if (bd.GetType() == typeof(Tower))
                {
                    bd.CreateOnEventListener();
                    InfoPopUp info = new InfoPopUp(new Rectangle((int)((bd.GetTile().getTilePosition().X - 0.5) * map.tileSize),
                                                        (int)((bd.GetTile().getTilePosition().Y - 0.5) * map.tileSize),
                                                        map.tileSize, map.tileSize))
                    {
                        Name = bd.GetType().ToString() + "Info",
                        Tag = "InfoPopUp",
                        font = CustomContentManager.GetInstance().Fonts["font"],
                        texture = CustomContentManager.GetInstance.Colors["pixel"],
                        Enabled = true
                    };
                    UIManager.GetInstance().UIElementsList.Add(info);
                    bd.SetInfoPopUp(info);
                    UIManager.GetInstance().defensePlayer.totalGold -= bd.Cost;
                }
                if (bd.GetType() == typeof(SpawnerBuilding))
                {
                    //On le cast en spawner pour appliquer les méthodes propres aux spawner
                    SpawnerBuilding spawner = (SpawnerBuilding)bd;
                    UIManager.GetInstance().attackPlayer.totalGold -= bd.Cost;
                    UIManager.GetInstance().UpdateBtnLists(spawner);
                    BuildingEngine.GetInstance().FreeBuildingsList.Add(spawner);
                    if (UIManager.GetInstance().attackPlayer.totalEnergy - UIManager.GetInstance().attackPlayer.usedEnergy >= (spawner.PowerNeeded))
                    {
                        spawner.powered = true;
                        UIManager.GetInstance().attackPlayer.usedEnergy += spawner.PowerNeeded;
                    }
                }

            }
            //Une fois traitée, on vide les éléments de la waiting List
            WaitingForConstruction.Clear();
            #endregion 
            //TODO : Apelle les bâtiments à faire leur actions respectives (si il y a des buildings)

        }

        //TODO : Créer une méthode appelée à la mort d'une unité






    }
}
