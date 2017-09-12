using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.Screens;
using Game1.Managers;
using Game1.GameElements;
using System.Collections.Generic;
using System;

namespace Game1
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>


    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Carte actuelle
        //private Map map;
        // Indique si le chemin a été calculé
       // private bool pathComputed = false;

        // Temporaire : spawn de mob
       // private int lastSecondSpawned = 0;
      //  private List<DemoUnit> mobs;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenManager.GetInstance();
            CustomContentManager.GetInstance();

            //mobs = new List<DemoUnit>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);



            //Use the Content_manager to load all the content, the different "Screens" can then acces them
            CustomContentManager.GetInstance().LoadTextures(Content);


            // Chargement de la carte
            //map = new Map();


            // Réglage de la taille de l'écran selon la carte
            graphics.PreferredBackBufferHeight = MapManager.GetInstance().GetMap().mapHeight * MapManager.GetInstance().GetMap().tileSize;
            graphics.PreferredBackBufferWidth = MapManager.GetInstance().GetMap().mapWidth * MapManager.GetInstance().GetMap().tileSize;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Mise à jour du temps de jeu
            base.Update(gameTime);
            ScreenManager.GetInstance().Update(gameTime);
            // Réglage de la taille de l'écran selon la carte



            //#region  Calcul du chemin
            //// Si le chemin a besoin d'être calculé
            //if (!pathComputed)
            //{
            //    map.ComputePath();
            //}
            //#endregion

            //#region Gestion des unitées
            //// Si cela fait plus d'une seconde qu'une unitée n'est pas apparue
            //int enlapsedSeconds = (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds);
            //if (enlapsedSeconds - lastSecondSpawned > 0)
            //{
            //    // On sauvegarde le nouveau temps
            //    lastSecondSpawned = enlapsedSeconds;
            //    // On créé une nouvelle unitée
            //    DemoUnit newMob = new DemoUnit();
            //    // On définit sa position comme étant celle du spawn
            //    newMob.UpdatePosition(map.Spawns[0].getTilePosition() * map.tileSize);
            //    // On définit sa destination comme étant la tuile suivante
            //    newMob.DestinationTile = map.Spawns[0].NextTile;
            //    // On l'ajoute à la liste des mobs
            //    mobs.Add(newMob);
            //}

            //// Pour chaque mob de la liste
            //foreach (DemoUnit mob in mobs)
            //{
            //// Quantité de déplacement disponible
            //float movementAvailable = mob.Speed * map.tileSize * gameTime.ElapsedGameTime.Milliseconds / 1000;

            //    while (movementAvailable != 0 && !mob.Dead)
            //    {
            //        // Destination
            //        Vector2 destinationPosition = mob.DestinationTile.getTilePosition() * map.tileSize;
            //        // Quantité de mouvement nécessaire pour le déplacement
            //        Vector2 movement = destinationPosition - mob.Position;
            //        // Etude de faisabilité du déplacement
            //        Vector2 finalMovement;
            //        if (movement.Length() > movementAvailable)
            //        {
            //            // Si le déplacement voulu est trop grand
            //            // On normalise le vecteur mouvement
            //            movement.Normalize();
            //            // On le multiplie par la quantité de mouvement restante
            //            movement *= movementAvailable;
            //            // On valide ce mouvement
            //            finalMovement = movement;
            //            // On vide la quantité de mouvement restante
            //            movementAvailable = 0;
            //        }
            //        else
            //        {
            //            // Si le déplacement est plus petit quand la quantité de mouvement restante
            //            // On valide le mouvement prévu
            //            finalMovement = movement;
            //            // On recalcule la quantité de mouvement restante
            //            movementAvailable -= movement.Length();
            //            // On modifie la destination
            //            if (mob.DestinationTile.TileType == Tile.TileTypeEnum.Base)
            //            {
            //                // Si la tuile destination était une base, on détruit le mob et on recommence
            //                mob.Dead = true;
            //            }
            //            else
            //            {
            //                // Sinon, on passe à la tuile suivante
            //                mob.DestinationTile = mob.DestinationTile.NextTile;
            //            }

            //        }

            //        mob.UpdatePosition(Vector2.Add(mob.Position, finalMovement));

            //    } // Fin du while quantité de mouvement > 0

            //} // Fin de la boucle pour tous les mobs de la liste

            //// Suppression de tous les mobs morts
            //mobs.RemoveAll(deadMob => deadMob.Dead);

            //#endregion

            //#region Sélection d'une tuile
            //// Récupération de l'état de la souris
            //MouseState mouseState = Mouse.GetState();
            //// Récupération de la position de la souris
            //Point mousePosition = mouseState.Position;
            //// Si la souris est dans l'écran
            //if(GraphicsDevice.Viewport.Bounds.Contains(mousePosition))
            //{
            //    // On récupère la tuile visée
            //    Tile selectedTile = map.Tiles[mousePosition.Y / map.tileSize, mousePosition.X / map.tileSize];
            //    // On marque la tuile comme sélectionnée
            //    selectedTile.selected = true;
            //}
            //#endregion
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Récupération du ScreenManager
            ScreenManager screenManager = ScreenManager.GetInstance();

            GraphicsDevice.Clear(Color.Black);

            // Début de l'affichage
            spriteBatch.Begin();


            // Affichage de la carte
           // map.Draw(spriteBatch);

            //// Pour chaque unité de la liste des mobs
            //foreach (DemoUnit mob in mobs)
            //{
            //    // Affichage de l'unité sur la carte
            //    screenManager.Draw(spriteBatch, mob.Position, Color.White, "unit");
            //}

            // Affichage du curseur
            screenManager.Draw(spriteBatch, Mouse.GetState().Position.ToVector2(), Color.White, "cursor");

            // Fin de l'affichage
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
