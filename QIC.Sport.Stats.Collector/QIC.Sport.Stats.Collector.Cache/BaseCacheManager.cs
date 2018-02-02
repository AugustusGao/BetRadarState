using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache
{
    public class BaseCacheManager
    {
        protected ConcurrentDictionary<string, BaseCacheEntity> DicCacheData = new ConcurrentDictionary<string, BaseCacheEntity>();
    }
}
