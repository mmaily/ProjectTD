using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefense.Commons.GameElements;
using DowerTefense.Commons.GameElements.Units;
using DowerTefense.Commons.GameElements.Projectiles;
using System.Linq;
using LibrairieTropBien.Network;
using LibrairieTropBien.SerializableObjects;
using static DowerTefense.Commons.GameElements.Units.Buildings.DefenseBuildings.Tower;

namespace DowerTefense.Commons.Managers
{

    /// <summary>
    /// Gestionnaire d'unité
    /// </summary>
    public static class UnitEngine
    {

        /// <summary>
        /// Traitement des unités : avance, décès, victoire
        /// </summary>
        /// <param name="mobs"></param>
        public static bool ProcessMobs(ref List<Unit> mobs, GameTime _gameTime, byte _tileSize, ref int goldWon, ref int livesLost)
        {
            // Est-ce qu'un mob est mort ?
            bool mobDied = false; 

            // Pour chaque mob de la liste
            foreach (Unit mob in mobs)
            {
                // Si le mob est mort
                if (mob.HealthPoints <= 0)
                {
                    mob.Dead = true;
                    goldWon += mob.GoldValue;

                    mobDied = true;
                    continue;
                }

                // Quantité de déplacement disponible
                float movementAvailable = mob.Speed * _tileSize * _gameTime.ElapsedGameTime.Milliseconds / 1000;

                // Tant que l'unité peut encore se déplacer et n'est pas morte
                while (movementAvailable != 0 && !mob.Dead)
                {
                    // Destination
                    Vector2 destinationPosition = mob.DestinationTile.GetTilePosition() * _tileSize;
                    // Quantité de mouvement nécessaire pour le déplacement
                    Vector2 movement = destinationPosition - mob.Position;
                    // Etude de faisabilité du déplacement
                    Vector2 finalMovement;
                    if (movement.Length() > movementAvailable)
                    {
                        // Si le déplacement voulu est trop grand
                        // On normalise le vecteur mouvement
                        movement.Normalize();
                        // On le multiplie par la quantité de mouvement restante
                        movement *= movementAvailable;
                        // On valide ce mouvement
                        finalMovement = movement;
                        // On ajoute cette distance à la distance totale parcourue par le mob
                        mob.DistanceTraveled += movementAvailable;
                        // On vide la quantité de mouvement restante
                        movementAvailable = 0;
                    }
                    else
                    {
                        // Si le déplacement est plus petit quand la quantité de mouvement restante
                        // On valide le mouvement prévu
                        finalMovement = movement;
                        // On recalcule la quantité de mouvement restante
                        movementAvailable -= movement.Length();
                        // On ajoute le déplacement à la quantité totale de déplacement du mob
                        mob.DistanceTraveled += movement.Length();
                        // On modifie la destination
                        if (mob.DestinationTile.TileType == Tile.TileTypeEnum.Base)
                        {
                            // Si la tuile destination était une base, on détruit le mob et on recommence
                            mob.Dead = true;
                            mobDied = true;
                            // Suppression d'une vie
                            livesLost++;
                        }
                        else
                        {
                            // Sinon, on passe à la tuile suivante
                            mob.DestinationTile = mob.DestinationTile.NextTile;
                        }

                    }

                    // On met à jour la position de cette unité
                    mob.UpdatePosition(Vector2.Add(mob.Position, finalMovement));

                } // Fin du while quantité de mouvement > 0 ou mort

            } // Fin de la boucle pour tous les mobs de la liste

            // Suppression de toutes les unités mortes
            mobs.RemoveAll(deadMob => deadMob.Dead);

            // Il y a un changement si : un mob est mort OU argent gagné OU vie perdue
            return mobDied || goldWon > 0 || livesLost > 0 ;
        }
       
        /// <summary>
        /// Récupère la liste des unités sur la carte triées selon leur avancement sur le chemin
        /// </summary>
        /// <param name="_units">Liste des unités à trier</param>
        /// <returns>Liste des unités triées</returns>
        public static List<Unit> GetSortedUnitList(List<Unit> _units, FocusEnum focus)
        {
            List<Unit> sortedList = null;
            switch (focus)
            {
                case FocusEnum.Far:
                    sortedList = _units.OrderBy(m => m.DistanceTraveled).ToList<Unit>();
                    break;
                case FocusEnum.Close:
                    sortedList = _units.OrderByDescending(m => m.DistanceTraveled).ToList<Unit>();
                    break;
                case FocusEnum.Weak:
                    sortedList = _units.OrderByDescending(m => m.HealthPoints).ToList<Unit>();
                    break;
                case FocusEnum.Strong:
                    sortedList = _units.OrderBy(m => m.HealthPoints).ToList<Unit>();
                    break;
            }

            return sortedList;
        }

    }
}
