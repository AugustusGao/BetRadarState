using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Manager
{
    public class PlayerManager : BaseWorkManager
    {
        public override void ExecuteTask(BaseParam param)
        {
            EnQueueData(new BaseData());
            logger.Debug("PlayerManager ExecuteTask");
            //base.ExecuteTask(param);
        }

        public override void ProcessData(BaseData data)
        {
            var tm = IocUnity.GetService<IWorkManager>("PlayerManager");
            //tm.AddOrUpdateParam(new BaseParam());
            logger.Debug("PlayerManager ProcessData");
            //base.ProcessData(data);
        }
    }
}
