using DowerTefenseGameServer.Elements;
using LibrairieTropBien.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGameServer
{
    /// <summary>
    /// Classe de serveur de jeu
    /// </summary>
    public class GameServer : Server
    {
        /// <summary>
        /// Constructeur de base du serveur de jeu
        /// </summary>
        public GameServer(Client _attack, Client _defense)
        {
            _attack.SetupRecieveCallback(this);
            _defense.SetupRecieveCallback(this);
        }

        /// <summary>
        /// Réception des données reçues par un client
        /// </summary>
        /// <param name="ar"></param>
        public override void OnReiceivedData(IAsyncResult ar)
        {
            // Récupération du client
            Client client = (Client)ar.AsyncState;
            // Données recues
            byte[] receivedData = client.GetRecievedData(ar);

            // Si aucune donnée n'a été reçue, la connexion est probablement fermée
            if (receivedData.Length > 0)
            {
                client.SetupRecieveCallback(this);

                // Récupération du message
                Message messageReceived = new Message(receivedData);
                ProcessMessage(messageReceived, client);
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

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        private void ProcessMessage(Message _messageReceived, Client _client)
        {
            // Traitement des différents cas
            switch (_messageReceived.Subject)
            {
                default:
                    break;
            }
        }
    }
}
