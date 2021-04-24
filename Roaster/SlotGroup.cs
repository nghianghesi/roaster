using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{

    public class SlotGroup
    {
        private List<Slot> slots = new List<Slot>();
       
        public SlotGroup(Roaster roaster, Timer timer, Lever lever, int numberOfSlotPerGroup)
        {
            for (int idx = 0; idx < numberOfSlotPerGroup; idx++)
            {
                this.slots.Add(new Slot(roaster, timer, lever));
            }
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
    }
}
