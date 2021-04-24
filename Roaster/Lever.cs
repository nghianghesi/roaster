using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roaster
{
    public class Lever : ITimmeoutHandler
    {
        public LeverStatus LeverStatus { get; private set; }
        internal readonly List<ILeverStatusChangedHandler> LeverStatusChangedHandlers = new List<ILeverStatusChangedHandler>();

        public void Close()
        {
            if (LeverStatus != LeverStatus.Closed)
            {
                this.LeverStatus = LeverStatus.Closed;
                foreach(var l in LeverStatusChangedHandlers)
                {
                    l.OnLeverClosed();
                }
            }
        }

        public void Open()
        {
            if (LeverStatus != LeverStatus.Open)
            {
                this.LeverStatus = LeverStatus.Open;
                foreach (var l in LeverStatusChangedHandlers)
                {
                    l.OnLeverOpened();
                }
            }
        }

        public void OnTimerTick()
        {
            // do nothing
        }

        public void OnTimeout()
        {
            this.Open();
        }
    }

    public enum LeverStatus
    {
        Open, Closed
    }

    public interface ILeverStatusChangedHandler
    {
        public void OnLeverClosed();
        public void OnLeverOpened();
    }
}
