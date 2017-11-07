﻿using DowerTefense.Commons;
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
using DowerTefense.Commons.GameElements.Units.Buildings.AttackBuildings;
using DowerTefense.Game.Players;
using LibrairieTropBien.Network.Game;
using DowerTefense.Game.Screens;

namespace DowerTefense.Game.Translator
{
    class ClientTranslator
    {
        //On envoie la liste des requêtes et on les traites une par une. 
        //Au final on fait une première update du Jeu en fonction des requêtes avant l'Update de la mécanique interne
        //On clear la liste des request à la fin
        public static void UpdateGame(ref GameEngine game, ref List<Message> orders, Boolean vsAi, PlayerRole _role)
        {
            lock (orders)
            {
                foreach (Message message in orders)
                {
                    Translate(ref game, message, vsAi, _role);
                }
                orders.Clear();
            }
        }

        //Méthode qui transforme le message en action sur le jeu
        public static void Translate(ref GameEngine game, Message message, Boolean vsAi, PlayerRole _role)
        {

            if (!vsAi)
            {
                //TODO : faire tous les cas qui intéressent les Clients ONLINE
                switch (message.Subject)
                {
                    case "NewBuildingsList":
                        game.ToConstructList = (List<Building>)message.received;
                        break;
                    case "UpdateBuildingsList":
                        game.ToUpdateList = (List<Building>)message.received;
                        break;
                    case "newWave":
                        game.newWave = true;
                        break;
                    //case "DefenseBuildingsList":
                    //    game.DefenseBuildingsList = (List<Building>)message.received;
                    //    break;
                    //case "FreeBuildingsList":
                    //    game.FreeBuildingsList = (List<SpawnerBuilding>)message.received;
                    //    break;
                    case "LockedBuildingsList":
                        game.LockedBuildingsList = (List<SpawnerBuilding>)message.received;
                        break;
                    case "defensePlayer":
                        game.defensePlayer = (DefensePlayer)message.received;
                        break;
                    case "attackPlayer":
                        game.attackPlayer = (AttackPlayer)message.received;
                        break;
                    #region Ecran de fin de jeu
                    case "attackWon":
                        ScreenManager.SetBackGroundScreen(ScreenManager.ScreenEnum.GameScreen);
                        if (_role == PlayerRole.Attacker || _role == PlayerRole.Debug)
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.WinScreen);
                        }
                        else
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.LoseScreen);
                        }
                        break;
                    case "defenseWon":
                        ScreenManager.SetBackGroundScreen(ScreenManager.ScreenEnum.GameScreen);
                        if (_role == PlayerRole.Defender || _role == PlayerRole.Debug)
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.WinScreen);
                        }
                        else
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.LoseScreen);
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            else
            {

                //TODO : faire tous les cas qui intéressent les Clients OFFLINE
                switch (message.Subject)
                {

                    case "newTower":
                        Building t = ((Tower)message.send).DeepCopy();
                        game.WaitingForConstruction.Add(t);
                        break;
                    case "newSpawner":
                        Building sp = (SpawnerBuilding)((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForConstruction.Add(sp);
                        break;
                    default:
                        break;
                    case "upTowerSpeed":
                        Building upTS = ((Tower)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upTS, "SpeedLvlUp");
                        break;
                    case "upTowerRange":
                        Building upTR = ((Tower)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upTR, "RangeLvlUp");
                        break;
                    case "upTowerDamage":
                        Building upTD = ((Tower)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upTD, "DmgLvlUp");
                        break;
                    #region UpSpawner
                    case "upSpawnerSpawnRate":
                        Building upSPR = ((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upSPR, "SpawnRateLvlUp");
                        break;
                    case "upSpawnerNumberSpawn":
                        Building upSNS = ((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upSNS, "NumberSpawnLvlUp");
                        break;
                    case "upSpawnerUnitHealth":
                        Building upSUH = ((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upSUH, "UnitHealthLvlUp");
                        break;
                    case "upSpawnerUnitSpeed":
                        Building upSUS = ((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upSUS, "UnitSpeedLvlUp");
                        break;
                    case "switchPower":
                        Building upSSP = ((SpawnerBuilding)message.send).DeepCopy();
                        game.WaitingForUpdate.Add(upSSP, "SwitchPower");
                        break;
                    case "UpdateBuildingsList":
                        game.ToUpdateList = (List<Building>)message.send;
                        break;
                    #endregion
                    #region Ecran de fin de jeu
                    case "attackWon":
                        ScreenManager.SetBackGroundScreen(ScreenManager.ScreenEnum.GameScreen);
                        if (_role == PlayerRole.Attacker || _role == PlayerRole.Debug)
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.WinScreen);
                        }
                        else
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.LoseScreen);
                        }
                        break;
                    case "defenseWon":
                        ScreenManager.SetBackGroundScreen(ScreenManager.ScreenEnum.GameScreen);
                        if (_role == PlayerRole.Defender || _role == PlayerRole.Debug)
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.WinScreen);
                        }
                        else
                        {
                            ScreenManager.SelectScreen(ScreenManager.ScreenEnum.LoseScreen);
                        }
                        break;
                        #endregion

                }
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
                foreach (var underDic in dic.Key.Where(pair => pair.Value != null))
                {
                    //Les sous dictionnaires sont définis dans le GameEngine
                    MultiplayerManager.Send(underDic.Key, underDic.Value);
                }

            }
        }
        public static void AutoGameUpdate(Dictionary<Dictionary<String, object>, bool> Changes, ref List<Message> orders)
        {
            //Si dans le dictionnaire il y un objet qui a la valeur True, alors il a changé et on l'envoie aux clients
            //Là attention, il y a un Dictionnaire qui contient des dictionnaires. Les sous dictionnaire contiennent
            //la paire objet/nom de l'objet, et ce mini-dictionnaire est associé à un boolean pour savoir si ça a changé
            Message message;
            Dictionary<String, object> underdic;
            foreach (var dic in Changes.Where(pair => pair.Value == true))
            {
                underdic = dic.Key;
                //Une fois qu'on a trouvé les changement, on extrait les paires objet/nom et on se les auto-envoi
                // C'est à dire on les place dans la list orders
                foreach (var minidic in underdic.Where(pair => pair.Value != null))
                {
                    message = new Message(minidic.Key, minidic.Value);
                    lock (orders)
                    {
                        orders.Add(message);
                    }
                }

            }
        }
    }
}
