using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DowerTefense.Commons.Units;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.Managers;
using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Game.Players;
using DowerTefense.Commons.GameElements.Projectiles;
using LibrairieTropBien.Network;
using DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings;

namespace DowerTefense.Commons
{
    /// <summary>
    /// Jeu proprement dit
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        #region Dictionaire dummies
        public Dictionary<string, string> UnitSpawned { get; private set; }
        #endregion
        #region === Buildings ===

        /// <summary>
        /// Liste de tous les bâtiments "locked"
        /// </summary>
        public List<SpawnerBuilding> LockedBuildingsList { get; set; }
        /// <summary>
        /// Liste de tous les bâtiments libre
        /// </summary>
        public List<SpawnerBuilding> FreeBuildingsList;
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
        public bool newWave;
        public int timeSince;
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
        #region===Dictionnaire des changements===
        //Le translator utilise ce dictionnaire pour transmettre les info entre Client et serveur
        public Dictionary<Dictionary<String, object>, bool> Changes;
        //Celui là sert à pouvoir réinitialiser le premier plus vite
        public Dictionary<Dictionary<String, object>, bool> Initial;
        //Mini-dictionnaire
        public Dictionary<String, object> DdefensePlayer;

        public Dictionary<String, object> DattackPlayer;

        public Dictionary<String, object> DLockedBuildingsList;

        public Dictionary<String, object> DDefenseBuildingsList;

        public Dictionary<String, object> Dprojectiles;

        public Dictionary<String, object> DFreeBuildingsList;

        public Dictionary<String, object> DWaitingForConstruction;

        public Dictionary<String, object> Dmobs;
        #endregion
        #region===Player===
        public DefensePlayer defensePlayer;
        public AttackPlayer attackPlayer;
        #endregion 
        /// <summary>
        /// Initialisation du jeu
        /// </summary>
        public GameEngine()
        {
        }


        public void Initialize()
        {
            #region===Map===
            map = new Map();
            #endregion
            #region===Initialise le dictionnaire des changements===
            //Ces mini-dicionnaire contiennent l'objet qui à changé et son nom
            //De cette façon les Translators sont standardisés
            DdefensePlayer = new Dictionary<String, object>();
            DdefensePlayer.Add("defensePlayer", defensePlayer);

            DattackPlayer = new Dictionary<String, object>();
            DattackPlayer.Add("attackPlayer", attackPlayer);

            DLockedBuildingsList = new Dictionary<String, object>();
            DLockedBuildingsList.Add("LockedBuildingsList", LockedBuildingsList);

            DDefenseBuildingsList = new Dictionary<String, object>();
            DDefenseBuildingsList.Add("DefenseBuildingsList", DefenseBuildingsList);

            Dprojectiles = new Dictionary<String, object>();
            Dprojectiles.Add("projectiles", projectiles);

            DFreeBuildingsList = new Dictionary<String, object>();
            DFreeBuildingsList.Add("FreeBuildingsList", FreeBuildingsList);

            DWaitingForConstruction = new Dictionary<String, object>();
            DWaitingForConstruction.Add("WaitingForConstruction", WaitingForConstruction);

            Dmobs = new Dictionary<String, object>();
            Dmobs.Add("mobs", Dmobs);
            //Dictionnaire de suivi des changements
            Changes = new Dictionary<Dictionary<String, object>, bool>();
            Changes.Add(DdefensePlayer, false);
            Changes.Add(DattackPlayer, false);
            Changes.Add(DLockedBuildingsList, false);
            Changes.Add(DDefenseBuildingsList, false);
            Changes.Add(Dprojectiles, false);
            Changes.Add(DFreeBuildingsList, false);
            Changes.Add(DWaitingForConstruction, false);
            Changes.Add(Dmobs, false);
            Initial = new Dictionary<Dictionary<string, object>, bool>(Changes);
            #endregion
            #region===Initialisation des listes Dummies===
            //SetSpawnerDictionnary();
            #endregion
            #region ===Initialisation des bâtiments===
            LockedBuildingsList = new List<SpawnerBuilding>();
            FreeBuildingsList = new List<SpawnerBuilding>();
            DefenseBuildingsList = new List<Building>();
            WaitingForConstruction = new List<Building>();
            #endregion
            #region===Initialisation des unités, projectiles et vagues===
            mobs = new List<Unit>();
            projectiles = new List<Projectile>();
            lastWaveTick = 0;
            waveCount = 0;
            tileSize = 8;
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
        public void Update(GameTime gameTime)
        {
            #region===On repasse le dictionnaire de changements à son état initial===
            //TODO : Boucle assez lourde... Peut être multitread si ça ralentit
            Changes = new Dictionary<Dictionary<string, object>, bool>(Initial);
            #endregion

            #region === Calcul des vagues ===

            // Calcul du cycle de 30 secondes
            newWave = false;
            // Durée depuis ancien tic
            timeSince = (int)(gameTime.TotalGameTime.TotalMilliseconds - lastWaveTick);
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
            //Cette méthode renvoie les gold gagnés à cet update + fais bouger les unités
            int gold = UnitEngine.ProcessMobs(ref mobs, gameTime, map.tileSize);
            if (gold != 0)
            {
                defensePlayer.totalGold += gold;
                Changes[DdefensePlayer] =true;
            }
            
            #endregion
            #region ===Update des tours et liste de projectile ===
            projectiles.Clear();
            foreach (Building tower in DefenseBuildingsList)
            {
                Tower t = (Tower)tower;
                t.Update(gameTime, mobs);
                projectiles.AddRange(t.projectileList);
            }
            #endregion
            #region===Lock des spawner===
            if (newWave == true)
            {
                LockSpawners();
                
            }
            #endregion 
            #region ====Construction des bâtiments en attente====
            //Construire la liste des tours en attente
            foreach (Building bd in WaitingForConstruction)
            {
                //// Retrait du coût du bâtiment
                //if (bd.GetType() == typeof(Tower))
                //{
                //    bd.CreateOnEventListener();
                //    InfoPopUp info = new InfoPopUp(new Rectangle((int)((bd.GetTile().getTilePosition().X - 0.5) * map.tileSize),
                //                                        (int)((bd.GetTile().getTilePosition().Y - 0.5) * map.tileSize),
                //                                        map.tileSize, map.tileSize))
                //    {
                //        Name = bd.GetType().ToString() + "Info",
                //        Tag = "InfoPopUp",
                //        font = CustomContentManager.GetInstance().Fonts["font"],
                //        texture = CustomContentManager.GetInstance.Colors["pixel"],
                //        Enabled = true
                //    };
                //    UIManager.GetInstance().UIElementsList.Add(info);
                //    bd.SetInfoPopUp(info);
                //    UIManager.GetInstance().defensePlayer.totalGold -= bd.Cost;
                //}
                //if (bd.GetType() == typeof(SpawnerBuilding))
                //{
                //    //On le cast en spawner pour appliquer les méthodes propres aux spawner
                //    SpawnerBuilding spawner = (SpawnerBuilding)bd;
                //    UIManager.GetInstance().attackPlayer.totalGold -= bd.Cost;
                //    UIManager.GetInstance().UpdateBtnLists(spawner);
                //    BuildingEngine.GetInstance().FreeBuildingsList.Add(spawner);
                //    if (UIManager.GetInstance().attackPlayer.totalEnergy - UIManager.GetInstance().attackPlayer.usedEnergy >= (spawner.PowerNeeded))
                //    {
                //        spawner.powered = true;
                //        UIManager.GetInstance().attackPlayer.usedEnergy += spawner.PowerNeeded;
                //    }
                //}

            }
            //Une fois traitée, on vide les éléments de la waiting List
            WaitingForConstruction.Clear();
            #endregion 
            //TODO : Apelle les bâtiments à faire leur actions respectives (si il y a des buildings)

        }


        //public void SetSpawnerDictionnary()
        //{
        //    UnitSpawned = new Dictionary<String, String>();
        //    UnitSpawned.Add("BasicSpawner", "Unit");
        //}

        public Boolean SetDummyEntities(List<Message> Messages)
        {
            Boolean success = false;
            foreach (Message _message in Messages)
            {
                //Entity entity = (Entity)_message.received;
                //DummyEntity.Add(entity);
                //success = DummyEntity.Count != 0 ? true : false;

            }
            return success;
        }


        /// <summary>
        /// Retourne une liste contenant les spawners verrouillés pour la prochaine vague
        /// </summary>
        /// <param name="spawners">Liste de tous les spawners du joueur</param>
        /// <returns>Liste des spawners actifs</returns>
        public void LockSpawners()
        {
            // Init de la liste de retour
            List<SpawnerBuilding> lockedSpawners = new List<SpawnerBuilding>();

            // Pour tous les spawners de la liste en paramètre
            foreach (SpawnerBuilding sp in FreeBuildingsList.FindAll(sp => sp.powered))
            {
                lockedSpawners.Add((SpawnerBuilding)sp.DeepCopy());
            }

        }

    }
}
