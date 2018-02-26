using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class PlayerStatisticsRecord : BaseCacheEntity
    {
        public string MatchId;
        public string PlayerId;
        public int Goals;
        public int Assists;
        public int YellowCard;
        public int YellowRedCard;
        public int RedCard;

        public void CompareSet(PlayerStatisticsRecord record)
        {
            if (this.Equals(record)) return;
            MatchId = record.MatchId;
            PlayerId = record.PlayerId;
            Goals = record.Goals;
            Assists = record.Assists;
            YellowCard = record.YellowCard;
            YellowRedCard = record.YellowRedCard;
            RedCard = record.RedCard;
        }

        public override string GetKey()
        {
            return PlayerId + "_" + MatchId;
        }

        public override bool Equals(object entity)
        {
            PlayerStatisticsRecord psr = entity as PlayerStatisticsRecord;
            return MatchId == psr.MatchId &&
                   PlayerId == psr.PlayerId &&
                   Goals == psr.Goals &&
                   Assists == psr.Assists &&
                   YellowCard == psr.YellowCard &&
                   YellowRedCard == psr.YellowRedCard &&
                   RedCard == psr.RedCard;
        }
    }
}
