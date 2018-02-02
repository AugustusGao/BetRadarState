using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CombinedData;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class SeasonEntity
    {
        public string SeasonId;
        public string SportId;
        public string SeasonName;
        public List<int> SeasonTypeList = new List<int>();
        public List<string> TeamIdList = new List<string>();
        public int TotalRound;
        public int CurrentRound;
        public int Status;

    }
}
