using LibrairieTropBien.Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DowerTefenseGame.Multiplayer
{

    /// <summary>
    /// Classe principale de gestion du multiplayer
    /// </summary>
    public static class MultiplayerManager
    {

        // Adresse et port du serveur
        private const string connectionServerIP = "86.200.78.166";
        private const string connectionServerIPlocal = "127.0.0.1";
        private const int connectionServerPort = 42666;

        public static bool TryConnect(string Name)
        {
            bool success = false;

            try
            {
                // Client TCP
                TcpClient connectionClient = new TcpClient(connectionServerIPlocal, connectionServerPort);

                ObjectSender.Send(Name, connectionClient);

                //connectionClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return success;
        }

    }
}
