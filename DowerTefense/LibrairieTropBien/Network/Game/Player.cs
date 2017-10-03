using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.Network.Game
{
    /// <summary>
    /// Enumrateur des rôles d'un joueur
    /// </summary>
    public enum PlayerRole
    {
        Attacker,
        Defender,
        Spectator,
        Debug, // Both
    }

    /// <summary>
    /// Classe joueur échangeable sur le réseau
    /// </summary>
    [Serializable]
    public class Player
    {
        /// <summary>
        /// Nom du joueur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rôle du joueur
        /// </summary>
        public PlayerRole Role { get; set; }

        /// <summary>
        /// Joueur prêt
        /// </summary>
        public bool Ready { get; set; }

        /// <summary>
        /// Constructeur du joueur
        /// </summary>
        public Player()
        {
            this.Name = "";
            this.Role = PlayerRole.Spectator;
            this.Ready = false;
        }
    }
}
