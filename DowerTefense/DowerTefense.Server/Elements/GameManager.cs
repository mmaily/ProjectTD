using DowerTefense.Commons;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime;

namespace DowerTefense.Server.Elements
{
    class GameManager : Microsoft.Xna.Framework.Game
    {
        #region===Gestion des requêtes===
        public Dictionary<Client, Player> clients;
        private List<Message> Requests;
        #endregion  
        private GameEngine game;
        // Gestionnaire des graphiques
        GraphicsDeviceManager graphics;
        /// <summary>
        /// Constructeur
        /// </summary>
        public GameManager(Dictionary<Client, Player> _clients, ref List<Message> _Requests)
        {
            Content.RootDirectory = "Content";
            game = new GameEngine(true);
            this.clients = _clients;
            this.Requests = _Requests;

            graphics = new GraphicsDeviceManager(this);

        }

        /// <summary>
        /// Initialisation des composants
        /// </summary>
        protected override void Initialize()
        { 

            // Initialisation des composants
            base.Initialize();
            //On initialise les variables internes de game
            game.Initialize();


        }

        /// <summary>
        /// Chargement des contenus
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// Déchargement des contenus
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Mise à jour du jeu
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Mise à jour du temps de jeu
            base.Update(gameTime);
            //TODO : Mettre à jour le jeu en fonction des requêtes
            ServerTranslator.UpdateGame(ref game, ref Requests);
            //Mise a jour du jeu en interne
            game.Update(gameTime);
            //On regarde la liste des changements et on les envoie aux clients
            //TODO : Pas sur que la méthode d'envoie dans le ServerTranslator soit ouf ouf
            ServerTranslator.SendGameUpdate(game.Changes, ref clients);
        }


        /// <summary>
        /// Mise à jour de l'affichage
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
        }
    }
    
    
}
