using DowerTefenseGameServer.Elements;
using LibrairieTropBien.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGameServer.Servers
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
            _attack.SetupReceiveCallback(this);
            _defense.SetupReceiveCallback(this);
        }

        /// <summary>
        /// Traitement du message reçu
        /// </summary>
        /// <param name="_messageReceived"></param>
        protected override void ProcessMessage(Message _messageReceived, Client _client)
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
