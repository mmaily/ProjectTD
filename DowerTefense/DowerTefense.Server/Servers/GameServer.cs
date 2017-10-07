﻿
using DowerTefense.Server.Elements;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DowerTefense.Server.Servers
{
    /// <summary>
    /// Classe de serveur de jeu
    /// </summary>
    public class GameServer : Server
    {
        // Correspondance clients / joueurs
        private Dictionary<Client, Player> clients;


        /// <summary>
        /// Constructeur de base du serveur de jeu
        /// </summary>
        public GameServer(Dictionary<Client, Player> _clients)
        {
            // Récupération de la liste des clients
            this.clients = _clients;
            Parallel.ForEach(clients, c =>
            {
                // Changement du callback
                c.Key.ReceiveDataCallback = this.OnReiceivedData;
                c.Key.SetupReceiveCallback(this);
            });

            // Création du jeu
            using (var game = new GameManager(clients))
                game.Run();
        }

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        protected override void ProcessMessage(Message _messageReceived, Client _client)
        {
            //On lock pour éviter les accès concurrentiels
            lock (Request)
            {
                Request.Add(_messageReceived);
            }
            //// Traitement des différents cas
            //switch (_messageReceived.Subject)
            //{
            //    case "towerUpdate":

            //        break;
            //    default:
            //        break;
            //}
        }

        ///<summary>
        ///Méthode déclenché par un événement worth mentionning dans le jeu
        ///Elle envoie cet événement au Translator qui décide quoi en faire (Envoyer, à qui ?)
        ///</summary>

        
    }
}