using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Méthode d'extension du type "object"
/// Source : http://www.c-sharpcorner.com/UploadFile/ff2f08/deep-copy-of-object-in-C-Sharp/
/// </summary>
namespace LibrairieTropBien.ObjectExtension
{
    public static class Object
    {
        public static object CloneObject(this object objSource)

        {
            // Récupération du type de l'objet
            Type typeSource = objSource.GetType();
            // Création d'une nouvelle instance
            object objTarget = Activator.CreateInstance(typeSource);

            // Récupération de toutes les propriétés de l'objet source
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Assignation de toutes les proprités
            foreach (PropertyInfo property in propertyInfo)
            {
                // Vérification de l'accès en écriture
                if (property.CanWrite)
                {
                    // Si la propriété est de type valeur, enum ou string
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        // On la copie directement
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    else
                    {
                        // Sinon, c'est un type par référence : par récursivité, on le copie lui même

                        // Vérification, sinon caca : source https://stackoverflow.com/a/6156603
                        if (property.GetIndexParameters().Length == 0)
                        {
                            continue;
                        }
                        // Sinon, récupération de la propriété
                        object objPropertyValue = property.GetValue(objSource);
                        // Si l'objet est null
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            // On copie le nouvel objet de manière récursive
                            property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                        }

                    }

                }

            }

            return objTarget;

        }
    }
}
