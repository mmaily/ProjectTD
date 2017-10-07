using DowerTefense.Commons;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Server.Elements
{
    static class ServerTranslator
    {
        //On envoie la liste des requêtes et on les traites une par une. 
        //Au final on fait une première update du Jeu en fonction des requêtes avant l'Update de la mécanique interne
        //On clear la liste des request à la fin
    public static void UpdateGame(ref GameEngine game, ref List<Message> requests)
        {
            lock (requests)
            {
                foreach (Message message in requests)
                {
                    Translate(ref game, message);
                }
            }
            requests.Clear();
        }

        //Méthode qui transforme le message en action sur le jeu
    public static void Translate(ref GameEngine game, Message message)
        {
            switch (message.Subject)
            {
                case "newTower":
                    //TODO : Faire envoyer un string par le client, et chercher la tour correspondante dans les Dummies
                    game.DefenseBuildingsList.Add((Tower)message.received);
                    break;
            }
        }
    
    public static void SendGameUpdate(Dictionary<object,bool> Changes, ref Dictionary<Client, Player> clients)
        {
            Parallel.ForEach(clients.Keys, c =>
            {
                //Si dans le dictionnaire il y un objet qui a la valeur True, alors il a changé et on l'envoie aux clients
                foreach (object obj in Changes.Where(change => change.Value==true))
                {
                    //TODO : A mon avis ça marche pas ça
                    c.Send(obj.ToString(),obj);
                }
                
            });

        }
    }
}
