using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieTropBien.ObjectExtension
{
    /// <summary>
    /// Méthodes d'extension de la classe byteArray
    /// </summary>
    public static class ByteArray
    {
        /// <summary>
        /// Ajoute le tableau en paramètre à la fin du tableau concerné
        /// </summary>
        /// <param name="_mainArray">Tableau destination</param>
        /// <param name="_arrayToAdd">Tableau à ajouter</param>
        public static byte[] Append(this byte[] _mainArray, byte[] _arrayToAdd)
        {
            // Si le tableau concerné n'a pas été instancié
            if (_mainArray == null)
            {
                // On crée un tableau vide
                _mainArray = new byte[0];
            }

            // Tableau complet
            byte[] arrayFull = new byte[_mainArray.Length + _arrayToAdd.Length];
            // Ajout du tableau principal
            Buffer.BlockCopy(_mainArray, 0, arrayFull, 0, _mainArray.Length);
            // Ajout du tableau à ajouter à la fin
            Buffer.BlockCopy(_arrayToAdd, 0, arrayFull, _mainArray.Length, _arrayToAdd.Length);
            
            // Le retour de l'array
            return arrayFull;
        }
    }
}