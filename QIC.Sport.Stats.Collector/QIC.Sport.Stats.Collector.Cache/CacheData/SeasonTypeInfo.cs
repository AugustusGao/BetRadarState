using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class SeasonTypeInfo : BaseCacheEntity
    {
        public string SeasonId;
        public string SeasonType;
        public string Name;
        public string ExtendedData;
        public List<string> TableIdList = new List<string>();
        public List<string> MatchIdList;

        public void CompareSet(SeasonTypeInfo info)
        {
            if (SeasonId == info.SeasonId &&
                SeasonType == info.SeasonType &&
                Name == info.Name &&
                ExtendedData == info.ExtendedData) return;

            SeasonId = info.SeasonId;
            SeasonType = info.SeasonType;
            Name = info.Name;
            ExtendedData = info.ExtendedData;
        }
        public Dictionary<string, List<string>> CompareSetTabelIdList(List<string> list)
        {
            var adds = list.Except(TableIdList).ToList();
            var dels = TableIdList.Except(list).ToList();

            TableIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }

        public override string GetKey()
        {
            return SeasonId + "_" + SeasonType;
        }
    }
}
