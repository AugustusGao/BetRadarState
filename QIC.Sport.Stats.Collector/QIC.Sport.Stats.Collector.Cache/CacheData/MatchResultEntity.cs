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
        public override bool Equals(object entity)
        {
            MatchResultEntity mr = entity as MatchResultEntity;
            return MatchId == mr.MatchId &&
                   SportId == mr.SportId &&
                   Result == mr.Result &&
                   ResultStatus == mr.ResultStatus;
        }
    }
}
