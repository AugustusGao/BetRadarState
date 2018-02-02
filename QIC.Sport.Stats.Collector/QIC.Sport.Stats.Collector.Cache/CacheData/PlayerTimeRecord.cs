using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    class PlayerTimeRecord
    {
        public string MatchId;
        public string PlayerId;
        public bool IsStarting;
        public bool IsInPlay;
        public bool IsOutPlay;
        public int MinutesPlayed;
    }
}
