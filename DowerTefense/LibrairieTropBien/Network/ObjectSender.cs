using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace LibrairieTropBien.Network
{
    /// <summary>
    /// Classe pour envoyer des objets sur le réseau
    /// </summary>
    public static class ObjectSender
    {
        /// <summary>
        /// Envoi d'un objet
        /// </summary>
        /// <param name="_item">Objet à envoyer</param>
        /// <param name="_tcpClient">TCPClient</param>
        /// <returns>Succès de l'opéarion</returns>
        public static bool Send(object _item, TcpClient _tcpClient)
        {
            // Succès de l'opération
            bool success = false;

            Message message;

            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, _item);
                message = new Message
                {
                    Data = memoryStream.ToArray(),
                };
            }

            // Récupération du flux réseau
            var dataSerializer = new XmlSerializer(typeof(Message));
            var networkStream = _tcpClient.GetStream();

            // Écriture du flux
            if (networkStream.CanWrite)
            {
                dataSerializer.Serialize(networkStream, message);
                success = true;
            }

            networkStream.Close();
            
            // Retour
            return success;
        }

        public static object Receive(TcpClient _tcpClient)
        {
            object received;

            NetworkStream stream = _tcpClient.GetStream();

            // Serialiseur de type Data
            XmlSerializer dataSerializer = new XmlSerializer(typeof(Message));
            // Récupération de l'objet Data
            Message message = (Message)dataSerializer.Deserialize(stream);

            using (var memoryStream = new MemoryStream(message.Data))
            {
                received = (new BinaryFormatter()).Deserialize(memoryStream);
            }

            stream.Close();

            return received;
        }
    }
}
