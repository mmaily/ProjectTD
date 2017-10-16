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
        public void OnReceivedData(IAsyncResult ar)
        {
            // Vérification de la présence de données
            byte[] receivedData = GetRecievedData(ar);
            // Ajout au tampon de message
            messageBuffer = messageBuffer.Append(receivedData);
            // Si le nombre d'octets reçus est supérieur à 0
            if (messageBuffer.Length > 0)
            {
                // Tentative de formation du message
                Message messageReceived = null;
                bool fullMessage = false;
                try
                {
                    // Récupération du message reçu
                    messageReceived = new Message(messageBuffer);
                    // Si c'est bon, le message est complet
                    fullMessage = true;
                } catch (Exception e)
                {
                    // Le paquet n'est pas complet
                    fullMessage = false;
                }

                // Si le message reçu était complet
                if (fullMessage && messageReceived!=null)
                {
                    // Affichage console
                    Console.WriteLine("<<< Message reçu <<< émetteur : " + this.Name + ", sujet : " + messageReceived.Subject + ", corps : " + messageReceived.received.ToString());

                    // On invoque l'évènement de réception de message
                    MessageReceived?.Invoke(this, messageReceived);
                    // On vide le tampon de message
                    messageBuffer = null;

                }
                else
                {
                    // Le message n'est pas encore totalement reçu
                }

                // Remise en était du callback de réception
                AsyncCallback recieveDataCallBack = new AsyncCallback(OnReceivedData);
                AuthSocket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, recieveDataCallBack, AuthSocket);

            }
            else
            {
                // La connextion est probablement fermée
                //socket.Shutdown(SocketShutdown.Both);
                AuthSocket.Close();
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
            int nToBeRead = 0;
            try
            {
                nToBeRead = AuthSocket.Available;
            }
            catch (Exception)
            {
                // Le socket a été fermé, c'est pas grave... (Nan en vrai c'est qu'on switch de serveur)
            }


            if (nToBeRead > 0)
            {
                // Récupération des octets restants
                byte[] byData = new byte[nToBeRead];
                AuthSocket.Receive(byData);
                // Ajout des octets au tableau de retour
                byReturn = byReturn.Append(byData);
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
            // Info console
            Console.WriteLine(">>> Message envoyé >>> destinataire : " + this.Name + ", sujet : " + _subject +", corps : " + _data.ToString());

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
