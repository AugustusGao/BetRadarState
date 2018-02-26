using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar
{
    [ExceptionHandlerAttribute(Order = 1)]
    public interface IHandle
    {
        void Process(BaseData data);
    }
}
