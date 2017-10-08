using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using LibrairieTropBien.GUI;
using DowerTefense.Game.Managers;
using Microsoft.Xna.Framework;
using DowerTefense.Game.Multiplayer;
using LibrairieTropBien.Network;

namespace DowerTefense.Game.Screens
{
    class EntranceScreen : Screen
    {
        // Bouton de connexion au service
        private Button connectionButton;
        /// <summary>
        /// Constructeur de l'écran d'acceuil
        /// </summary>
        public EntranceScreen()
        {

        }
        
        /// <summary>
        /// Initialisation de l'écran d'accueil
        /// </summary>
        /// <param name="_graphics"></param>
        public override void Initialize(GraphicsDeviceManager _graphics)
        {
            this.Graphics = _graphics;
            Graphics.PreferredBackBufferHeight = 400;
            Graphics.PreferredBackBufferWidth = 600;
            Graphics.ApplyChanges();
            #region Création du bouton Versus IA
            // Bouton de contruction de tour basique
            int height = 100;
            int width = 150;
            Button newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2, width, height)
            {
                Name = "versusIA",
                Tag = "horsLigne"

            };
            newButton.SetText("Versus IA", CustomContentManager.Fonts["font"]);
            newButton.BackgroundColor = Color.DarkGreen;
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion
            #region Création du bouton matchmaking
            newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2 + height, width, height)
            {
                Name = "Matchmaking",
                Tag = "matchmaking"
            };
            newButton.SetText("Matchmaking", CustomContentManager.Fonts["font"]);
            newButton.BackgroundColor = Color.Blue;
            newButton.TextColor = Color.White;
            newButton.OnRelease += Btn_OnClick;
            newButton.Disabled = true;
            UIElementsList.Add(newButton);
            #endregion
            #region Création du bouton pour aller sur l'éditeur
            // Bouton pour aller à l'éditeur
            newButton = new Button((_graphics.PreferredBackBufferWidth - width) / 2, (_graphics.PreferredBackBufferHeight / 2 - height) / 2 + 2 * height, width, height)
            {
                Name = "BasicSpawner",
                Tag = "editor"

            };
            newButton.SetText("EDITEUR MAP", CustomContentManager.Fonts["font"]);
            newButton.SetTexture(CustomContentManager.Textures[newButton.Name], false);
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion

            #region Bouton de connexion
            connectionButton = new Button(10, 10, 80, 30)
            {
                Name = "loginButton",
                Tag = "connect",
                BackgroundColor = Color.DarkRed,
            };
            connectionButton.SetText("Connexion", CustomContentManager.Fonts["font"]);
            connectionButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(connectionButton);

            #endregion

            #region Boutons choix rôle
            newButton = new Button(10, 100, 100, 20)
            {
                Name = "Attack",
                Tag = "multi",
                Text = "Attack",
                BackgroundColor = Color.Wheat,
                TextColor = Color.Black,
                font = CustomContentManager.Fonts["font"],
            };
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            newButton = new Button(10, 150, 100, 20)
            {
                Name = "Defense",
                Tag = "multi",
                Text = "Defense",
                BackgroundColor = Color.Wheat,
                TextColor = Color.Black,
                font = CustomContentManager.Fonts["font"],

            };
            newButton.OnRelease += Btn_OnClick;
            UIElementsList.Add(newButton);
            #endregion
            // Abonnement aux modifications de l'état de connexion du compte
            MultiplayerManager.StateChanged += StateChanged;

        }

        /// <summary>
        /// Changement d'état de connexion du compte
        /// </summary>
        /// <param name="_state"></param>
        private void StateChanged(MultiplayerState _state)
        {
            switch (_state)
            {
                case MultiplayerState.Disconnected:
                    // Modification du bouton de connexion
                    connectionButton.Text = "Connexion";
                    connectionButton.BackgroundColor = Color.DarkRed;
                    UIElementsList.Find(elem => elem.Name.Equals("Matchmaking")).Disabled = true;
                    break;
                case MultiplayerState.Connected:
                    // Modification du bouton de connexion
                    connectionButton.Text = "Connexion...";
                    connectionButton.BackgroundColor = Color.DarkOrange;
                    break;
                case MultiplayerState.Authentified:
                    // Modification du bouton de connexion
                    connectionButton.Text = MultiplayerManager.name;
                    connectionButton.BackgroundColor = Color.Green;
                    // Modification du bouton de matchmaking
                    UIElementsList.Find(elem => elem.Name.Equals("Matchmaking")).Disabled = false;
                    break;
                case MultiplayerState.SearchingGame:
                    // Modifiication bouton Matchmaking
                    ((Button)UIElementsList.Find(elem => elem.Name.Equals("Matchmaking"))).Text = "Searching...";
                    // TODO : remettre bouton en état
                    break;
                case MultiplayerState.InLobby:
                    ScreenManager.SelectScreen("Lobby");
                    break;
                case MultiplayerState.InGame:
                    break;
                case MultiplayerState.InEndGameLobby:
                    break;
                default:
                    break;
            }
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
                    case "horsLigne":
                        ScreenManager.UpdateGameScreenMode(true);
                       ScreenManager.SelectScreen("GameScreen");
                        break;
                    case "matchmaking":
                        //MultiplayerManager.SearchMatch("");
                        break;
                    case "multi":
                        MultiplayerManager.SearchMatch(btn.Name);
                        break;
                    case "editor":
                        ScreenManager.SelectScreen("Editor");
                        break;
                    case "connect":
                        // Si le compte est déconnecté
                        if (MultiplayerManager.State == MultiplayerState.Disconnected)
                        {
                            // Tentative de connexion
                            // TODO : Nom de connexion
                            MultiplayerManager.TryConnect(DateTime.Now.Millisecond.ToString());
                        }
                        else
                        {
                            // Sinon, déconnexion
                            MultiplayerManager.CloseConnection();
                        }
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

        /// <summary>
        /// Affihage de tous les éléments d'interfaces
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            Parallel.ForEach(UIElementsList, element =>
            {
                element.Draw(_spriteBatch);
            });
            base.Draw(_spriteBatch);
        }
    }
}
