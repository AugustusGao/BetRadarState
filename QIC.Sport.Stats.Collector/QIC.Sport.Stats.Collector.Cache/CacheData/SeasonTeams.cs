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
    public class SeasonTeams : BaseCacheEntity
    {
        public string SeasonId;
        public List<SeasonTeamRank> TeamRankList = new List<SeasonTeamRank>();  //  list名次队伍积分数据

        public void CompareSetTeamRank(List<SeasonTeamRank> list)
        {
            bool isChanged;
            if (TeamRankList.Count != list.Count)
            {
                TeamRankList = list;
                isChanged = true;
            }
            else if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (TeamRankList[i].TeamId == list[i].TeamId && TeamRankList[i].TeamPoints == list[i].TeamPoints && TeamRankList[i].Description == list[i].Description) continue;
                    TeamRankList = list;
                    isChanged = true;
                    break;
                }
            }
        }

        public override string GetKey()
        {
            return SeasonId;
        }
    }

}
