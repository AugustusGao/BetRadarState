using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class MatchResultEntity : BaseCacheEntity
    {
        public string MatchId;
        public string SportId;
        public string Result;
        public int ResultStatus;
    }
}
