using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CombinedData;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    /// <summary>
    /// 不同赛季下的队伍统计信息（1、积分排名）
    /// </summary>
    public class SeasonTeams
    {
        public string SeasonId;
        public Dictionary<string, TeamRank> RankDic = new Dictionary<string, TeamRank>();  //  kv = 名次--队伍积分数据
    }

}
