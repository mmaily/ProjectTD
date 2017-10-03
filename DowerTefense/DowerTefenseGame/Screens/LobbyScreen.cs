using DowerTefenseGame.Managers;
using LibrairieTropBien.GUI;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DowerTefenseGame.Multiplayer;

namespace DowerTefenseGame.Screens
{
    class LobbyScreen : Screen
    {
        //TODO : infos joueur et opposant
        private Dictionary<PlayerRole, GuiElement> players;
        private Button readyButton;

        private bool initialized = false;

        /// <summary>
        /// Constructeur de base
        /// </summary>
        public LobbyScreen()
        {
            initialized = false;

            // Init liste éléments GUI
            UIElementsList = new List<GuiElement>();

            // Init boutons
            players = new Dictionary<PlayerRole, GuiElement>();

            // Abonnement aux mises à jour du lobby
            Multiplayer.MultiplayerManager.LobbyUpdate += this.LobbyUpdate;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="_graphics"></param>
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            GuiElement attacker = new Button(50, 100, 80, 30)
            {
                Name = "player",
                Text = "Ici nom du joueur",
                TextColor = Color.White,
                BackgroundColor = Color.DarkRed,
                font = CustomContentManager.GetInstance().Fonts["font"],
                GreyedOut = false,
            };

            GuiElement defender = new Button(200, 100, 80, 30)
            {
                Name = "opponant",
                Text = "",
                TextColor = Color.White,
                BackgroundColor = Color.DarkRed,
                font = CustomContentManager.GetInstance().Fonts["font"],
                GreyedOut = false,
            };

            players.Add(PlayerRole.Attacker, attacker);
            players.Add(PlayerRole.Defender, defender);
            UIElementsList.Add(attacker);
            UIElementsList.Add(defender);

            readyButton = new Button(100, 200, 50, 50)
            {
                Name = "Ready",
                Tag = "ready",
                Text = "ready",
                font = CustomContentManager.GetInstance().Fonts["font"],
                BackgroundColor = Color.Indigo,
                GreyedOut = true,
            };
            readyButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(readyButton);

            MultiplayerManager.Send("lol", "casseToiOMG");

            initialized = true;
        }

        /// <summary>
        /// Handler de clic sur les boutons de l'interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OnClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button btn = (Button)sender;
                switch (btn.Tag.ToString())
                {
                    case "ready":
                        MultiplayerManager.Send("ready", btn.GreyedOut ? "ok" : "nope");
                        btn.GreyedOut = !btn.GreyedOut;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Mise à jour de tous les éléments d'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Update();
            });
        }

        private void LobbyUpdate(Message _message)
        {
            Messages.Add(_message);
        }
        public override void TreatMessages()
        {
            foreach(Message _message in Messages)
            {
                switch (_message.Subject)
                {
                    case "playerUpdate":
                        // Récupération de l'objet joueur
                        Player newPlayer = (Player)_message.received;
                        // TODO SALE
                        UpdatePlayer(newPlayer);
                        break;
                    case "game":
                        if (_message.received.Equals("starting"))
                        {
                            // Réglage du mode de l'écran de jeu
                            ScreenManager.GetInstance().UpdateGameScreenMode(false);
                            // Le jeu commence, on change d'écran
                            ScreenManager.GetInstance().SelectScreen("GameScreen");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Mise à jour d'un joueur
        /// </summary>
        /// <param name="newPlayer"></param>
        private void UpdatePlayer(Player newPlayer)
        {
            // Récupération du bouton à modifier
            GuiElement toUpdate = players[newPlayer.Role];
            // Mise à jour
            toUpdate.GreyedOut = newPlayer.Ready;
            toUpdate.Text = newPlayer.Name;
        }

        /// <summary>
        /// Affihage de tous les éléments d'interfaces
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Draw(_spriteBatch);
            });
            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.GetInstance().Textures["cursor"];
            _spriteBatch.Draw(fap, lol, Color.White);
        }

    }
}
