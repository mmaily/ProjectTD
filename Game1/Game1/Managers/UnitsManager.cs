using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DowerTefenseGame.GameElements;
using DowerTefenseGame.GameElements.Units;

namespace DowerTefenseGame.Managers
{

    /// <summary>
    /// Gestionnaire d'unité
    /// </summary>
    class UnitsManager
    {

        // Instance du gestionnaire d'unité
        private static UnitsManager instance = null;
        // Horodatage d'appartition de la dernière unité
        private int lastSecondSpawned = 0;
        // Liste des unités courantes
        public List<DemoUnit> mobs;
        // Carte en cours
        public Map CurrentMap { get; set; }

        /// <summary>
        /// Constructeur du gestionnaire d'unité
        /// </summary>
        private UnitsManager()
        {
            mobs = new List<DemoUnit>();
            CurrentMap = MapManager.GetInstance().map;
        }

        /// <summary>
        /// Récupération de l'instance du gestionnaire d'unité
        /// </summary>
        /// <returns></returns>
        public static UnitsManager GetInstance()
        {
            if (instance == null)
            {

                instance = new UnitsManager();

            }
            return instance;
        }

        /// <summary>
        /// Mise à jour des unités
        /// </summary>
        /// <param name="_gameTime"></param>
        public void Update(GameTime _gameTime)
        {
            #region === Gestion du déplacement des unités ===
            // Si cela fait plus d'une seconde qu'une unité n'est pas apparue
            int enlapsedSeconds = (int)Math.Floor(_gameTime.TotalGameTime.TotalMilliseconds);
            if (enlapsedSeconds - lastSecondSpawned > 200)
            {
                // On sauvegarde le nouveau temps
                lastSecondSpawned = enlapsedSeconds;
                // On créé une nouvelle unité
                DemoUnit newMob = new DemoUnit();
                // On définit sa position comme étant celle du spawn
                newMob.UpdatePosition(CurrentMap.Spawns[0].getTilePosition() * CurrentMap.tileSize);
                // On définit sa destination comme étant la tuile suivante
                newMob.DestinationTile = CurrentMap.Spawns[0].NextTile;
                // On l'ajoute à la liste des mobs
                mobs.Add(newMob);
            }
            // Pour chaque mob de la liste
            foreach (DemoUnit mob in mobs)
            {
                // Si le mob est mort
                if(mob.HealthPoints <= 0)
                {
                    mob.Dead = true;
                    continue;
                }

                // Quantité de déplacement disponible
                float movementAvailable = mob.Speed * CurrentMap.tileSize * _gameTime.ElapsedGameTime.Milliseconds / 1000;
                
                // Tant que l'unité peut encore se déplacer et n'est pas morte
                while (movementAvailable != 0 && !mob.Dead)
                {
                    // Destination
                    Vector2 destinationPosition = mob.DestinationTile.getTilePosition() * CurrentMap.tileSize;
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
                        // On modifie la destination
                        if (mob.DestinationTile.TileType == Tile.TileTypeEnum.Base)
                        {
                            // Si la tuile destination était une base, on détruit le mob et on recommence
                            mob.Dead = true;
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

            #endregion
        }

        /// <summary>
        /// Affichage des unités
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Pour chaque unité de la liste des mobs
            foreach (DemoUnit mob in mobs)
            {
                // Affichage de l'unité sur la carte
                spriteBatch.Draw(CustomContentManager.GetInstance().Textures[mob.name],mob.Position, Color.White);
            }
        }

    }
}
