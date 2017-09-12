using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.GameElements;
using Game1.GameElements.Units;
using Game1.Managers;
using Game1.Screens;

namespace Game1.Managers
{
    class UnitsManager
    {
        private static UnitsManager instance = null;//L'instance est privée pour empêcher d'autre classe de la modifier. Utiliser le getter GetInstance()
        private Map map;
        private int lastSecondSpawned = 0;
        private List<DemoUnit> mobs;
        // Indique si le chemin a été calculé
        private bool pathComputed = false;

        private UnitsManager()
        {
            mobs = new List<DemoUnit>();
            map = MapManager.GetInstance().GetMap();
        }

        //Créé une seule instance du ScreenManager même si il est appelé plusieurs fois
        public static UnitsManager GetInstance()
        {
            if (instance == null)
            {

                instance = new UnitsManager();

            }
            return instance;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Pour chaque unité de la liste des mobs
            foreach (DemoUnit mob in mobs)
            {
                // Affichage de l'unité sur la carte
                spriteBatch.Draw(CustomContentManager.GetInstance().Textures["unit"],mob.Position, Color.White);
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color col, string _what)
        {
        }

        public void Update(GameTime gameTime)
        {
            #region  Calcul du chemin
            // Si le chemin a besoin d'être calculé
            if (!pathComputed)
            {
                MapManager.GetInstance().ComputePath();
            }
            #endregion

            #region Gestion des unitées
            // Si cela fait plus d'une seconde qu'une unitée n'est pas apparue
            int enlapsedSeconds = (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds);
            if (enlapsedSeconds - lastSecondSpawned > 0)
            {
                // On sauvegarde le nouveau temps
                lastSecondSpawned = enlapsedSeconds;
                // On créé une nouvelle unitée
                DemoUnit newMob = new DemoUnit();
                // On définit sa position comme étant celle du spawn
                newMob.UpdatePosition(map.Spawns[0].getTilePosition() * map.tileSize);
                // On définit sa destination comme étant la tuile suivante
                newMob.DestinationTile = map.Spawns[0].NextTile;
                // On l'ajoute à la liste des mobs
                mobs.Add(newMob);
            }

            // Pour chaque mob de la liste
            foreach (DemoUnit mob in mobs)
            {
                // Quantité de déplacement disponible
                float movementAvailable = mob.Speed * map.tileSize * gameTime.ElapsedGameTime.Milliseconds / 1000;

                while (movementAvailable != 0 && !mob.Dead)
                {
                    // Destination
                    Vector2 destinationPosition = mob.DestinationTile.getTilePosition() * map.tileSize;
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

                    mob.UpdatePosition(Vector2.Add(mob.Position, finalMovement));

                } // Fin du while quantité de mouvement > 0

            } // Fin de la boucle pour tous les mobs de la liste

            // Suppression de tous les mobs morts
            mobs.RemoveAll(deadMob => deadMob.Dead);

            #endregion

        }
    }
}
