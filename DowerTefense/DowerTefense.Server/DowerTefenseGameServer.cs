
using DowerTefense.Server.Servers;
using System;

namespace DowerTefense.Server
{
    /// <summary>
    /// Classe principale de lancement du serveur pour DowerTefense
    /// Auteur : Bueno le 23/09
    /// Ça s'annonce chiant, mais faut se lancer, et sans librairie moisie
    /// Source : https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example PUTAIN ENCORE UN TRUC NUL OMG
    /// Source 2 en fait : https://docs.microsoft.com/en-us/dotnet/framework/network-programming/using-tcp-services
    /// </summary>
    public static class DowerTefense.Server
    {

        /// <summary>
        /// Méthode principale de lancement
        /// </summary>
        public static void Main()
        {
            // Lancement du serveur d'authentification
            AuthentificationServer authServer = new AuthentificationServer();

            Console.ReadLine();

        }


    }

}