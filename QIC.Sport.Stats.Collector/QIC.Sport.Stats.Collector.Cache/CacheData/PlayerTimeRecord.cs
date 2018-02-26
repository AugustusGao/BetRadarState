using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class PlayerTimeRecord : BaseCacheEntity
    {
        public string MatchId;
        public string PlayerId;
        public bool IsStarting;
        public bool IsInPlay;
        public bool IsOutPlay;
        public int MinutesPlayed;

        public void CompareSet(PlayerTimeRecord record)
        {
            if (this.Equals(record)) return;

            MatchId = record.MatchId;
            PlayerId = record.PlayerId;
            IsStarting = record.IsStarting;
            IsInPlay = record.IsInPlay;
            IsOutPlay = record.IsOutPlay;
            MinutesPlayed = record.MinutesPlayed;
        }

        public override bool Equals(object entity)
        {
            PlayerTimeRecord ptr = entity as PlayerTimeRecord;

            return MatchId == ptr.MatchId &&
                   PlayerId == ptr.PlayerId &&
                   IsStarting == ptr.IsStarting &&
                   IsInPlay == ptr.IsInPlay &&
                   IsOutPlay == ptr.IsOutPlay &&
                   MinutesPlayed == ptr.MinutesPlayed;
        }
    }
}
