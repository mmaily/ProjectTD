using System;
using System.Net.Sockets;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using LibrairieTropBien.ObjectExtension;

namespace DowerTefense.Server.Elements
{
    /// <summary>
	/// Class holding information and buffers for the Client socket connection
	/// </summary>
	public class Client
    {

        /// <summary>
        /// Connexion au client
        /// </summary>
        public Socket AuthSocket { get; private set; }

        /// <summary>
        /// Callback de réception
        /// </summary>
        AsyncCallback receiveDataCallback;


        /// <summary>
        /// Nom du client
        /// </summary>
        public string Name { get; set; }

        // Buffer de réception du callback
        private byte[] receivedBuffer = new byte[255];     // Receive data buffer

        // Buffer de réception pour fusion des paquets
        private byte[] messageBuffer;

        /// <summary>
        /// Durée de connexion
        /// </summary>
        public DateTime ConnectedSince { get; set; }

        // Etat de la connexion
        public MultiplayerState state = MultiplayerState.Disconnected;

        // Interface réseau
        NetworkInterface networkInterface;

        /// <summary>
        /// Évènement de réception de message
        /// </summary>
        public event MessageReceivedEventHanlder MessageReceived;
        public delegate void MessageReceivedEventHanlder(Client sender, Message message);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_sock">client socket conneciton this object represents</param>
        public Client(Socket _sock)
        {
            AuthSocket = _sock;
            this.Name = "";

            // Interface réseau
            networkInterface = new NetworkInterface();

            // Abonnement à l'évènement
            networkInterface.MessageReceived += this.MessageReceivedHandler;

            // Mise en place du callback
            receiveDataCallback = new AsyncCallback(OnReceivedData);
        }

        /// <summary>
        /// Mise en place du callback pour réception de données
        /// </summary>
        /// <param name="app"></param>
        public void SetupReceiveCallback()
        {
            try
            {
                AuthSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, this.receiveDataCallback, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mise en place du callback pour réception - Échec : {0}", ex.Message);
            }
        }


        /// <summary>
        /// Callback de réception de données
        /// </summary>
        /// <param name="ar"></param>
        public void OnReceivedData(IAsyncResult _ar)
        {
            // Récupération des données reçues
            byte[] receivedData = NetworkInterface.GetReceivedData(_ar, AuthSocket, receivedBuffer);

            // Si le nombre d'octets reçus est supérieur à 0
            if (receivedData.Length > 0)
            {
                // Ajout des octets reçus au tampon de réception
                networkInterface.AddReceivedData(receivedData);

                // Remise en était du callback de réception
                AsyncCallback recieveDataCallBack = new AsyncCallback(OnReceivedData);
                AuthSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, recieveDataCallBack, AuthSocket);
            }
            else
            {
                // La connextion est probablement fermée
                AuthSocket.Close();
                // Etat : déconnecté
                state = MultiplayerState.Disconnected;
            }
        }

        private void MessageReceivedHandler(Message _message)
        {
            MessageReceived?.Invoke(this, _message);
        }


        /// <summary>
        /// Méthode d'envoi de données
        /// </summary>
        /// <param name="_subject">Sujet du message</param>
        /// <param name="_data">Données du message</param>
        public void Send(string _subject, object _data)
        {
            // Info console
            //Console.WriteLine(">>> Message envoyé >>> destinataire : " + this.Name + ", sujet : " + _subject +", corps : " + _data.ToString());

            // Si pas connecté
            if (AuthSocket == null || !AuthSocket.Connected)
            {
                return;
            }

            try
            {
                // Création d'un objet message et envoi
                Message message = new Message(_subject, _data);

                // Envoi des données
                NetworkInterface.Send(message, AuthSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d'envoi du message : " + e.ToString());
            }
        }
    }
}
