using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public class Timer: IRoasterStatusChangedHandler, Clock.IClockTickHandler
    {
        private TimerStatus status;
        internal Lever Lever
        {
            get; 
            set;
        }
        internal SlotGroup SlotGroup
        {
            get;
            set;
        }

        public int Timeout
        { 
            get; 
            set;
        }

        internal void ResetCountDown()
        {
            if (this.status != TimerStatus.Initial)
            {
                this.Timeout = 0;
                this.status = TimerStatus.Initial;
                DI.Resolver.Resolve<Clock.IClock>()?.RemoveHandler(this);
            }
        }

        internal void StartCountDown(RoasterStatus roasterStatus)
        {
            if (roasterStatus == RoasterStatus.On)
            {
                this.status = TimerStatus.CountingDown;
            }
            else
            {
                this.status = TimerStatus.Pended;
            }

            DI.Resolver.Resolve<Clock.IClock>()?.AddHandler(this);
        }

        public void OnRoasterOn()
        {
            if (this.status == TimerStatus.Pended)
            {
                this.status = TimerStatus.CountingDown;
            }
        }

        public void OnRoasterOff()
        {
            if (this.status == TimerStatus.CountingDown)
            {
                this.status = TimerStatus.Pended;
            }
        }

        public void OnClockTick()
        {
            if (this.status == TimerStatus.CountingDown)
            {
                this.Timeout -= 1;
                this.SlotGroup.OnTimerClick();
                if(this.Timeout<=0)
                {
                    this.ResetCountDown();
                    this.Lever.Open();
                }
            }
        }
    }

    public enum TimerStatus
    {
        Initial, CountingDown, Pended
    }
}
