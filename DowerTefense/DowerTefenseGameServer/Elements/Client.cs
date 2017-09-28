using System;
using System.Net.Sockets;
using LibrairieTropBien.Network;

namespace DowerTefenseGameServer.Elements
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

        public string Name { get; set; }

        private byte[] receivedBuffer = new byte[50];     // Receive data buffer

        public DateTime ConnectedSince { get; set; }

        // Etat de la connexion
        public MultiplayerState state = MultiplayerState.Disconnected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_sock">client socket conneciton this object represents</param>
        public Client(Socket _sock)
        {
            AuthSocket = _sock;
            this.Name = "";
        }

        /// <summary>
        /// Lise en place du callback pour réception de données
        /// </summary>
        /// <param name="app"></param>
        public void SetupRecieveCallback(AuthentificationServer _authentificationServer)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(_authentificationServer.OnReiceivedData);
                AuthSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, recieveData, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mise en place du callback pour réception - Échec : {0}", ex.Message);
            }
        }

        /// <summary>
        /// Récupération des données recues
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>Tableau d'octets des données</returns>
        public byte[] GetRecievedData(IAsyncResult ar)
        {
            // Nombre d'octets reçus
            int nBytesReceived = 0;
            try
            {
                nBytesReceived = AuthSocket.EndReceive(ar);
            }
            catch { }
            byte[] byReturn = new byte[nBytesReceived];

            // Copie des octets
            Array.Copy(receivedBuffer, byReturn, nBytesReceived);

            // Vérifie la présence de données restantes
            // Augmente la performance des paquets
            // "pas essentiel et chiant à lire"
            int nToBeRead = AuthSocket.Available;
            if (nToBeRead > 0)
            {
                // Récupération des octets restants
                byte[] byData = new byte[nToBeRead];
                AuthSocket.Receive(byData);
                // Ajout des octets au tableau de retour
                byte[] byReturnFull = new byte[nBytesReceived + nToBeRead];
                Buffer.BlockCopy(byReturn, 0, byReturnFull, 0, nBytesReceived);
                Buffer.BlockCopy(byData, 0, byReturnFull, nBytesReceived, nToBeRead);
                byReturn = byReturnFull;
            }

            return byReturn;
        }

        /// <summary>
        /// Méthode d'envoi de données
        /// </summary>
        /// <param name="_subject">Sujet du message</param>
        /// <param name="_data">Données du message</param>
        public void Send(string _subject, object _data)
        {
            // Si pas connecté
            if (AuthSocket == null || !AuthSocket.Connected)
            {
                return;
            }

            try
            {
                // Création d'un objet message et envoi
                Message message = new Message(_subject, _data);
                byte[] bMessage = message.GetArray();
                AuthSocket.Send(bMessage, bMessage.Length, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d'envoi du message : " + e.ToString());
            }
        }
    }
}
