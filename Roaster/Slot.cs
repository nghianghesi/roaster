using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public class Slot : ITimmeoutHandler, ILeverStatusChangedHandler
    {
        private ItemAbstract item;
        private int CookingTime = 0;

        private Roaster roaster;

        private Lever lever;

        public Slot(Roaster roaster, Timer timer, Lever lever)
        {
            this.roaster = roaster;
            this.lever = lever;
            timer?.TimeoutHandlers.Add(this);
            lever?.LeverStatusChangedHandlers.Add(this);
        }

        internal bool Receive(ItemAbstract item)
        {
            if(this.item == null && this.lever?.LeverStatus == LeverStatus.Open)
            {
                this.item = item;
                return true;
            }

            return false;
        }

        internal ItemAbstract Release()
        {
            ItemAbstract outItem = null;
            if (this.item != null && this.lever?.LeverStatus == LeverStatus.Open)
            {
                outItem = this.item;
                this.item = null;
            }

            return outItem;
        }

        public void OnTimerTick()
        {
            if (this.lever?.LeverStatus == LeverStatus.Closed && this.roaster?.RoasterStatus == RoasterStatus.On)
            {
                this.CookingTime += 1;
            }
        }

        public void OnTimeout()
        {
            // do nothing
        }

        public void OnLeverClosed()
        {            
            this.CookingTime = 0;
        }

        public void OnLeverOpened()
        {
            this.item?.Cooked(this.CookingTime);
        }
    }
}
