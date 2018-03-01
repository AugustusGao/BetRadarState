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
        public string TotalRound;
        public string CurrentRound;
        public int Status;
        public List<string> SeasonTypeList = new List<string>();
        public List<string> TeamIdList = new List<string>();

        public void CompareSetSeasonInfo(SeasonEntity seasonEntity)
        {
            if (
                SeasonId == seasonEntity.SeasonId &&
                SportId == seasonEntity.SportId &&
                SeasonName == seasonEntity.SeasonName &&
                TotalRound == seasonEntity.TotalRound &&
                CurrentRound == seasonEntity.CurrentRound
                ) return;

            SeasonId = seasonEntity.SeasonId;
            SportId = seasonEntity.SportId;
            SeasonName = seasonEntity.SeasonName;
            TotalRound = seasonEntity.TotalRound;
            CurrentRound = seasonEntity.CurrentRound;
        }
        public Dictionary<string, List<string>> CompareSetTeamIdList(List<string> list)
        {
            var adds = list.Except(TeamIdList).ToList();
            var dels = TeamIdList.Except(list).ToList();

            TeamIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }
        public Dictionary<string, List<string>> CompareSetSeasonTypeList(List<string> list)
        {
            var adds = list.Except(SeasonTypeList).ToList();
            var dels = SeasonTypeList.Except(list).ToList();

            SeasonTypeList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }
    }
}
