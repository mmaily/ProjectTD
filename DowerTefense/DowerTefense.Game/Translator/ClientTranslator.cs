using DowerTefense.Commons;
using LibrairieTropBien.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowerTefense.Game;
using DowerTefense.Game.Multiplayer;
using DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings;
using DowerTefense.Commons.Units;

namespace DownerTefense.Game.Translator
{
    class ClientTranslator
    {
        //On envoie la liste des requêtes et on les traites une par une. 
        //Au final on fait une première update du Jeu en fonction des requêtes avant l'Update de la mécanique interne
        //On clear la liste des request à la fin
        public static void UpdateGame(ref GameEngine game, ref List<Message> orders)
        {
            lock (orders)
            {
                foreach (Message message in orders)
                {
                    Translate(ref game, message);
                }
                orders.Clear();
            }
        }

        //Méthode qui transforme le message en action sur le jeu
        public static void Translate(ref GameEngine game, Message message)
        {
            //TODO : faire tous les cas qui intéressent les Clients
            switch (message.Subject)
            {
                case "DdefenseList":
                    game.DefenseBuildingsList= (List<Building>)message.received;
                    break;
            }
        }

        public static void SendGameUpdate(Dictionary<Dictionary<String, object>, bool> Changes)
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
                    MultiplayerManager.Send(underDic.Key, underDic.Value);
                }

            }
        }
    }
}
