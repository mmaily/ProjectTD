using System;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace LibrairieTropBien.GUI.Network
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

            // Sérialisation selon le type
            var objectSerializer = new XmlSerializer(_item.GetType());

            // Passage en donnée
            Data data = new Data()
            {
                type = _item.GetType(),
            };
            objectSerializer.Serialize(data.stream, _item);

            // Récupération du flux réseau
            var dataSerializer = new XmlSerializer(typeof(Data));
            var networkStream = _tcpClient.GetStream();

            // Écriture du flux
            if (networkStream.CanWrite)
            {
                dataSerializer.Serialize(networkStream, data);
                success = true;
            }

            // Retour
            return success;
        }

        public static object Receive(NetworkStream _stream)
        {
            // Serialiseur de type Data
            XmlSerializer dataSerializer = new XmlSerializer(typeof(Data));
            // Récupération de l'objet Data
            Data receivedData = (Data)dataSerializer.Deserialize(_stream);

            // Type de l'objet à recevoir
            Type type = receivedData.type.GetType();
            // Serialiseur du type de l'objet
            XmlSerializer objectSerialize = new XmlSerializer(type);
            // Récupération de l'objet


            return  ;
        }
    }
}
