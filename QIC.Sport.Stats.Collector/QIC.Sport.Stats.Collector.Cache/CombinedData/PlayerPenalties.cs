using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CombinedData
{
    /// <summary>
    /// 联赛下该球员的点球数
    /// </summary>
    public class PlayerPenalties : BaseCacheEntity
    {
        public string SeasonId;
        public string PlayerId;
        public string Penalties;
        public override string GetKey()
        {
            return PlayerId + "_" + SeasonId;
        }

        public void ComparePlayerPenalties(string penalties)
        {
            if (Penalties == penalties) return;
            Penalties = penalties;
        }
    }
}
