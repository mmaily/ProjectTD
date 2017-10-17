using System;
using System.Collections.Generic;
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
using LibrairieTropBien.GUI;

namespace DowerTefense.Commons
{
    /// <summary>
    /// Jeu proprement dit
    /// </summary>
    public class GameEngine
    {
        #region === Déclaration des variables ===

        // Unitées de base
        public Dictionary<string, string> UnitSpawned { get; private set; }
        public List<Building> Dummies { get; set; }

        // Bâtiments
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
        public object CustomContentManager { get; private set; }

        // Projectiles
        public List<Projectile> projectiles;

        // Unités
        public bool newWave;
        public int timeSince;
        public List<Unit> mobs;

        // Vagues
        public double lastWaveTick;
        public byte waveCount;
        public int waveLength;

        // Carte
        public byte tileSize;
        public Map map;

        #region # Dictionnaires des changements #
        //Le translator utilise ce dictionnaire pour transmettre les info entre Client et serveur
        public Dictionary<Dictionary<String, object>, bool> Changes;
        //Celui là sert à pouvoir réinitialiser le premier plus vite
        public Dictionary<Dictionary<String, object>, bool> Initial;
        //Mini-dictionnaires
        public Dictionary<String, object> DdefensePlayer;
        public Dictionary<String, object> DattackPlayer;
        public Dictionary<String, object> DLockedBuildingsList;
        public Dictionary<String, object> DDefenseBuildingsList;
        public Dictionary<String, object> Dprojectiles;
        public Dictionary<String, object> DFreeBuildingsList;
        public Dictionary<String, object> DTowerWaiting;
        public Dictionary<String, object> DSpawnerWaiting;
        public Dictionary<String, object> Dmobs;
        public Dictionary<String, object> DnewWave;
        #endregion

        // Joueurs
        public DefensePlayer defensePlayer;
        public AttackPlayer attackPlayer;

        // Mode client / serveur
        private bool serverMode;

        #endregion

        /// <summary>
        /// Initialisation du jeu
        /// </summary>
        public GameEngine(bool _serverMode = false)
        {
            this.serverMode = _serverMode;
        }

        /// <summary>
        /// Initialisation du Game Engine
        /// </summary>
        public void Initialize()
        {
            #region===Initialisation des Joueurs===
            defensePlayer = new DefensePlayer();
            attackPlayer = new AttackPlayer();
            #endregion
            #region===Map===
            map = new Map();
            MapEngine.ComputePath(ref map);
            #endregion
            #region ===Initialisation des bâtiments===
            LockedBuildingsList = new List<SpawnerBuilding>();
            FreeBuildingsList = new List<SpawnerBuilding>();
            DefenseBuildingsList = new List<Building>();
            WaitingForConstruction = new List<Building>();
            #endregion
            #region===Initialise le dictionnaire des changements===
            //Ces mini-dicionnaire contiennent l'objet qui à changé et son nom
            //De cette façon les Translators sont standardisés
            DdefensePlayer = new Dictionary<String, object>
            {
                { "defensePlayer", defensePlayer }
            };
            DattackPlayer = new Dictionary<String, object>
            {
                { "attackPlayer", attackPlayer }
            };
            DLockedBuildingsList = new Dictionary<String, object>
            {
                { "LockedBuildingsList", LockedBuildingsList }
            };
            DDefenseBuildingsList = new Dictionary<String, object>
            {
                { "DefenseBuildingsList", DefenseBuildingsList }
            };
            Dprojectiles = new Dictionary<String, object>
            {
                { "projectiles", projectiles }
            };
            DFreeBuildingsList = new Dictionary<String, object>
            {
                { "FreeBuildingsList", FreeBuildingsList }
            };
            DTowerWaiting = new Dictionary<String, object>
            {
                { "newTower", null }
            };
            DSpawnerWaiting = new Dictionary<String, object>
            {
                { "newSpawner", null }
            };
            Dmobs = new Dictionary<String, object>()
            {
                { "mobs", Dmobs }
            };
            DnewWave = new Dictionary<string, object>()
            {
                { "newWave", lastWaveTick }
            };

            //Dictionnaire de suivi des changements
            Changes = new Dictionary<Dictionary<String, object>, bool>
            {
                { DdefensePlayer, false },
                { DattackPlayer, false },
                { DLockedBuildingsList, false },
                { DDefenseBuildingsList, false },
                { Dprojectiles, false },
                { DFreeBuildingsList, false },
                { DTowerWaiting, false },
                { DSpawnerWaiting, false },
                { Dmobs, false },
                { DnewWave, false },
            };

            Initial = new Dictionary<Dictionary<string, object>, bool>(Changes);
            #endregion
            #region===Initialisation des listes Dummies===
            //SetSpawnerDictionnary();
            #endregion
            #region===Initialisation des unités, projectiles et vagues===
            mobs = new List<Unit>();
            projectiles = new List<Projectile>();
            lastWaveTick = 0;
            waveCount = 0;
            tileSize = 8;
            waveLength = 10 * 1000;
            #endregion

        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Remise du dictionnaire des changement en état
            //TODO : Boucle assez lourde... Peut être multitread si ça ralentit
            Changes = new Dictionary<Dictionary<string, object>, bool>(Initial);

            // Durée depuis ancien tic de vague
            timeSince = (int)(gameTime.TotalGameTime.TotalMilliseconds - lastWaveTick);
            newWave = false;
            // Partie uniquement pour le serveur : bâtiments construction + vagues
            if (serverMode)
            {
                // Construire la liste des tours en attente
                foreach (Building bd in WaitingForConstruction)
                {
                    // Différence Tour (défense) / Spawner (attaque)
                    if (bd is Tower)
                    {
                        // Si le joueur possède assez d'argent
                        if(defensePlayer.totalGold >= bd.Cost)
                        {
                            //Why ? Osef ! Tower t = (Tower)bd.DeepCopy();
                            Tower t = (Tower)bd;
                            // Retrait du coût du bâtiment
                            defensePlayer.totalGold -= t.Cost;
                            // Ajout à la liste
                            DefenseBuildingsList.Add(t);
                            // Ajout sur la tuile
                            bd.GetTile().building = t;
                            // Notification de changement
                            Changes[DDefenseBuildingsList] = true;
                            Changes[DdefensePlayer] = true;
                        }
                        // On peut passer directement au bâtiment suivant
                        continue;
                    }
                    if (bd is SpawnerBuilding)
                    {
                        // Si le joueur possède assez d'argent
                        if(attackPlayer.totalGold >= bd.Cost)
                        {
                            //On le cast en spawner pour appliquer les méthodes propres aux spawner
                            SpawnerBuilding spawner = (SpawnerBuilding)bd;
                            // Retrait du coût
                            attackPlayer.totalGold -= bd.Cost;
                            // Ajout à la liste
                            FreeBuildingsList.Add(spawner);

                            // Activation automatique si assez d'énergie
                            if (attackPlayer.totalEnergy - attackPlayer.usedEnergy >= (spawner.PowerNeeded))
                            {
                                spawner.powered = true;
                                attackPlayer.usedEnergy += spawner.PowerNeeded;
                            }
                            // Notification de changement
                            Changes[DFreeBuildingsList] = true;
                            Changes[DattackPlayer] = true;
                        }
                        // On peut passer directement au bâtiment suivant
                        continue;
                    }

                }
                //Une fois traitée, on vide les éléments de la waiting List
                WaitingForConstruction.Clear();

                // Vagues

                // Calcul du cycle de 30 secondes
                // Si le tic est vieux de 30 secondes
                if (timeSince > waveLength)
                {
                    // Vague suivante
                    waveCount++;
                    
                    // Nouvelle vague
                    newWave = true;

                    // Info changement
                    Changes[DnewWave] = true;

                    // Verrouillage des spawners 
                    LockSpawners();
                    // Notification de changement
                    Changes[DLockedBuildingsList] = true;
                }
            }

            // Si nouvelle vague :
            if (newWave)
            {
                // Sauvegarde horodatage
                lastWaveTick = gameTime.TotalGameTime.TotalMilliseconds;
            }

            // Mise à jour des unités
            int goldWon = 0;
            int livesLost = 0;
            bool mobsChanged = UnitEngine.ProcessMobs(ref mobs, gameTime, map.tileSize, ref goldWon, ref livesLost);
            if (mobsChanged && serverMode)
            {
                // Mise à jour de la liste des mobs
                Changes[Dmobs] = true;

                // Si changement état player, envoi
                if(goldWon > 0)
                {
                    defensePlayer.totalGold += goldWon;
                    Changes[DdefensePlayer] = true;
                }

                if(livesLost > 0)
                {
                    defensePlayer.lives -= livesLost;
                    Changes[DdefensePlayer] = true;
                }
            }

            // Mise à jour des tours et des projectiles
            projectiles.Clear();
            foreach (Building tower in DefenseBuildingsList)
            {
                Tower t = (Tower)tower;
                t.Update(gameTime, ref mobs);
                projectiles.AddRange(t.ProjectileList);
            }

            // Mise à jour des spawners
            foreach (SpawnerBuilding sp in LockedBuildingsList)
            {
                sp.Update(gameTime,map, ref mobs);
            }


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
            LockedBuildingsList.Clear();
            // Pour tous les spawners de la liste en paramètre
            foreach (SpawnerBuilding _sp in FreeBuildingsList.FindAll(sp => sp.powered))
            {
                SpawnerBuilding sp = (SpawnerBuilding)_sp.DeepCopy();
                sp.Lock();
                LockedBuildingsList.Add(sp);
            }

        }

    }
}
