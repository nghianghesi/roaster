using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public class Timer : Clock.IClockTickHandler, ILeverStatusChangedHandler
    {
        internal readonly List<ITimmeoutHandler> TimeoutHandlers = new List<ITimmeoutHandler>();

        public int Timeout
        {
            get;
            set;
        }

        private Roaster Roaster
        {
            get;set;
        }

        public Timer(Roaster roaster)
        {
            this.Roaster = roaster;
        }

        public void OnClockTick()
        {
            foreach (var h in this.TimeoutHandlers)
            {
                h.OnTimerTick();
            }

            if (this.Roaster?.RoasterStatus == RoasterStatus.On)
            {
                this.Timeout -= 1;
                if(this.Timeout<=0)
                {
                    this.Timeout = 0;
                    DI.Resolver.Resolve<Clock.IClock>()?.RemoveHandler(this);
                    foreach(var h in this.TimeoutHandlers)
                    {
                        h.OnTimeout();
                    }
                }
            }
        }

        public void OnLeverClosed()
        {
            // start countdown
            DI.Resolver.Resolve<Clock.IClock>()?.AddHandler(this);
        }

        public void OnLeverOpened()
        {
            // stop countdown
            DI.Resolver.Resolve<Clock.IClock>()?.RemoveHandler(this);
        }
    }

    public interface ITimmeoutHandler
    {
        public void OnTimerTick();
        public void OnTimeout();
    }
}
