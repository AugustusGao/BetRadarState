using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Handle;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar
{
    public static class HandleFactory
    {
        public static IHandle CreateHandle(int handleType)
        {
            IHandle handle;
            switch ((RBHandleType)handleType)
            {
                case RBHandleType.Organizer:
                    handle = new OrganizerHandle();
                    break;
                case RBHandleType.Season:
                    handle = new SeasonHandle();
                    break;
                default:
                    handle = null;
                    break;
            }
            return handle;
        }
    }
}
