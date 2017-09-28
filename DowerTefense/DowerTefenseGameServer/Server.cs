using DowerTefenseGameServer.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefenseGameServer
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
        public abstract void OnReiceivedData(IAsyncResult ar);
    }
}
