using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowerTefenseGameServer.Elements;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;

namespace DowerTefenseGameServer.Servers
{
    /// <summary>
    /// Classe gérant le lobby entre deux joueurs
    /// </summary>
    public class LobbyServer : Server
    {
        /// <summary>
        /// Liste des joueurs du lobby
        /// </summary>
        private Dictionary<Client, Player> clients;

        // Serveur d'authentification
        private AuthentificationServer authServer;

        /// <summary>
        /// Consttructeur avec un ou plusieurs clients
        /// </summary>
        /// <param name="_player1"></param>
        /// <param name="_player2"></param>
        public LobbyServer(AuthentificationServer _authServer)
        {
            this.authServer = _authServer;
        }

        /// <summary>
        /// Ajout d'un joueur au lobby
        /// </summary>
        /// <param name="_client"></param>
        public void AddPlayer(Client _client)
        {
            // Création du nouveau joueur
            Player newPlayer = new Player()
            {
                Name = _client.Name,
            };

            // Envoi au joueur l'info de la création du lobby
            _client.Send("newLobby", "");
            // Pour tous les joueurs déjà présents
            Parallel.ForEach(clients, other =>
            {
                // Info du nouveau joueur
                other.Key.Send("newPlayer", newPlayer);
                // Info des joueurs déjà présents
                _client.Send("newPlayer", other.Value.Name);
            });
            // Ajout du joueur
            clients.Add(_client, newPlayer);
        }


        /// <summary>
        /// Retrait d'un jouer au lobby
        /// </summary>
        /// <param name="_client"></param>
        public void RemovePlayer(Client _client)
        {
            // Retrait du client
            clients.Remove(_client);
            // Pour tous les autres, info du départ
            Parallel.ForEach(clients, other =>
            {
                other.Key.Send("removeOpponant", _client.Name);
            });
        }

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="messageReceived"></param>
        /// <param name="client"></param>
        protected override void ProcessMessage(Message messageReceived, Client client)
        {
            // Selon le sujet du message
            switch (messageReceived.Subject)
            {
                case "ready":
                    // Le client modifie son état "ready"
                    clients[client].Ready = messageReceived.received.Equals("ok") ? true : false;
                    // Vérification de l'état global
                    UpdateClients(client);
                    break;
                default:
                    break;
            }
        }

        private void UpdateClients(Client _clientModified)
        {
            // Tous prêts ?
            bool allReady = true;

            // Pour tous les clients
            Parallel.ForEach(clients, other =>
            {
                // Info de la modification
                other.Key.Send("update", clients[_clientModified]);
                // On regarde l'état prêt
                allReady = allReady && other.Value.Ready;
            });

            // Si tout le monde est prêt
            if (allReady)
            {
                // Ouais !
            }
        }
    }
}
