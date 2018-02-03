using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CombinedData;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class SeasonEntity : BaseCacheEntity
    {
        public string SeasonId;
        public string SportId;
        public string SeasonName;
        public List<int> SeasonTypeList = new List<int>();
        public List<string> TeamIdList = new List<string>();
        public int TotalRound;
        public int CurrentRound;
        public int Status;
        /// <summary>
        /// 按赛季下的队伍统计信息（1、积分排名）
        /// </summary>
        public SeasonTeams SeasonTeams = new SeasonTeams();
        /// <summary>
        /// 按赛季下的队员统计信息（1、最佳射手排名）
        /// </summary>
        public SeasonPalyers SeasonPalyers = new SeasonPalyers();

        public Dictionary<string, List<string>> CompareTeamIdList(List<string> list)
        {
            var adds = list.Except(TeamIdList).ToList();
            var dels = TeamIdList.Except(list).ToList();

            TeamIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }

        public void AddOrUpdateTeamRank(TeamRank teamRank)
        {

        }
        public void AddOrUpdateTopScorePlayer(int rankNum, string playerId)
        {

        }
    }
}
