using System;
using System.IO;

namespace LibrairieTropBien.GUI.Network
{
    /// <summary>
    /// Classe d'échange de donnée
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Type de l'objet
        /// </summary>
        public Type type;
        /// <summary>
        /// Données à échanger
        /// </summary>
        public Stream stream;

        /// <summary>
        /// Données à échanger sur le réseau
        /// </summary>
        public Data()
        {

        }

        /// <summary>
        /// Données à échanger sur le réseau
        /// </summary>
        public Data(Type _type, string _data)
        {
            this.type = _type;
            //this.stream = _data;
        }
    }
}
