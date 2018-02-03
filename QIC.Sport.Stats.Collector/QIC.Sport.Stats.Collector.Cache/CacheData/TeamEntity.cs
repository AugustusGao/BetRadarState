using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class TeamEntity : BaseCacheEntity
    {
        public string TeamId;
        public string TeamName;
        public string Venue;            //  主场
        public string Manager;          //  教练
        public string Mark;             //  队标
    }
}
