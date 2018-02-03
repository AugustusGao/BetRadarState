using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Manager
{
    public class TeamManager : BaseWorkManager
    {
        public DateTime ins = DateTime.Now;
        public override void ExecuteTask(BaseParam param)
        {
            BRBaseParam bp = param as BRBaseParam;

            var url = bp.GetUrl();

            var html = RequestPage(url);

            BRData data = new BRData() { Param = param, Html = html };
            EnQueueData(data);
        }

        public override void ProcessData(BaseData data)
        {
            BRBaseParam param = data.Param as BRBaseParam;
            BRData bd = data as BRData;

            var handle = HandleFactory.CreateHandle(param.HandleType);
            handle.Process(bd);
        }
    }
}
