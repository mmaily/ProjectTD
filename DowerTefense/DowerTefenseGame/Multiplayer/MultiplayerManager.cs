using LibrairieTropBien.Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DowerTefenseGame.Multiplayer
{

    /// <summary>
    /// Classe principale de gestion du multiplayer
    /// </summary>
    public static class MultiplayerManager
    {

        // Adresse et port du serveur
        private const string authServerIP = "86.200.78.166";
        private const string authServerIPlocal = "127.0.0.1";
        private const int authServerPort = 42666;

        // Socket de connexion
        private static Socket authSocket = null;
        // Received data buffer
        private static byte[] receivedBuffer = new byte[256];

        // Temp ?
        private static string name;

        /// <summary>
        /// Tentative de connexion au serveur d'authentification
        /// </summary>
        /// <param name="Name">Pseudo de connexion</param>
        /// <returns></returns>
        public static bool TryConnect(string _name)
        {
            bool success = false;

            name = _name;

            try
            {
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
                IPEndPoint epAuthServer = new IPEndPoint(IPAddress.Parse(authServerIPlocal), authServerPort);

                // Connexion au serveur avec cellback
                authSocket.Blocking = false;
                AsyncCallback onConnect = new AsyncCallback(OnConnect);
                authSocket.BeginConnect(epAuthServer, onConnect, authSocket);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Callback de la tentative de connexion
        /// </summary>
        /// <param name="ar"></param>
        public static void OnConnect( IAsyncResult ar)
        {
            // Récupération du socket
            Socket authSocket = (Socket)ar.AsyncState;

            // Vérification du succès de la connexion
            try
            {
                if (authSocket.Connected)
                {
                    // Mise en place du callback de réception de données
                    AsyncCallback receiveData = new AsyncCallback(OnReceivedData);
                    authSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, receiveData, authSocket);

                    // Processus d'authentification
                    StartAuthentification();
                }
                else
                {
                    // C'est con
                }
            }
            catch (Exception e)
            {
                // OMFG
            }
        }

        /// <summary>
        /// Callback de réception de données
        /// </summary>
        /// <param name="ar"></param>
        public static void OnReceivedData(IAsyncResult ar)
        {
            // Récupération du socket
            Socket socket = (Socket)ar.AsyncState;

            // Vérification de la présence de données
            try
            {
                int nBytesReceived = socket.EndReceive(ar);
                // Si le nombre d'octets reçus est supérieur à 0
                if(nBytesReceived > 0) {
                    // Récupération du message reçu
                    Message messageReceived = new Message(receivedBuffer);

                    // Remise en était du callback de réception
                    AsyncCallback recieveData = new AsyncCallback(OnReceivedData);
                    socket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, recieveData, socket);
                }
                else
                {
                    // La connextion est probablement fermée
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Fermeture du socket de connexion
        /// </summary>
        private static void CloseConnection()
        {
            if(authSocket != null && authSocket.Connected)
            {
                authSocket.Shutdown(SocketShutdown.Both);
                authSocket.Close();
            }
        }

        /// <summary>
        /// Méthode d'envoi de données
        /// </summary>
        /// <param name="_subject">Sujet du message</param>
        /// <param name="_data">Données du message</param>
        private static void Send(string _subject, object _data)
        {
            // Si pas connecté
            if(authSocket == null || !authSocket.Connected)
            {
                return;
            }

            try
            {
                Message message = new Message(_subject, _data);
                byte[] bMessage = message.GetArray();
                authSocket.Send(bMessage, bMessage.Length, 0);
            }
            catch (Exception e)
            {

                throw;
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
    }
}
