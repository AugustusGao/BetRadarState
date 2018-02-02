using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache
{
    public interface ICacheManager
    {
        BaseCacheEntity AddOrGetCacheEntity(string key);
        void Remove(BaseCacheEntity entity);
    }
}
