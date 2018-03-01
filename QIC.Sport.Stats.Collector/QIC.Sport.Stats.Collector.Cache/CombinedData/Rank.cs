using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CombinedData
{
    public class SeasonTableRank : TeamRank
    {
        public string TableId;
    }

    public class SeasonTeamRank : TeamRank
    {
        public string SeasonId;
    }

    public class TeamRank
    {
        public string TeamId;
        public string TeamPoints;
        public string Description;
    }
}
