using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class PlayerEntity : BaseCacheEntity
    {
        public int PlayerId;
        public string FullName;
        public string Country;
        public string SecondaryCountry;
        public DateTime Birth;
        public string Age;
        public string Height;
        public string Weight;
        public string Position;
        public string ShirtNumber;
        public string TeamName;
        public string PreferredFoot;
        public List<TransferHistory> TransferHistorys;
    }

    public class TransferHistory
    {
        public int TeamId;
        public string DuringTime;
        public string Description;

    }
}
