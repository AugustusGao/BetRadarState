using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class OrganizerEntity : BaseCacheEntity
    {
        private List<string> seasonIdList = new List<string>();
        public string OrganizerName;
        public string ContinentName;
        public string SportName = "Soccer";

        public string ContinentId;
        public string OrganizerId;
        public string SportId = "1";

        public Dictionary<string, List<string>> CompareSetSeasonIds(List<string> list)
        {
            var adds = list.Except(seasonIdList).ToList();
            var dels = seasonIdList.Except(list).ToList();

            seasonIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }
    }
}
