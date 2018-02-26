using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class MatchEntity : BaseCacheEntity
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

        public void CompareSetMatchInfo(MatchEntity matchEntity)
        {
            if (Equals(matchEntity)) return;
            MatchId = matchEntity.MatchId;
            SeasonId = matchEntity.SeasonId;
            SeasonType = matchEntity.SeasonType;
            SportId = matchEntity.SportId;
            HomeId = matchEntity.HomeId;
            AwayId = matchEntity.AwayId;
            MatchDate = matchEntity.MatchDate;
            ExtendedData = matchEntity.ExtendedData;
        }

        public void CompareSetMatchResult(MatchResultEntity matchResult)
        {
            if (matchResult == null) return;
            if (MatchResult == null || !MatchResult.Equals(matchResult)) MatchResult = matchResult;
        }

        public override bool Equals(object entity)
        {
            MatchEntity me = entity as MatchEntity;
            return MatchId == me.MatchId &&
                   SeasonId == me.SeasonId &&
                   SeasonType == me.SeasonType &&
                   SportId == me.SportId &&
                   HomeId == me.HomeId &&
                   AwayId == me.AwayId &&
                   MatchDate == me.MatchDate &&
                   ExtendedData == me.ExtendedData;
        }
    }
}




