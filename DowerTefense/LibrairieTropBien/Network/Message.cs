using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibrairieTropBien.Network
{
    /// <summary>
    /// Classe d'échange de donnée
    /// </summary>
    [System.Serializable]
    public class Message
    {
        // Sujet du message
        public string Subject { get; set; }
        // Données internes
        public byte[] Data { get; set; }
        // objet reçu
        [System.NonSerialized] public object received;
        // objet envoyé
        [System.NonSerialized] public object send;


        /// <summary>
        /// Génère un message à partir d'un objet
        /// </summary>
        /// <param name="_object">Objet à envoyer</param>
        public Message(string _subject, object _object)
        {
            // Enregistrement du sujet du message
            this.Subject = _subject;

            // Enregistrement de l'objet envoyé
            this.send = _object;

            try
            {
                // Transformation de l'objet en array
                using (var memoryStream = new MemoryStream())
                {
                    (new BinaryFormatter()).Serialize(memoryStream, _object);
                    Data = memoryStream.ToArray();
                }
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Oulà !");
                throw;
            }

        }

        /// <summary>
        /// Restitue un message reçu à partir du tableau d'octets
        /// </summary>
        /// <param name="_bArray"></param>
        public Message(byte[] _bArray)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(_bArray))
                {
                    // Récupération du message
                    IFormatter br = new BinaryFormatter();
                    Message received = (Message)br.Deserialize(ms);
                    this.Subject = received.Subject;

                    using (var memoryStream = new MemoryStream(received.Data))
                    {
                        this.received = (new BinaryFormatter()).Deserialize(memoryStream);
                    }
                }
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Oulà");
                throw;
            }
        }

        /// <summary>
        /// Récupération de l'array d'octets de ce message pour envoi
        /// </summary>
        /// <returns></returns>
        public byte[] GetArray()
        {
            byte[] messageArray;

            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, this);
                messageArray = memoryStream.ToArray();
            }

            return messageArray;
        }
    }
}
