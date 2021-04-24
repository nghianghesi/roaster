using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    public interface IClockTickHandler
    {
        public void OnClockTick();
    }

    public interface IClock
    {
        public void AddHandler(Clock.IClockTickHandler h);
        public void RemoveHandler(Clock.IClockTickHandler h);
    }
}
