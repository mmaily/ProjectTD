
using DowerTefense.Server.Elements;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

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


        /// <summary>
        /// Constructeur de base du serveur de jeu
        /// </summary>
        public GameServer(Dictionary<Client, Player> _clients)
        {
            // Récupération de la liste des clients
            this.clients = _clients;
            foreach (Client c in clients.Keys)
            {
                // Changement du callback
                c.ReceiveDataCallback = this.OnReiceivedData;
                c.SetupReceiveCallback(this);
            }
            Requests = new List<Message>();
            // Création du jeu, on lui file client et liste des requêtes
            using (var game = new GameManager(clients, ref Requests))
                game.Run();
        }

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        protected override void ProcessMessage(Message _messageReceived, Client _client)
        {
            //On lock pour éviter les accès concurrentiels
            lock (Requests)
            {
                Requests.Add(_messageReceived);
            }
        }

        ///<summary>
        ///Méthode déclenché par un événement worth mentionning dans le jeu
        ///Elle envoie cet événement au Translator qui décide quoi en faire (Envoyer, à qui ?)
        ///</summary>

        
    }
}
