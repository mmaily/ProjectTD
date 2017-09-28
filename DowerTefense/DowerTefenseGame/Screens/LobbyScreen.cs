using DowerTefenseGame.Managers;
using DowerTefenseGame.Screens;
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
        private Dictionary<PlayerRole, Button> buttons;
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
            buttons = new Dictionary<PlayerRole, Button>();

            // Abonnement aux mises à jour du lobby
            Multiplayer.MultiplayerManager.LobbyUpdate += this.LobbyUpdate;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="_graphics"></param>
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            Button attacker = new Button(50, 100, 80, 30)
            {
                Name = "player",
                Text = "Ici nom du joueur",
                BackgroundColor = Color.Wheat,
            };

            Button defender = new Button(200, 100, 80, 30)
            {
                Name = "opponant",
                Text = "",
                BackgroundColor = Color.Wheat,
            };

            buttons.Add(PlayerRole.Attacker, attacker);
            buttons.Add(PlayerRole.Defender, defender);
            UIElementsList.Add(attacker);
            UIElementsList.Add(defender);

            readyButton = new Button(100, 200, 50, 50)
            {
                Name = "Ready",
                Tag = "ready",
                Text = "ready",
            };
            readyButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(readyButton);

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
            while (!initialized)
            {
                System.Threading.Thread.Sleep(10);
            }


            switch (_message.Subject)
            {
                case "playerUpdate":
                    // Récupération de l'objet joueur
                    Player newPlayer = (Player)_message.received;
                    // TODO SALE
                    UpdatePlayer(newPlayer);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mise à jour d'un joueur
        /// </summary>
        /// <param name="newPlayer"></param>
        private void UpdatePlayer(Player newPlayer)
        {
            // Récupération du bouton à modifier
            Button toUpdate = buttons[newPlayer.Role];
            // Mise à jour
            toUpdate.Disabled = newPlayer.Ready;
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
