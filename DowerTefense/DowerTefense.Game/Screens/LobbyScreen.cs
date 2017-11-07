using DowerTefense.Game.Managers;
using LibrairieTropBien.GUI;
using LibrairieTropBien.Network;
using LibrairieTropBien.Network.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DowerTefense.Game.Multiplayer;

namespace DowerTefense.Game.Screens
{
    class LobbyScreen : Screen
    {
        //TODO : infos joueur et opposant
        private Dictionary<PlayerRole, GuiElement> players;
        private Button readyButton;
        private String MyName;
        /// <summary>
        /// Constructeur de base
        /// </summary>
        public LobbyScreen()
        {
            // Init liste éléments GUI
            UIElementsList = new List<GuiElement>();

            // Abonnement aux mises à jour du lobby
            MultiplayerManager.LobbyUpdate += this.LobbyUpdate;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="_graphics"></param>
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            GuiElement attacker = new GuiElement(50, 100, 80, 30)
            {
                Name = "player",
                Text = "Vous",
                TextColor = Color.White,
                BackgroundColor = Color.DarkRed,
                font = CustomContentManager.Fonts["font"],
                GreyedOut = false,
            };

            GuiElement defender = new GuiElement(200, 100, 80, 30)
            {
                Name = "opponant",
                Text = "Adversaire",
                TextColor = Color.White,
                BackgroundColor = Color.DarkRed,
                font = CustomContentManager.Fonts["font"],
                GreyedOut = false,
            };

            // Init boutons
            players = new Dictionary<PlayerRole, GuiElement>
            {
                { PlayerRole.Attacker, attacker },
                { PlayerRole.Defender, defender }
            };
            UIElementsList.Add(attacker);
            UIElementsList.Add(defender);

            readyButton = new Button(100, 200, 50, 50)
            {
                Name = "Ready",
                Tag = "ready",
                Text = "ready",
                font = CustomContentManager.Fonts["font"],
                BackgroundColor = Color.Indigo,
                GreyedOut = true,
            };
            readyButton.OnReleaseLeft += Btn_OnClickLeft;
            readyButton.OnReleaseRight += Btn_OnClickRight;
            UIElementsList.Add(readyButton);

        }

        /// <summary>
        /// Handler de clic sur les boutons de l'interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OnClickLeft(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button)
              && System.Windows.Forms.Form.ActiveForm != null
              && System.Windows.Forms.Form.ActiveForm.Text.Equals("DowerTefense"))
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
        private void Btn_OnClickRight(object sender, EventArgs e) { }

        /// <summary>
        /// Mise à jour de tous les éléments d'interface
        /// </summary>
        /// <param name="_gameTime"></param>
        public override void Update(GameTime _gameTime)
        {
            foreach (GuiElement element in UIElementsList)
            {
                element.Update();
            }

            lock (Messages)
            {
                TreatMessages();
            }
        }

        private void LobbyUpdate(Message _message)
        {
            lock (Messages)
            {
                Messages.Add(_message);
            }
        }
        public override void TreatMessages()
        {
            Player newPlayer;
            foreach (Message _message in Messages)
            {
                
                switch (_message.Subject)
                {
                    
                    case "playerUpdate":
                        // Récupération de l'objet joueur
                        newPlayer = (Player)_message.received;
                        // TODO SALE
                        UpdatePlayer(newPlayer);
                        break;
                    case "YourName":
                        // Récupération de l'objet joueur
                        newPlayer = ((Player)_message.received);
                        MyName = newPlayer.Name;
                        UpdatePlayer(newPlayer);
                        break;
                    case "game":
                        if (_message.received.Equals("starting"))
                        {
                            // Réglage du mode de l'écran de jeu
                            if (players[PlayerRole.Attacker].Text.Equals(MyName))
                            {
                                ScreenManager.UpdateGameScreenMode(false, PlayerRole.Attacker);
                            }
                            else if (players[PlayerRole.Defender].Text.Equals(MyName))
                            {
                                ScreenManager.UpdateGameScreenMode(false, PlayerRole.Defender);
                            }
                            else
                            {
                                ScreenManager.UpdateGameScreenMode(false, PlayerRole.Debug);

                            }
                            // Passage en mode jeu
                            MultiplayerManager.State = MultiplayerState.InGame;
                            // Le jeu commence, on change d'écran
                            ScreenManager.SelectScreen("GameScreen");
                        }
                        break;
                    default:
                        break;
                }
            }
            Messages.Clear();
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
            // Selon l'état du joueur
            if (newPlayer.Ready)
            {
                // Le joueur est prêt
                toUpdate.BackgroundColor = Color.LawnGreen;
            }
            else
            {
                // Le joueur n'est pas prêt
                toUpdate.BackgroundColor = Color.Red;
            }

            toUpdate.Text = newPlayer.Name;
        }

        /// <summary>
        /// Affihage de tous les éléments d'interfaces
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            foreach (GuiElement element in UIElementsList)
            {
                element.Draw(_spriteBatch);
            }



            // Affichage du curseur
            Vector2 lol = Mouse.GetState().Position.ToVector2();
            Texture2D fap = CustomContentManager.Textures["cursor"];
            _spriteBatch.Draw(fap, lol, Color.White);
        }

    }
}
