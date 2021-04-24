using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public enum CookingStatus
    {
        None, Under, Cooked, Over
    }

    public abstract class ItemAbstract
    {
        public abstract int IdealTime
        {
            get;
        }

        public CookingStatus CookingStatus
        {
            get;
            set;
        }

        public virtual void Cooked(int cookingTime)
        {
            if (cookingTime <= 0)
            {
                this.CookingStatus = CookingStatus.None;
            }
            else if (cookingTime < this.IdealTime)
            {
                this.CookingStatus = CookingStatus.Under;
            }
            else if (cookingTime == this.IdealTime)
            {
                this.CookingStatus = CookingStatus.Cooked;
            }
            else
            {
                this.CookingStatus = CookingStatus.Over;
            }
        }
    }
}
