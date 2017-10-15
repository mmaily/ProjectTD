using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DowerTefense.Server.Elements;
using LibrairieTropBien.Network;

/// <summary>
/// Mieux mieux mieux ! https://www.codeproject.com/Articles/1608/Asynchronous-socket-communication
/// </summary>

namespace DowerTefense.Server.Servers
{
    /// <summary>
    /// Serveur d'authentification
    /// </summary>
    public class AuthentificationServer : Server
    {
        // Port du serveur de connexion
        private const string localIP = "127.0.0.1";
        private const string DistantIP = "5.51.40.64";
        private const int portNum = 42666;

        // Paramètres
        // Taille maximale de la file d'attente
        private const byte maxQueueLenght = 50;

        // Liste des clients connectés
        // TODO : Liste ou dictionnaire ?
        private List<Client> connectedClients;
        // Liste des clients en recherche de match
        private Dictionary<string, List<Client>> matchmakingClients;
        private object matchmakingListLock;

        // Liste des lobbiess
        private List<LobbyServer> lobbies;

        /// <summary>
        /// Constructeur
        /// </summary>
        public AuthentificationServer()
        {
            // Création de la liste des clients connectés
            connectedClients = new List<Client>();
            matchmakingClients = new Dictionary<string, List<Client>>();
            matchmakingClients.Add("Attack", new List<Client>());
            matchmakingClients.Add("Defense", new List<Client>());
            matchmakingListLock = new object();

            // Création de la liste des lobbys
            lobbies = new List<LobbyServer>();

            // Adress IP locale
            IPAddress local = IPAddress.Any;

            // Création d'un socket TCP/IP
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            // Liaison du socket avec l'IP locale et le port
            listener.Bind(new IPEndPoint(local, portNum));
            // Mise en écoute
            listener.Listen(maxQueueLenght);

            // Début de l'acceptation des clients et mise en place du callback
            listener.BeginAccept(new AsyncCallback(this.OnConnectRequest), listener);
        }

        #region === Connexion ===

        /// <summary>
        /// Callback à la demande de connexion d'un client
        /// </summary>
        /// <param name="_ar"></param>
        public void OnConnectRequest(IAsyncResult _ar)
        {
            // Récupération socket auditeur
            Socket listener = (Socket)_ar.AsyncState;
            // Socket client connecté
            Socket client = listener.EndAccept(_ar);
            // Gestion de la connexion
            NewConnection(client);

            // On accepte à nouveau un client
            listener.BeginAccept(new AsyncCallback(OnConnectRequest), listener);
        }

        /// <summary>
        /// Gestion de la nouvelle connexion : ajout à la liste
        /// </summary>
        /// <param name="_socket"></param>
        public void NewConnection(Socket _socket)
        {
            // Création d'un nouveau client connecté
            Client client = new Client(_socket);
            connectedClients.Add(client);

            // TODO : Vérifier nom unique

            // Récupération de l'horodatage
            client.ConnectedSince = DateTime.Now;

            // Changement d'état pour ce client
            client.state = MultiplayerState.Connected;

            // Création du callback de réception
            client.SetupReceiveCallback(this);

            // Abonnement à ce client
            client.MessageReceived += ProcessMessage;
        }

        #endregion

        #region === Réception ===

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_client"></param>
        /// <param name="_messageReceived"></param>
        protected override void ProcessMessage(Client _client, Message _messageReceived)
        {
            // Traitement des différents cas
            switch (_messageReceived.Subject)
            {
                case "login":
                    // Le client demande à se connecter avec ce pseudo
                    _client.Name = (string)_messageReceived.received;
                    // Le client est maintenant authentifié
                    _client.state = MultiplayerState.Authentified;
                    // Info console
                    Console.WriteLine("Client {0} s'est connecté", _client.Name);
                    // Envoi de la confirmation client
                    _client.Send("login", "ok");
                    break;
                case "matchmaking":
                    // Le client demande à rechercher un match en ligne
                    // Si c'est déjà le cas
                    if(_client.state == MultiplayerState.SearchingGame)
                    {
                        break;
                    }
                    // Etat du client
                    _client.state = MultiplayerState.SearchingGame;
                    // Envoi confirmation recherche au client
                    _client.Send("matchmaking", "searching");
                    // Lancement d'une recherche de match
                    this.ProcessMatchmaking(_client, (string)_messageReceived.received);
                    // Désabonnement au client (putain, 6€ par mois quoi !)
                    _client.MessageReceived -= ProcessMessage;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Recherche un match compatible pour ce client
        /// </summary>
        /// <param name="_client"></param>
        private void ProcessMatchmaking(Client _client, string _role)
        {
            // Info console
            Console.WriteLine("Joueur " + _client.Name + " recherche un match en tant que " + _role);

            // Match trouvé ou non
            bool matchFound = false;
            // Opposant compatible
            Client opponant = null;

            string opponantRole = _role.Equals("Attack") ? "Defense" : "Attack";

            // Verrouillage pour accès concurentiel
            lock (matchmakingListLock)
            {
                // Parcours de la liste des clients recherchant un match
                foreach (Client c in matchmakingClients[opponantRole])
                {
                    // TODO : ELO searching
                    if (true)
                    {
                        // Match trouvé
                        matchFound = true;
                        // Sauvegarde opposant
                        opponant = c;
                        break;
                    }
                }

                // Si aucun match n'a été trouvé
                if (matchFound)
                {
                    // Lancement du match entre les deux joueurs compatibles
                    LobbyServer lobby = new LobbyServer(this);
                    // Ajout des joueurs
                    lobby.AddPlayer(_client, _role);
                    lobby.AddPlayer(opponant, opponantRole);
                    // Retrait de l'opposant de la liste des clients en recherche de match
                    matchmakingClients[opponantRole].Remove(opponant);

                    // Info console
                    Console.WriteLine("Match créé entre " + _client.Name + " et " + opponant.Name + ". GLHF !");
                }
                else
                {
                    // On ajoute le client à la liste
                    matchmakingClients[_role].Add(_client);
                }
            }


        }

        #endregion
    }



}
