using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public class Roaster
    {
        private List<SlotGroup> groups = new List<SlotGroup>();
        private List<Lever> levers = new List<Lever>();
        private List<Timer> timers = new List<Timer>();
        private List<IRoasterStatusChangedHandler> roasterStatusChangedHandlers = new List<IRoasterStatusChangedHandler>();

        public RoasterStatus RoasterStatus {
            get;
            private set;
        }

        public Roaster(List<SlotGroup> groups, List<Timer> timers, List<Lever> levers)
        {
            // verify input & count match
            this.groups = groups;
            this.timers = timers;
            this.levers = levers;
            for(int idx = 0; idx<this.levers.Count; idx++)
            {
                this.levers[idx].SlotGroup = groups[idx];                
                this.levers[idx].Timer = this.timers[idx];
                this.timers[idx].Lever = this.levers[idx];
                this.timers[idx].SlotGroup = this.groups[idx];
            }

            roasterStatusChangedHandlers.AddRange(this.timers);
            roasterStatusChangedHandlers.AddRange(this.groups);
        }

        public void ToggleStatus()
        {
            if (this.RoasterStatus == RoasterStatus.Off)
            {
                this.RoasterStatus = RoasterStatus.On;
                foreach(IRoasterStatusChangedHandler handler in this.roasterStatusChangedHandlers)
                {
                    handler.OnRoasterOn();
                }
            }
            else
            {
                this.RoasterStatus = RoasterStatus.Off;
                foreach (IRoasterStatusChangedHandler handler in this.roasterStatusChangedHandlers)
                {
                    handler.OnRoasterOff();
                }
            }
        }

        public bool Receive(ItemAbstract item, int groupIdx, int slotIdx)
        {
            if (groupIdx < this.groups.Count)
            {
                return this.groups[groupIdx].Receive(item, slotIdx);
            }
            return false;
        }

        public ItemAbstract Release(int groupIdx, int slotIdx)
        {
            if (groupIdx < this.groups.Count)
            {
                return this.groups[groupIdx].Release(slotIdx);
            }

            return null;
        }

        public void Settimer(int timerIdx, int timeout)
        {
            if (timerIdx < this.timers.Count)
            {
                this.timers[timerIdx].Timeout = timeout;
            }
        }

        public void CloseLever(int leverIdx)
        {
            if (leverIdx < this.levers.Count)
            {
                this.levers[leverIdx].Close(this.RoasterStatus);
            }
        }

        public void OpenLever(int leverIdx)
        {
            if (leverIdx < this.levers.Count)
            {
                this.levers[leverIdx].Open();
            }
        }
    }

    public enum RoasterStatus
    {
        On, Off
    }

    public interface IRoasterStatusChangedHandler
    {
        public void OnRoasterOn();
        public void OnRoasterOff();
    }
}
