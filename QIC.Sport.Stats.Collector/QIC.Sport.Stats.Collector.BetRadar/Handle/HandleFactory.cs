using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public static class HandleFactory
    {
        public static IHandle CreateHandle(DataType dataType)
        {
            IHandle handle;
            switch (dataType)
            {
                case DataType.Organizer:
                    handle = new OrganizerHandle();
                    break;
                default:
                    handle = null;
                    break;
            }
            return handle;
        }
    }
}
