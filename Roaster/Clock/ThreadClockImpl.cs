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
        public ThreadClockImpl(int sleepTime = 1000)
        {
            sleepTime = (sleepTime > 0) ? sleepTime : 1000;
            this.clockTask = Task.Factory.StartNew(()=>
            {
                while (!this.Shutdown)
                {
                    System.Threading.Thread.Sleep(sleepTime);

                    List<Clock.IClockTickHandler> cloneHandlers;

                    lock (this.handlers)
                    {
                        cloneHandlers = this.handlers.ToList();
                    }

                    foreach (Clock.IClockTickHandler h in cloneHandlers)
                    {
                        h.OnClockTick();
                    }
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
