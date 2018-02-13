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
    public class LeagueManager : BaseWorkManager
    {
        public override void ExecuteTask(BaseParam baseParam)
        {
            BRBaseParam bp = baseParam as BRBaseParam;

            var url = bp.GetUrl();

            var html = RequestPage(url);

            BRData data = new BRData() { Param = baseParam, Html = html };
            EnQueueData(data);

            //  测试调整请求间隔时间
            IntervalsTime = 5 * 1000;
            
            //  最佳射手不需要请求数据，可以根据球员参赛记录加上最佳名单的点球数生成最佳射手名单

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
