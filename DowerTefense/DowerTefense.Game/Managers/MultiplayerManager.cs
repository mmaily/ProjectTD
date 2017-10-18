using LibrairieTropBien.Network;
using LibrairieTropBien.ObjectExtension;
using System;
using System.Net;
using System.Net.Sockets;

namespace DowerTefense.Game.Multiplayer
{

    /// <summary>
    /// Classe principale de gestion du multiplayer
    /// </summary>
    public static class MultiplayerManager
    {

        // Adresse et port du serveur
        private const string authServerIP = "86.200.78.166";
        private const string authServerIPlocal = "127.0.0.1";
        private const string authServerIPDistant = "5.51.40.64";
        private const int authServerPort = 42666;

        // Socket de connexion
        private static Socket authSocket = null;
        // Received data buffer
        private static byte[] receivedBuffer = new byte[255];

        // Interface réseau
        private static NetworkInterface networkInterface;

        // Etat du compte
        private static MultiplayerState state = MultiplayerState.Disconnected;
        public static MultiplayerState State
        {
            get => state;
            set
            {
                state = value;
                StateChanged?.Invoke(value);
            }
        }
        public static event StateChangedEventHanlder StateChanged;
        public delegate void StateChangedEventHanlder(MultiplayerState state);

        //Mise à jour du lobby
        public static event LobbyUpdateEventHanlder LobbyUpdate;
        public delegate void LobbyUpdateEventHanlder(Message message);

        //Mise à jour du jeu
        public static event GameUpdateEventHanlder GameUpdate;
        public delegate void GameUpdateEventHanlder(Message message);

        // Temp ?
        public static string name;

        #region === Connexion ===

        /// <summary>
        /// Tentative de connexion au serveur d'authentification
        /// </summary>
        /// <param name="Name">Pseudo de connexion</param>
        /// <returns></returns>
        public static void TryConnect(string _name)
        {
            name = _name;

            // Fermeture du socket si toujours ouvert
            if (authSocket != null && authSocket.Connected)
            {
                authSocket.Shutdown(SocketShutdown.Both);
                System.Threading.Thread.Sleep(10);
                authSocket.Close();
            }

            // Création de l'objet socket
            authSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            // Définition de l'addresse du serveur
            IPEndPoint epAuthServer = new IPEndPoint(IPAddress.Parse(authServerIPDistant), authServerPort);

            // Connexion au serveur avec cellback
            authSocket.Blocking = false;
            AsyncCallback onConnect = new AsyncCallback(OnConnect);
            authSocket.BeginConnect(epAuthServer, onConnect, authSocket);

        }

        /// <summary>
        /// Callback de la tentative de connexion
        /// </summary>
        /// <param name="ar"></param>
        public static void OnConnect(IAsyncResult ar)
        {
            // Récupération du socket
            Socket authSocket = (Socket)ar.AsyncState;

            // Vérification du succès de la connexion
            try
            {
                if (authSocket.Connected)
                {
                    // Création de l'interface réseau
                    networkInterface = new NetworkInterface();

                    // Abonnement à l'évènement de réception de message
                    networkInterface.MessageReceived += ProcessMessageReceived;

                    // Mise en place du callback de réception de données
                    AsyncCallback receiveData = new AsyncCallback(OnReceivedData);
                    authSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, receiveData, authSocket);

                    // Etat : connecté
                    State = MultiplayerState.Connected;

                    // Processus d'authentification
                    StartAuthentification();
                }
                else
                {
                    // C'est con
                }
            }
            catch (Exception)
            {
                // OMFG
            }
        }

        /// <summary>
        /// Processus d'authentification
        /// </summary>
        private static void StartAuthentification()
        {
            // Demande de connection avec le pseudo demandé
            Send("login", name);
        }

        /// <summary>
        /// Fermeture du socket de connexion
        /// </summary>
        public static void CloseConnection()
        {
            if (authSocket != null && authSocket.Connected)
            {
                authSocket.Shutdown(SocketShutdown.Both);
                authSocket.Close();
            }

            // Etat : déconnecté
            State = MultiplayerState.Disconnected;
        }

        #endregion

        #region === Émission ===

        /// <summary>
        /// Méthode d'envoi de données
        /// </summary>
        /// <param name="_subject">Sujet du message</param>
        /// <param name="_data">Données du message</param>
        public static void Send(string _subject, object _data)
        {
            // Si pas connecté
            if (authSocket == null || !authSocket.Connected)
            {
                return;
            }

            try
            {
                // Création d'un objet message et envoi
                Message message = new Message(_subject, _data);
                // Envoi du message
                NetworkInterface.Send(message, authSocket);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Demande de recherche de match
        /// </summary>
        public static void SearchMatch(string _role)
        {
            Send("matchmaking", _role);
        }

        #endregion

        #region === Réception ===


        /// <summary>
        /// Callback de réception de données
        /// </summary>
        /// <param name="_ar"></param>
        public static void OnReceivedData(IAsyncResult _ar)
        {
            // Récupération des données reçues
            byte[] receivedData = NetworkInterface.GetReceivedData(_ar, authSocket, receivedBuffer);

            // Si le nombre d'octets reçus est supérieur à 0
            if (receivedData.Length > 0)
            {
                // Ajout des octets reçus au tampon de réception
                networkInterface.AddReceivedData(receivedData);

                // Remise en était du callback de réception
                AsyncCallback recieveDataCallBack = new AsyncCallback(OnReceivedData);
                authSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, recieveDataCallBack, authSocket);
            }
            else
            {
                // La connextion est probablement fermée
                authSocket.Close();
                // Etat : déconnecté
                State = MultiplayerState.Disconnected;
            }
        }

        /// <summary>
        /// Traitement du message reçu selon l'état de la connexion et le sujet du message
        /// </summary>
        /// <param name="_message">Message reçu</param>
        public static void ProcessMessageReceived(Message _message)
        {
            // Selon le message du sujet et l'état de connexion, on en déduit l'utilité du message
            switch (State)
            {
                case MultiplayerState.Disconnected:
                    break;
                case MultiplayerState.Connected:
                    // Cela concerne la demande d'authentification précédente :
                    if (_message.Subject.Equals("login"))
                    {
                        switch (_message.received)
                        {
                            case "ok":
                                // Connexion réussie
                                State = MultiplayerState.Authentified;
                                //
                                break;
                            default:
                                // Erreur d'authentification
                                break;
                        }
                    }
                    break;
                case MultiplayerState.Authentified:
                    if (_message.Subject.Equals("matchmaking"))
                    {
                        switch (_message.received)
                        {
                            case "searching":
                                State = MultiplayerState.SearchingGame;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case MultiplayerState.SearchingGame:
                    if (_message.Subject.Equals("newLobby"))
                    {
                        State = MultiplayerState.InLobby;
                    }
                    break;
                case MultiplayerState.InLobby:
                    LobbyUpdate?.Invoke(_message);
                    break;
                case MultiplayerState.InGame:
                    GameUpdate?.Invoke(_message);
                    break;
                case MultiplayerState.InEndGameLobby:
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
