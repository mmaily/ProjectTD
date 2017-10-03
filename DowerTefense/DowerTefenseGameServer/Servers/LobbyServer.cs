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
        /// Constructeur
        /// </summary>
        public LobbyServer(AuthentificationServer _authServer)
        {
            // Récupération du serveur d'authentification
            this.authServer = _authServer;
            // Initialisation de la liste des clients
            clients = new Dictionary<Client, Player>();

            // Récupération des évènements de réception
        }

        /// <summary>
        /// Ajout d'un joueur au lobby
        /// </summary>
        /// <param name="_client"></param>
        public void AddPlayer(Client _client, string _role)
        {
            // Vérification de l'unicité du client
            if (clients.ContainsKey(_client))
            {
                return;
            }

            // Envoi au joueur l'info de la création du lobby
            _client.Send("newLobby", "");
            // Changement de l'état du client
            _client.state = MultiplayerState.InLobby;

            // Modification du destinataire du callback
            _client.ReceiveDataCallback = this.OnReiceivedData;
            // Création du callback de réception
            _client.SetupReceiveCallback(this);

            // Création du nouveau joueur
            Player newPlayer = new Player()
            {
                Name = _client.Name,
                Role = _role.Equals("Attack") ? PlayerRole.Attacker : PlayerRole.Defender,
            };

            // Envoi de lui même
            _client.Send("playerUpdate", newPlayer);

            // Pour tous les joueurs déjà présents
            Parallel.ForEach(clients, other =>
            {
                // Info du nouveau joueur
                other.Key.Send("playerUpdate", newPlayer);
                // Info des joueurs déjà présents
                _client.Send("playerUpdate", other.Value);
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
            // Debug
            Console.WriteLine("Mise à jour client : " + clients[_clientModified].Name + " est maintenant prêt : " + clients[_clientModified].Ready);

            // Tous prêts ?
            bool allReady = true;

            // Pour tous les clients
            Parallel.ForEach(clients, other =>
            {
                // Info de la modification
                other.Key.Send("playerUpdate", clients[_clientModified]);
                // On regarde l'état prêt
                allReady = allReady && other.Value.Ready;
            });

            // Si tout le monde est prêt
            if (allReady)
            {
                Parallel.ForEach(clients, c =>
                {
                    // Lancement du jeu
                    c.Key.Send("game", "starting");
                });

                GameServer gameServer = new GameServer(clients);
            }
        }
    }
}
