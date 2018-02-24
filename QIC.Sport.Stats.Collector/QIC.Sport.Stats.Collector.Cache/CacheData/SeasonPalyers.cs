using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CombinedData;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    /// <summary>
    /// 不同赛季下的队员统计信息（1、最佳射手排名）
    /// </summary>
    public class SeasonPalyers
    {
        public string SeasonId;
        public List<string> TopScorePlayerList = new List<string>();     //  list名次球员Id
    }
}
