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
        private const int connectionServerPort = 42666;

        public static bool TryConnect(string Name)
        {
            bool success = false;

            try
            {
                // Client TCP
                TcpClient connectionClient = new TcpClient(IPAddress.Any.ToString(), connectionServerPort);
                // Récupération du flux
                NetworkStream ns = connectionClient.GetStream();

                // Chaîne de demande de connexion
                byte[] askConnectionString = Encoding.ASCII.GetBytes(Name);

                // Envoi des bytes de donnée
                ns.Write(askConnectionString, 0, askConnectionString.Length);

                ns.Close();
                connectionClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return success;
        }

    }
}
