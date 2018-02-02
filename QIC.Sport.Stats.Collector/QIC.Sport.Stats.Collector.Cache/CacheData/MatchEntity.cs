using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class MatchEntity
    {
        public string MatchId;
        public string SeasonId;
        public int SeasonType;
        public string SportId;
        public string HomeId;
        public string AwayId;
        public string MatchDate;
        public string ExtendedData;
        public MatchResultEntity MatchResult;
    }
}
