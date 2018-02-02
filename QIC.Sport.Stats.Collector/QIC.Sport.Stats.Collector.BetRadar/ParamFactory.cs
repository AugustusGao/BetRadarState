using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Param;

namespace QIC.Sport.Stats.Collector.BetRadar
{
    public static class ParamFactory
    {
        public static BRBaseParam CreateParam(int handleType)
        {
            BRBaseParam param;
            switch ((RBHandleType)handleType)
            {
                case RBHandleType.Organizer:
                    param = new OrganizerParam() { HandleType = handleType };
                    break;
                default:
                    param = null;
                    break;
            }
            return param;
        }
    }
}
