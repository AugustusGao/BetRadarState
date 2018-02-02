using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using QIC.Sport.Stats.Collector.BetRadar.League;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Organizer
{
    public class OrganizerManager : BaseWorkManager
    {
        private ILog logger = LogManager.GetLogger(typeof(OrganizerManager));
        public override void ExecuteTask(BaseParam param)
        {
        }

        public override void ProcessData(BaseData data)
        {

        }
    }
}
