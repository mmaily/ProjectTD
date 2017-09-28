using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DowerTefenseGameServer.Elements;
using LibrairieTropBien.Network;

/// <summary>
/// Mieux mieux mieux ! https://www.codeproject.com/Articles/1608/Asynchronous-socket-communication
/// </summary>

namespace DowerTefenseGameServer
{
    /// <summary>
    /// Serveur d'authentification
    /// </summary>
    public class AuthentificationServer : Server
    {
        // Port du serveur de connexion
        private const string localIP = "127.0.0.1";
        private const int portNum = 42666;

        // Paramètres
        // Taille maximale de la file d'attente
        private const byte maxQueueLenght = 50;

        private List<Client> connectedClients;

        /// <summary>
        /// Constructeur
        /// </summary>
        public AuthentificationServer()
        {
            // Création de la liste des clients connectés
            connectedClients = new List<Client>();

            // Adress IP locale
            IPAddress local = IPAddress.Parse(localIP);

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

            // Récupération de l'horodatage
            client.ConnectedSince = DateTime.Now;

            // Changement d'état pour ce client
            client.state = MultiplayerState.Connected;

            // Création du callback de réception
            client.SetupRecieveCallback(this);
        }

        /// <summary>
        /// Réception des données reçues par un client
        /// </summary>
        /// <param name="ar"></param>
        public override void OnReiceivedData(IAsyncResult ar)
        {
            // Récupération du client
            Client client = (Client)ar.AsyncState;
            // Données recues
            byte[] receivedData = client.GetRecievedData(ar);

            // Si aucune donnée n'a été reçue, la connexion est probablement fermée
            if (receivedData.Length > 0)
            {
                client.SetupRecieveCallback(this);
                
                // Récupération du message
                Message messageReceived = new Message(receivedData);
                ProcessMessage(messageReceived, client);
            }
            else
            {
                Console.WriteLine("Client " + client.Name + " déconnecté.");
                // Fermeture du socket
                client.AuthSocket.Close();
                // Retrait de la liste des clients
                connectedClients.Remove(client);
                return;
            }
        }
        
        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        private void ProcessMessage(Message _messageReceived, Client _client)
        {
            // Traitement des différents cas
            switch (_messageReceived.Subject)
            {
                case "login":
                    // L'utilisateur demande à se connecter avec ce pseudo
                    _client.Name = (string)_messageReceived.received;
                    // L'utilisateur est maintenant authentifié
                    _client.state = MultiplayerState.Authentified;
                    // Info console
                    Console.WriteLine("Client {0} s'est connecté", _client.Name);
                    // Envoi de la confirmation client
                    _client.Send("login", "ok");
                    break;
                default:
                    break;
            }
        }
    }


    
}
