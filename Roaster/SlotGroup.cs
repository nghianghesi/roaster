using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{

    public class SlotGroup: IRoasterStatusChangedHandler
    {
        private List<Slot> slots = new List<Slot>();
       
        public SlotGroup(List<Slot> slots)
        {
            this.slots = slots;
        }

        public bool Receive(ItemAbstract item, int slotIdx)
        { 
            if (slotIdx < this.slots.Count)
            {
                return this.slots[slotIdx].Receive(item);
            }

            return false;
        }

        public ItemAbstract Release(int slotIdx)
        {
            if (slotIdx < this.slots.Count)
            {
                return this.slots[slotIdx].Release();
            }

            return null;
        }

        internal void StartCook(RoasterStatus roasterStatus)
        {
            foreach(Slot slot in this.slots)
            {
                slot.StartCooking(roasterStatus);
            }
        }

        internal void EndCook()
        {
            foreach (Slot slot in this.slots)
            {
                slot.EndCooking();
            }
        }

        public void OnTimerClick()
        {
            foreach (Slot slot in this.slots)
            {
                slot.OnTimerClick();
            }
        }

        public void OnRoasterOn()
        {
            foreach (Slot slot in this.slots)
            {
                slot.OnRoasterOn();
            }
        }

        public void OnRoasterOff()
        {
            foreach (Slot slot in this.slots)
            {
                slot.OnRoasterOff();
            }
        }
    }
}
