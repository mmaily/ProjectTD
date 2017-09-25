
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

        /// <summary>
        /// Méthode principale de lancement
        /// </summary>
        public static void Main()
        {

            AuthentificationServer authServer = new AuthentificationServer();

            // Variable de sortie du serveur
            bool exit = false;

            Console.ReadLine();

        }


    }

}