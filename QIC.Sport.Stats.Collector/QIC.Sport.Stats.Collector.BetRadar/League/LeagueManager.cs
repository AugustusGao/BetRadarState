using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.League
{
    public class LeagueManager : BaseWorkManager
    {
        private ILog logger = LogManager.GetLogger(typeof(LeagueManager));
        public override void ExecuteTask(BaseParam param)
        {
            EnQueueData(new BaseData());
            logger.Debug("LeagueManager ExecuteTask");
            //base.ExecuteTask(param);
        }

        public override void ProcessData(BaseData data)
        {
            var tm = IocUnity.GetService<IWorkManager>("TeamManager");
            tm.AddOrUpdateParam(new BaseParam());
            logger.Debug("LeagueManager ProcessData");
            //base.ProcessData(data);
        }
    }
}
