using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units
{
    [Serializable()]
     public class Unit : Entity, ISerializable
    {
        public Unit()
        {

        }
        public virtual Unit DeepCopy()
        {
            Unit other = (Unit)this.MemberwiseClone();
            return other;
        }
    }

}
