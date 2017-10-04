using DowerTefenseGame.GameElements.Units;
using DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefenseGame.Units;
using DowerTefenseGame.Units.Buildings;
using DowerTefenseGameServer.Elements;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGameServer.Servers
{
    /// <summary>
    /// Classe de serveur de jeu
    /// </summary>
    public class GameServer : Server
    {
        // Correspondance clients / joueurs
        private Dictionary<Client, Player> clients;
        #region Unité tirées des fichiers de config
        //Chemin d'accès menant aux dossiers contenant les fichiers de config
        private static String filePath = Path.Combine("Content");
        //Liste unités
        private List<Unit> DummyUnits;
        private String unitsPath = Path.Combine(filePath,"Units"); // Chemin d'accès des fichier de config unités
        //Liste tours
        private List<Tower> DummyTowers;
        private String towersPath = Path.Combine(filePath, "Towers");
        //Liste Spawner
        private List<SpawnerBuilding> DummySpawners;
        private String spawnersPath = Path.Combine(filePath, "Spawners");
        #endregion

        /// <summary>
        /// Constructeur de base du serveur de jeu
        /// </summary>
        public GameServer(Dictionary<Client, Player> _clients)
        {
            // Récupération de la liste des clients
            this.clients = _clients;
            Parallel.ForEach(clients, c =>
            {
                // Changement du callback
                c.Key.ReceiveDataCallback = this.OnReiceivedData;
                c.Key.SetupReceiveCallback(this);
            });
            //Listes pour envoyer les fichiers de cnfig aux clients
            DummySpawners = new List<SpawnerBuilding>();
            DummyTowers = new List<Tower>();
            DummyUnits = new List<Unit>();



        }

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        protected override void ProcessMessage(Message _messageReceived, Client _client)
        {
            // Traitement des différents cas
            switch (_messageReceived.Subject)
            {
                default:
                    break;
            }
        }
        //Génère la liste Bâtiments défensif, Bâtiments offensif et Entity
        protected void GenerateDummiesDictionnary()
        {
            //TODO : On désérialise
            #region Les unités
            //Clear mp for further usage.
            Unit unit = null;
            //Open the file written above and read values from it.
            Stream stream = File.Open(Path.Combine(unitsPath, "unit" + ".osl"), FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            unit = (Unit)bformatter.Deserialize(stream);
            stream.Close();
            DummyUnits.Add(unit);
            #endregion
            #region Tours
            Tower tower;
            foreach (Tower.NameEnum _tower in Enum.GetValues(typeof(Tower.NameEnum)))
            {
                stream = File.Open(Path.Combine(towersPath, _tower + ".osl"), FileMode.Create);
                bformatter = new BinaryFormatter();
                tower = (Tower)bformatter.Deserialize(stream);
                stream.Close();
                DummyTowers.Add(tower);
            }
            #endregion
            #region Spawners
            SpawnerBuilding sp;
            foreach (SpawnerBuilding.NameEnum _sp in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
            {
                sp = (SpawnerBuilding)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings." + _sp.ToString()));
                sp.DeleteOnEventListener();
                //Gestion de l'emplacement de sauvegarde des fichiers config unités, tours et spawner
                // Open a file and serialize the object into it in binary format.
                // EmployeeInfo.osl is the file that we are creating. 
                // Note: -you can give any extension you want for your file
                // If you use custom extensions, then the user will now
                //   that the file is associated with your program.
                stream = File.Open(Path.Combine(spawnersPath, _sp + ".osl"), FileMode.Create);
                bformatter = new BinaryFormatter();
                sp = (SpawnerBuilding)bformatter.Deserialize(stream);
                stream.Close();
                DummySpawners.Add(sp);
            }
            #endregion

        }
        protected void SendUnity()
        {
            foreach(Client c in clients.Keys)
            {
                // Envoie bât off et unités à l'attaquant
                if (clients[c].Role == PlayerRole.Attacker)
                {
                    c.Send("Unit", DummyUnits);
                    c.Send("Spawner", DummySpawners);  
                }
                // Envoie bât def et unités
                if (clients[c].Role == PlayerRole.Defender)
                {
                    c.Send("Unit", DummyUnits);
                    c.Send("Towers", DummyTowers);
                }
            }


        }
        protected void CreateSerializedEntities()
        {
            //Ici on utilise les classes des Unités, tours et spawner pour créer des objets sérialisés
            #region Unités
            Unit entity = new Unit();
           //Gestion de l'emplacement de sauvegarde des fichiers config unités, tours et spawner
        // Open a file and serialize the object into it in binary format.
        // EmployeeInfo.osl is the file that we are creating. 
        // Note: -you can give any extension you want for your file
        // If you use custom extensions, then the user will now
        //   that the file is associated with your program.
        Stream stream = File.Open(Path.Combine(unitsPath, entity.name + ".osl"), FileMode.Create);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Serialize(stream, entity);
        stream.Close();
            #endregion
            #region Tours
            Tower tower;
            foreach (Tower.NameEnum _tower in Enum.GetValues(typeof(Tower.NameEnum)))
                {
                    tower = (Tower)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units.Buildings.DefenseBuildings." + _tower.ToString()));
                    tower.DeleteOnEventListener();
                // Open a file and serialize the object into it in binary format.
                // EmployeeInfo.osl is the file that we are creating. 
                // Note: -you can give any extension you want for your file
                // If you use custom extensions, then the user will now
                //   that the file is associated with your program.
                stream = File.Open(Path.Combine(towersPath, tower.name + ".osl"), FileMode.Create);
                bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, tower);
                stream.Close();
            }
            #endregion
            #region Spawners
            SpawnerBuilding sp;
            foreach (SpawnerBuilding.NameEnum _sp in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
            {
                sp = (SpawnerBuilding)Activator.CreateInstance(Type.GetType("DowerTefenseGame.GameElements.Units.Buildings.AttackBuildings." + _sp.ToString()));
                sp.DeleteOnEventListener();
                //Gestion de l'emplacement de sauvegarde des fichiers config unités, tours et spawner
                // Open a file and serialize the object into it in binary format.
                // EmployeeInfo.osl is the file that we are creating. 
                // Note: -you can give any extension you want for your file
                // If you use custom extensions, then the user will now
                //   that the file is associated with your program.
                stream = File.Open(Path.Combine(spawnersPath, sp.name + ".osl"), FileMode.Create);
                bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, sp);
                stream.Close();
            }
            #endregion
        }

    }
}
