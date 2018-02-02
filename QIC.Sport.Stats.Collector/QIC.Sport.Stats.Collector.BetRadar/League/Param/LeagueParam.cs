using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.League
{
    public class LeagueParam : BaseParam
    {
        public LeagueParam()
        {
            //DataType = DataType.League;
        }
        public override string GetKey()
        {
            // todo key = id_DataType
            return base.GetKey();
        }
    }
}
