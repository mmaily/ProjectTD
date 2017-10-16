using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DowerTefense.Commons.GameElements.Units
{
    [Serializable()]
     public class Unit : Entity
    {
        public float BaseSpeed { get;protected set; }
        public double SpeedCoeff { get;protected set; }
        public int SpeedPrice { get;protected set; }
        public double SpeedPriceCoeff { get; protected set; }
        public int MaxHealthPointsPrice { get; protected set; }
        public int BaseMaxHealthPoints { get; protected set; }
        public double MaxHealthPointsCoeff { get; protected set; }
        public double MaxHealthPointsPriceCoeff { get; protected set; }

        public Unit()
        {
        }


        public virtual Unit DeepCopy()
        {
            Unit other = (Unit)this.MemberwiseClone();
            return other;
        }
        #region Leveling
        public Boolean SpeedLvlUp(int gold)
        {
            Boolean success = false;

            if (SpeedPrice <= gold)
            {
                this.Speed += (int)Math.Ceiling(this.BaseSpeed * SpeedCoeff);
                //Calcule le nouveau coût du lvl up
                SpeedPrice *= (int)Math.Ceiling(1 + SpeedPriceCoeff);
            }
            return success;
        }
        public Boolean MaxHealthPointsLvlUp(int gold)
        {
            Boolean success = false;

            if (MaxHealthPointsPrice <= gold)
            {
                this.MaxHealthPoints += (int)Math.Ceiling(this.BaseMaxHealthPoints * MaxHealthPointsCoeff);
                //Calcule le nouveau coût du lvl up
                MaxHealthPointsPrice *= (int)Math.Ceiling(1 + MaxHealthPointsPriceCoeff);
            }
            return success;
        }
        #endregion

    }

}
