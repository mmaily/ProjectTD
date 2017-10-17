using LibrairieTropBien.ObjectExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.Network
{
    /// <summary>
    /// Classe interagissant avec le réseau : envoi et émission de trames TCP
    /// </summary>
    public class NetworkInterface
    {
        // Évènement de réception de message complet
        public event MessageReceivedEventHanlder MessageReceived;
        public delegate void MessageReceivedEventHanlder(Message message);

        // Tableau d'octets en tampon
        private byte[] buffer;

        // Longueur des données du message en cours de réception
        private int messageSize;

        /// <summary>
        /// Constructeur de l'interface réseau
        /// </summary>
        /// <param name="_socket"></param>
        public NetworkInterface()
        {
            this.messageSize = 0;
        }

        /// <summary>
        /// Méthode statique d'envoi de message sur un flux TCP avec encapsulation de taille de message
        /// </summary>
        /// <param name="_message"></param>
        /// <param name="_socket"></param>
        /// <returns></returns>
        public static bool Send(Message _message, Socket _socket)
        {
            // Récupération du tableau d'octets du message
            byte[] bMessage = _message.GetArray();

            byte[] encapsulated = new byte[bMessage.Length + 2];

            Buffer.BlockCopy(BitConverter.GetBytes((short)bMessage.Length), 0, encapsulated, 0, 2);
            Buffer.BlockCopy(bMessage, 0, encapsulated, 2, bMessage.Length);

            // Envoi du message avec récupération du nombre d'octets envoyés
            int sent = _socket.Send(encapsulated, encapsulated.Length, 0);

            // Confirmation
            return sent > 0;
        }

        /// <summary>
        /// Ajout d'octets au tampon de réception
        /// </summary>
        /// <param name="_data"></param>
        public void AddReceivedData(byte[] _data)
        {
            // Si nous n'avons pas de données en entrée : c'est le premier message
            if(messageSize == 0 || buffer == null)
            {
                // Récupération de la longeur
                byte[] lol = new byte[] { _data[0], _data[1] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lol);

                messageSize = BitConverter.ToInt16(lol, 0);

                // Récupération des données réelles
                byte[] realData = new byte[_data.Length - 2];
                Buffer.BlockCopy(_data, 2, realData, 0, realData.Length);
                _data = realData;
            }

            // Ici, messageSize != 0

            // Si la longueur des données reçues est exactement la longueur attendue, nous avons reçu un message en entier
            if (messageSize == _data.Length)
            {
                // Reconstitution du message
                Message messageReceived = new Message(_data);
                // Invocation de l'évènement
                MessageReceived?.Invoke(messageReceived);

                // Réinitialisation de la taille du message attendu pour la prochaine fois
                messageSize = 0;
            }
            // Sinon, si nous n'avons pas reçu assez de données
            else if (messageSize > _data.Length)
            {
                // C'est que la suite va arriver
                buffer = buffer.Append(_data);

                // On enregistre la longueur qu'il manque
                messageSize -= _data.Length;

                // Et on attend la prochaine fois !
            }
            // Sinon, nous avons trop de données !
            else if (messageSize < _data.Length)
            {
                // On récupère donc juste les données qu'il faut pour finir le message actuel
                byte[] missingData = new byte[messageSize];
                Buffer.BlockCopy(_data, 0, missingData, 0, messageSize);

                // On l'ajoute aux données précédentes (peut-être qu'il n'y en a pas hein)
                buffer = buffer.Append(missingData);

                // On créé le message
                Message messageReceived = new Message(buffer);
                // On invoque l'évènement
                MessageReceived?.Invoke(messageReceived);

                // On récupère les données restantes
                byte[] leftData = new byte[_data.Length - messageSize];
                Buffer.BlockCopy(_data, messageSize, leftData, 0, _data.Length - messageSize);

                // On réinitialise le buffer
                buffer = null;
                // Et la taille du message attendu
                messageSize = 0;

                // Enfin, on appel récursivement la méthode pour traiter le reste du message
                this.AddReceivedData(leftData);
            }
        }


        /// Récupération des données recues
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>Tableau d'octets des données</returns>
        public static byte[] GetReceivedData(IAsyncResult _ar, Socket _socket, byte[] _receivedBuffer)
        {
            // Nombre d'octets reçus
            int nBytesReceived = 0;
            try
            {
                nBytesReceived = _socket.EndReceive(_ar);
            }
            catch { }
            byte[] byReturn = new byte[nBytesReceived];

            // Copie des octets
            Array.Copy(_receivedBuffer, byReturn, nBytesReceived);

            // Vérifie la présence de données restantes
            // Augmente la performance des paquets
            // "pas essentiel et chiant à lire"
            int nToBeRead = 0;
            try
            {
                nToBeRead = _socket.Available;
            }
            catch (Exception)
            {
                // Le socket a été fermé, c'est pas grave... (Nan en vrai c'est qu'on switch de serveur)
            }


            if (nToBeRead > 0)
            {
                // Récupération des octets restants
                byte[] byData = new byte[nToBeRead];
                _socket.Receive(byData);
                // Ajout des octets au tableau de retour
                byReturn = byReturn.Append(byData);
            }

            return byReturn;
        }
    }
}
