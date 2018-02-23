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

        public void Compare(PlayerStatisticsRecord record)
        {
            if (this.Equal(record)) return;
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
    }
}
