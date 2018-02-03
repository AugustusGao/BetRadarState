using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class PlayerTransferHandle : BaseHandle, IHandle
    {
        //  解析转会记录，缓存到PlayerEntity中
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            throw new NotImplementedException();
        }
    }
}
