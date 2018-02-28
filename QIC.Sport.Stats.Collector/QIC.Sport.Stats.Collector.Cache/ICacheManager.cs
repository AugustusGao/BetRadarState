using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache
{
    public interface ICacheManager
    {
        T AddOrGetCacheEntity<T>(string key) where T : BaseCacheEntity, new();
        void Remove(string key);
        int Count { get; }
    }
}
