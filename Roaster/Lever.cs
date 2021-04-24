using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    enum LeverStatus
    {
        Open, Closed
    }
    public class Lever
    {
        private LeverStatus LeverStatus { get; set; }

        internal Timer Timer
        { get; set; }


        internal SlotGroup SlotGroup
        {
            get; set;
        }

        public void Close(RoasterStatus roasterStatus)
        {
            if (LeverStatus != LeverStatus.Closed)
            {
                this.LeverStatus = LeverStatus.Closed;
                this.SlotGroup.StartCook(roasterStatus);
                this.Timer.StartCountDown(roasterStatus);
            }
        }

        public void Open()
        {
            if (LeverStatus != LeverStatus.Open)
            {
                this.LeverStatus = LeverStatus.Open;
                this.SlotGroup.EndCook();
            }
        }
    }
}
