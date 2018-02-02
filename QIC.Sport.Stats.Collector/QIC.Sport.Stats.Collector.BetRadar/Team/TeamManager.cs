using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.League;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Team
{
    public class TeamManager : BaseWorkManager
    {
        public DateTime ins = DateTime.Now;
        private ILog logger = LogManager.GetLogger(typeof(TeamManager));
        public override void ExecuteTask(BaseParam param)
        {
            logger.Debug("TeamManager ExecuteTask");
            //base.ExecuteTask(param);
        }

        public override void ProcessData(BaseData data)
        {
            var tm = IocUnity.GetService<IWorkManager>("TeamManager");

            logger.Debug("TeamManager ProcessData");
            //base.ProcessData(data);
        }
    }
}
