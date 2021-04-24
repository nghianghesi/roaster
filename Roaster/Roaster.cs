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

        public RoasterStatus RoasterStatus {
            get;
            private set;
        }

        public Roaster(int numberGroups, int numberOfSlotPerGroup)
        {
            // verify input & count match
            for(int idx = 0; idx < numberGroups; idx++)
            {
                Lever lever = new Lever();
                Timer timer = new Timer(this);
                timer.TimeoutHandlers.Add(lever);
                lever.LeverStatusChangedHandlers.Add(timer);

                SlotGroup group = new SlotGroup(this, timer, lever, numberOfSlotPerGroup);
                this.groups.Add(group);
                this.levers.Add(lever);
                this.timers.Add(timer);
            }
        }

        public void ToggleStatus()
        {
            if (this.RoasterStatus == RoasterStatus.Off)
            {
                this.RoasterStatus = RoasterStatus.On;
            }
            else
            {
                this.RoasterStatus = RoasterStatus.Off;
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
                this.levers[leverIdx].Close();
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
}
