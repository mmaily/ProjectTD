
using LibrairieTropBien.Network;
using System;
using System.Net;
using System.Net.Sockets;

namespace DowerTefenseGameServer
{
    /// <summary>
    /// Classe principale de lancement du serveur pour DowerTefense
    /// Auteur : Bueno le 23/09
    /// Ça s'annonce chiant, mais faut se lancer, et sans librairie moisie
    /// Source : https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example PUTAIN ENCORE UN TRUC NUL OMG
    /// Source 2 en fait : https://docs.microsoft.com/en-us/dotnet/framework/network-programming/using-tcp-services
    /// </summary>
    public static class DowerTefenseGameServer
    {
        // Port du serveur de connexion
        private const string localIP = "127.0.0.1";
        private const int portNum = 42666;


        /// <summary>
        /// Méthode principale de lancement
        /// </summary>
        public static int Main()
        {
            // Variable de sortie du serveur
            bool exit = false;

            // Adress IP locale
            IPAddress local = IPAddress.Parse(localIP);

            // Auditeur TCP
            TcpListener listener = new TcpListener(local, portNum);
            listener.Start();
            System.Console.WriteLine("Serveur lancé. En attente.");

            // Boucle principale
            while (!exit)
            {
                // Attente d'un client
                TcpClient connectionClient = listener.AcceptTcpClient();

                var item = ObjectSender.Receive(connectionClient);

                Type type = item.GetType();

                //connectionClient.Close();
            }

            // Lancement du serveur


            // sortie
            return 0;
        }
    }

}