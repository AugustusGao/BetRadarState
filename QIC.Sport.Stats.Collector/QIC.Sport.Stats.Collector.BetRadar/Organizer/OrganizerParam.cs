using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Organizer
{
    public class OrganizerParam : BaseParam
    {
        public string IndexUrl;
        public override string GetKey()
        {
            return base.GetKey();
        }
    }
}
