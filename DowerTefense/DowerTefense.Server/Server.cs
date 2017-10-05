using DowerTefense.Server.Elements;
using LibrairieTropBien.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Server
{
    public abstract class Server
    {
        public Server()
        {

        }

        /// <summary>
        /// Réception des données reçues par un client
        /// </summary>
        /// <param name="ar"></param>
        public virtual void OnReiceivedData(IAsyncResult ar)
        {
            // Récupération du client
            Client client = (Client)ar.AsyncState;
            // Données recues
            byte[] receivedData = client.GetRecievedData(ar);

            // Si aucune donnée n'a été reçue, la connexion est probablement fermée
            if (receivedData.Length > 0)
            {
                client.SetupReceiveCallback(this);

                // Récupération du message
                Message messageReceived = new Message(receivedData);
                ProcessMessage(messageReceived, client);

                //Console.WriteLine("Après réception du message " + messageReceived.Subject +", on remet le callback pour " + this.ToString());
            }
            else
            {
                // TODO : Déconnexion en plein jeu
                Console.WriteLine("Client " + client.Name + " déconnecté.");
                // Fermeture du socket
                client.AuthSocket.Close();
                return;
            }
        }

        protected abstract void ProcessMessage(Message messageReceived, Client client);
    }

}