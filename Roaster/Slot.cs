using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public enum SlotStatus
    {
        Open, Cooking, Pending
    }

    public class Slot : IRoasterStatusChangedHandler
    {
        private ItemAbstract item;
        private int CookingTime = 0;

        public SlotStatus SlotStatus
        {
            get; private set;
        }

        internal bool Receive(ItemAbstract item)
        {
            if(this.item == null && SlotStatus == SlotStatus.Open)
            {
                this.item = item;
                return true;
            }
            return false;
        }

        internal ItemAbstract Release()
        {
            ItemAbstract outItem = null;
            if (this.item != null && SlotStatus == SlotStatus.Open)
            {
                outItem = this.item;
                this.item = null;
            }

            return outItem;
        }

        public void StartCooking(RoasterStatus roasterStatus)
        {
            lock (this)
            {
                if (roasterStatus == RoasterStatus.On)
                {
                    this.SlotStatus = SlotStatus.Cooking;
                }
                else
                {
                    this.SlotStatus = SlotStatus.Pending;
                }
            }
            this.CookingTime = 0;
        }

        internal void EndCooking()
        {
            if (this.item != null && this.SlotStatus != SlotStatus.Open)
            {
                this.item.Cooked(this.CookingTime);
            }

            lock (this)
            {
                this.SlotStatus = SlotStatus.Open;
            }
        }

        public void OnTimerClick()
        {
            lock (this)
            {
                if (this.SlotStatus == SlotStatus.Cooking)
                {
                    this.CookingTime += 1;
                }
            }
        }

        public void OnRoasterOn()
        {
            lock (this)
            {
                if (this.SlotStatus == SlotStatus.Pending)
                {
                    this.SlotStatus = SlotStatus.Cooking;
                }
            }
        }

        public void OnRoasterOff()
        {
            lock (this)
            {
                if (this.SlotStatus == SlotStatus.Cooking)
                {
                    this.SlotStatus = SlotStatus.Pending;
                }
            }
        }
    }
}
