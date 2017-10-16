
using DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Commons.Units;
using DowerTefense.Server.Elements;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime;
using System.Threading;

namespace DowerTefense.Server.Servers
{
    /// <summary>
    /// Classe de serveur de jeu
    /// </summary>
    public class GameServer : Server
    {
        // Correspondance clients / joueurs
        private Dictionary<Client, Player> clients;
        private List<Message> Requests;

        public List<Building> Dummies { get; private set; }


        /// <summary>
        /// Constructeur de base du serveur de jeu
        /// </summary>
        public GameServer(ref Dictionary<Client, Player> _clients)
        {
            // Récupération de la liste des clients
            this.clients = _clients;
            foreach (Client c in clients.Keys)
            {
                // Abonnement au client
                c.MessageReceived += ProcessMessage;
            }
            Requests = new List<Message>();

            foreach (Client c in clients.Keys)
            {
                // Changement du callback
                c.Send("game", "starting");
            }

            GameManager game = new GameManager(_clients, ref Requests);

            game.Run();
        }
        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        public void ProcessMessage(Client _client, Message _messageReceived)
        {
            switch (_messageReceived.Subject)
            {
                case "DummiesRequest":
                    _client.Send("DummiesList", Dummies);
                    break;

                default:
                    //INGAME
                    //On lock pour éviter les accès concurrentiels
                    lock (Requests)
                    {
                        Requests.Add(_messageReceived);
                    }
                    break;
            }


        }

        public void GenerateDummies()
        {
            #region === Remplir le catalogue des unités de base==
            Dummies = new List<Building>();
            Building newBuilding;

            foreach (Tower.NameEnum tower in Enum.GetValues(typeof(Tower.NameEnum)))
            {
                newBuilding = (Building)Activator.CreateInstance(Assembly.Load("DowerTefense.Commons").GetType("DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings." + tower.ToString()));
                //newBuilding.DeleteOnEventListener();
                Dummies.Add(newBuilding);
            }
            foreach (SpawnerBuilding.NameEnum spawn in Enum.GetValues(typeof(SpawnerBuilding.NameEnum)))
            {
                newBuilding = (Building)Activator.CreateInstance(Assembly.Load("DowerTefense.Commons").GetType("DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings." + spawn.ToString()));
                //newBuilding.DeleteOnEventListener(); // On le "désactive" en le rendant désabonnant de son event listener d'action
                Dummies.Add(newBuilding);
            }
            #endregion

        }


    }
}
