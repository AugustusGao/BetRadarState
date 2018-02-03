using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CombinedData
{
    /// <summary>
    /// 积分榜,其他信息可由teamid获得
    /// </summary>
    public class TeamRank
    {
        public string SeasonId;
        public string TeamId;
        public string TeamPoints;
        public string Description;
    }
}
