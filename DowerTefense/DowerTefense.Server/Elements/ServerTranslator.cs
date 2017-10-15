using DowerTefense.Commons;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Commons.Units;
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
                requests.Clear();
            }

        }

        //Méthode qui transforme le message en action sur le jeu
    public static void Translate(ref GameEngine game, Message message)
        {
            //TODO : faire tous les cas qui intéressent le serveur INGAME
            switch (message.Subject)
            {
                case "DdefenseList":
                    game.DefenseBuildingsList = (List<Building>)message.received;
                    break;
                case "DWaitingForConstruction":
                    //TODO : Gérer la différenciation entre Tower et Spawning
                    game.DefenseBuildingsList.AddRange((List<Building>)message.received);
                    break;
                case "newTower":
                    // Réception d'un nouveau bâtiment
                    game.WaitingForConstruction.Add((Building)message.received);
                    break;
                default:
                    break;
            }

        }
    
    public static void SendGameUpdate(Dictionary<Dictionary<String, object>, bool> Changes, ref Dictionary<Client, Player> clients)
        {
            foreach(Client c in clients.Keys)
            {
                //Si dans le dictionnaire il y un objet qui a la valeur True, alors il a changé et on l'envoie aux clients
                //Là attention, il y a un Dictionnaire qui contient des dictionnaires. Les sous dictionnaire contiennent
                //la paire objet/nom de l'objet, et ce mini-dictionnaire est associé à un boolean pour savoir si ça a changé
                foreach (var dic in Changes.Where(pair => pair.Value == true))
                {
                    //Une fois qu'on a trouvé les changement, on extrait les paires objet/nom et on les envoie
                    foreach (var underDic in dic.Key)
                    {
                        //Les sous dictionnaires sont définis dans le GameEngine
                        c.Send(underDic.Key, underDic.Value);
                    }

                }

            }

        }
    }
}
