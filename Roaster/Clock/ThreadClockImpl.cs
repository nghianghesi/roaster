using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Roaster
{
    public class ThreadClockImpl : Clock.IClock, IDisposable
    {
        private List<Clock.IClockTickHandler> handlers = new List<Clock.IClockTickHandler>();
        private bool Shutdown = false;
        private Task clockTask = null;
        public ThreadClockImpl(int msPerTick = 1000)
        {
            msPerTick = (msPerTick > 0) ? msPerTick : 1000;
            this.clockTask = Task.Factory.StartNew(()=>
            {
                int timeToWait = msPerTick;
                while (!this.Shutdown)
                {
                    if (timeToWait > 0)
                    {
                        System.Threading.Thread.Sleep(timeToWait);
                    }
                    long startTick = System.DateTime.Now.Ticks;

                    List<Clock.IClockTickHandler> cloneHandlers;

                    lock (this.handlers)
                    {
                        cloneHandlers = this.handlers.ToList();
                    }

                    foreach (Clock.IClockTickHandler h in cloneHandlers)
                    {
                        h.OnClockTick();
                    }

                    startTick = (System.DateTime.Now.Ticks - startTick) / 10000;
                    timeToWait = msPerTick - (int)startTick;
                }
            });
        }

        public void AddHandler(Clock.IClockTickHandler h)
        {
            lock (this.handlers)
            {
                if (!this.handlers.Contains(h))
                {
                    this.handlers.Add(h);
                }
            }
        }
        public void RemoveHandler(Clock.IClockTickHandler h)
        {
            lock (this.handlers)
            {
                this.handlers.Remove(h);
            }
        }

        public void Dispose()
        {
            this.Shutdown = true;
            this.clockTask.Wait();
        }
    }
}
