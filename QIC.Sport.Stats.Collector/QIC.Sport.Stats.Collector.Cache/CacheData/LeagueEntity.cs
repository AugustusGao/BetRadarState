using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class LeagueEntity : BaseCacheEntity
    {
        public List<string> SeasonIdList = new List<string>();
        public string LeagueName;
        public string SportId = "1";
        public string CurrentSeasonId;

        public Dictionary<string, List<string>> CompareSetSeasonIds(List<string> list)
        {
            var adds = list.Except(SeasonIdList).ToList();
            var dels = SeasonIdList.Except(list).ToList();

            SeasonIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }

        public void AddSeasonId(string seasonId, bool isCurrent = false)
        {
            if (SeasonIdList.Contains(seasonId)) return;
            SeasonIdList.Add(seasonId);
            if (isCurrent) CurrentSeasonId = seasonId;
        }
        public override string GetKey()
        {
            return CurrentSeasonId;
        }
    }
}
